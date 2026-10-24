using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using ArchUnitNET.Domain.PlantUml.Exceptions;

namespace ArchUnitNET.Domain.PlantUml.Export
{
    public class PlantUmlFileBuilder
    {
        private readonly PlantUmlDiagram _diagram = new PlantUmlDiagram();

        private readonly List<PlantUmlDependency> _dependencies = new List<PlantUmlDependency>();

        private List<Slice> _sliceList = new List<Slice>();

        /// <summary>
        /// The name each slice is drawn under, and the root package it is nested in, if any.
        /// </summary>
        private Dictionary<Slice, (string Name, string NameSpace)> _diagramNames =
            new Dictionary<Slice, (string Name, string NameSpace)>();

        public string AsString(RenderOptions renderOptions = null)
        {
            if (renderOptions == null)
            {
                renderOptions = new RenderOptions();
            }

            return _diagram.GetPlantUmlString(renderOptions);
        }

        public void WriteToFile(string path, RenderOptions renderOptions = null)
        {
            if (renderOptions == null)
            {
                renderOptions = new RenderOptions();
            }
            if (!renderOptions.Overwrite && File.Exists(path))
            {
                throw new FileAlreadyExistsException(
                    "File already exists and overwriting is disabled."
                );
            }

            Directory.CreateDirectory(
                Path.GetDirectoryName(path) ?? throw new ArgumentException("Invalid path.")
            );

            using (var sw = File.CreateText(path))
            {
                sw.Write(AsString(renderOptions));
            }
        }

        public PlantUmlFileBuilder WithElements(IEnumerable<IPlantUmlElement> elements)
        {
            _diagram.AddElements(elements);
            return this;
        }

        public PlantUmlFileBuilder WithDependenciesFrom(
            IEnumerable<IType> types,
            GenerationOptions generationOptions = null
        )
        {
            var typeList = types.Distinct().ToList();
            if (generationOptions == null)
            {
                generationOptions = new GenerationOptions();
            }

            var nodes = new Dictionary<IType, IPlantUmlElement>();

            foreach (var type in typeList)
            {
                var filteredTypeDependencies = type.Dependencies.Where(dep =>
                    (generationOptions.DependencyFilter?.Invoke(dep) ?? true)
                    && !Equals(dep.Origin, dep.Target)
                );

                if (!generationOptions.IncludeDependenciesToOther)
                {
                    filteredTypeDependencies = filteredTypeDependencies.Where(dep =>
                        typeList.Contains(dep.Target)
                    );
                }

                var plantUmlDependencies = filteredTypeDependencies
                    .Select(dep => new PlantUmlDependency(
                        dep.Origin.FullName,
                        dep.Target.FullName,
                        DependencyType.OneToOne
                    ))
                    .Distinct();
                _dependencies.AddRange(plantUmlDependencies);

                if (type is Interface)
                {
                    nodes.Add(type, new PlantUmlInterface(type.FullName));
                }
                else
                {
                    var plantUmlClass = new PlantUmlClass(type.FullName);
                    foreach (var member in type.Members)
                    {
                        plantUmlClass.AddField(member.Name);
                    }

                    nodes.Add(type, plantUmlClass);
                }
            }

            var nodeElements = generationOptions.IncludeNodesWithoutDependencies
                ? nodes.Values
                : nodes
                    .Where(node =>
                        _dependencies
                            .SelectMany(dep => new[] { dep.Origin, dep.Target })
                            .Contains(node.Key.FullName)
                    )
                    .Select(node => node.Value);

            _diagram.AddElements(nodeElements);
            _diagram.AddElements(_dependencies);

            return this;
        }

        /// <summary>
        /// Draws the given slices as components, with an arrow from one slice to another wherever
        /// one of its types depends on one of the other's.
        /// </summary>
        /// <param name="slices">The slices to draw, e.g. from <c>GivenSlices.GetObjects</c>.</param>
        /// <param name="generationOptions">Filters and layout options; the defaults if <c>null</c>.
        /// With <see cref="GenerationOptions.NestSlicesInPackages"/>, slices from a slice pattern
        /// are nested in <c>package</c> blocks along the namespace they matched.</param>
        /// <exception cref="ArgumentException">Two slices would be drawn under the same name.</exception>
        public PlantUmlFileBuilder WithDependenciesFrom(
            IEnumerable<Slice> slices,
            GenerationOptions generationOptions = null
        )
        {
            _sliceList = slices.Distinct().ToList();

            var nodes = new Dictionary<Slice, IPlantUmlElement>();
            if (generationOptions == null)
            {
                generationOptions = new GenerationOptions();
            }

            AssignDiagramNames(generationOptions.NestSlicesInPackages);
            RemovePatternInappropriateSlices();

            foreach (var slice in _sliceList)
            {
                var sliceIsPackage = IsPackage(slice);
                var dependencyTargets = SelectDependencies(
                    slice,
                    _sliceList,
                    NameOf,
                    target => target.Types,
                    generationOptions
                );

                if (!sliceIsPackage)
                {
                    var slc = new PlantUmlSlice(NameOf(slice), NameSpaceOf(slice));
                    if (generationOptions.C4Style)
                    {
                        slc.UseS4Style();
                    }
                    nodes.Add(slice, slc);
                }

                if (!generationOptions.LimitDependencies)
                {
                    if (sliceIsPackage)
                    {
                        _dependencies.AddRange(
                            dependencyTargets.Select(target => new PlantUmlDependency(
                                NameOf(slice),
                                NameOf(target),
                                DependencyType.PackageToOne
                            ))
                        );
                    }
                    else
                    {
                        _dependencies.AddRange(
                            dependencyTargets.Select(target => new PlantUmlDependency(
                                NameOf(slice),
                                NameOf(target),
                                DependencyType.OneToOne
                            ))
                        );
                    }
                }
                else if (NameSpaceOf(slice) != null)
                {
                    if (sliceIsPackage)
                    {
                        _dependencies.AddRange(
                            dependencyTargets.Select(target => new PlantUmlDependency(
                                NameOf(slice),
                                NameOf(target),
                                DependencyType.PackageToPackageIfSameParentNamespace
                            ))
                        );
                    }
                    else
                    {
                        var dt = dependencyTargets
                            .Select(target => new PlantUmlDependency(
                                NameOf(slice),
                                NameOf(target),
                                DependencyType.OneToOneIfSameParentNamespace
                            ))
                            .Where(dependency =>
                                dependency.OriginCountOfDots() < dependency.TargetCountOfDots()
                                || _sliceList.All(slc =>
                                    NameOf(slc) == dependency.Target
                                    || !NameOf(slc).Contains(dependency.Target)
                                )
                            );
                        _dependencies.AddRange(dt);
                    }
                }
                else
                {
                    _dependencies.AddRange(
                        dependencyTargets.Select(target => new PlantUmlDependency(
                            NameOf(slice),
                            NameOf(target),
                            DependencyType.OneToOneCompact
                        ))
                    );
                }
            }

            if (!generationOptions.IncludeNodesWithoutDependencies)
            {
                RemoveNodesNotContainedInDependencies(nodes, NameOf);
            }

            if (!generationOptions.LimitDependencies)
            {
                RemoveDuplicateDependenciesWhenShowingPackages();
                ReplaceCirclesWithAppropriateDependencyType();
            }
            RemoveDuplicatedArrowsIfExist();

            _diagram.AddElements(nodes.Values);
            _diagram.AddElements(_dependencies);

            return this;
        }

        /// <summary>
        /// Draws the given namespaces as nested PlantUML namespaces, with an arrow from one
        /// namespace to another wherever one of its types depends on one of the other's. Parent
        /// namespaces that are not in the list are added so that the nesting is complete.
        /// </summary>
        /// <param name="namespaces">The namespaces to draw, e.g. <see cref="Architecture.Namespaces"/>.</param>
        /// <param name="generationOptions">Filters and layout options; the defaults if <c>null</c>.
        /// <see cref="GenerationOptions.C4Style"/> does not apply to namespaces.</param>
        public PlantUmlFileBuilder WithDependenciesFrom(
            IEnumerable<Namespace> namespaces,
            GenerationOptions generationOptions = null
        )
        {
            var namespaceList = namespaces.Distinct().ToList();
            if (generationOptions == null)
            {
                generationOptions = new GenerationOptions();
            }

            var dependencyType = generationOptions.LimitDependencies
                ? DependencyType.OneToOneCompact
                : DependencyType.OneToOne;
            var nodes = new Dictionary<Namespace, IPlantUmlElement>();

            foreach (var ns in namespaceList)
            {
                nodes.Add(ns, new PlantUmlNamespace(ns.FullName));
                _dependencies.AddRange(
                    SelectDependencies(
                            ns,
                            namespaceList,
                            target => target.FullName,
                            target => target.Types,
                            generationOptions
                        )
                        .Select(target => new PlantUmlDependency(
                            ns.FullName,
                            target.FullName,
                            dependencyType
                        ))
                );
            }

            if (!generationOptions.IncludeNodesWithoutDependencies)
            {
                RemoveNodesNotContainedInDependencies(nodes, ns => ns.FullName);
            }

            var nodeElements = nodes.Values.ToList();
            foreach (var ns in nodes.Keys)
            {
                var namespaceName = ns.FullName;
                var dotIndex = namespaceName.Length;
                var parentNamespaces = new List<string>();
                while (dotIndex != -1)
                {
                    namespaceName = namespaceName.Substring(0, dotIndex);
                    parentNamespaces.Add(namespaceName);
                    dotIndex = namespaceName.LastIndexOf('.');
                }

                parentNamespaces.Reverse();
                foreach (
                    var parent in parentNamespaces.Where(parentName =>
                        nodeElements
                            .OfType<PlantUmlNamespace>()
                            .All(element => element.Name != parentName)
                    )
                )
                {
                    nodeElements.Add(new PlantUmlNamespace(parent));
                }
            }

            if (!generationOptions.LimitDependencies)
            {
                ReplaceCirclesWithAppropriateDependencyType();
            }
            RemoveDuplicatedArrowsIfExist();

            _diagram.AddElements(
                nodeElements.OrderBy(element => ((PlantUmlNamespace)element).Name.Length)
            );
            _diagram.AddElements(_dependencies);

            return this;
        }

        /// <summary>
        /// Draws only the dependencies from and to the slices whose diagram name contains
        /// <paramref name="package"/>, and colors those slices.
        /// </summary>
        /// <param name="slices">The slices to draw, e.g. from <c>GivenSlices.GetObjects</c>.</param>
        /// <param name="package">The text to focus on. When slices are nested in packages, their
        /// diagram names are fully qualified, so this has to be as well.</param>
        /// <param name="generationOptions">Only <see cref="GenerationOptions.NestSlicesInPackages"/>
        /// is read; the defaults if <c>null</c>.</param>
        /// <exception cref="ArgumentException">The package is empty, a single dot or not contained
        /// in any slice's diagram name, or two slices would be drawn under the same name.</exception>
        public PlantUmlFileBuilder WithDependenciesFromFocusOn(
            IEnumerable<Slice> slices,
            string package,
            GenerationOptions generationOptions = null
        )
        {
            _sliceList = slices.Distinct().ToList();
            AssignDiagramNames(generationOptions?.NestSlicesInPackages ?? false);

            switch (package)
            {
                case "":
                    throw new ArgumentException("Package can't be empty");
                case ".":
                    throw new ArgumentException("Package can't contain a single dot only");
            }

            if (package[package.Length - 1] == '.')
            {
                package = package.Remove(package.Length - 1);
            }

            RemovePatternInappropriateSlices(package);

            var existPackage = _sliceList.Any(slice =>
                (NameSpaceOf(slice) + NameOf(slice)).Contains(package)
            );
            if (!existPackage)
            {
                throw new ArgumentException(
                    "The package [" + package + "] is not contained in this slice"
                );
            }

            var nodes = new Dictionary<Slice, IPlantUmlElement>();
            var defaultOptions = new GenerationOptions();

            foreach (var slice in _sliceList)
            {
                var sliceIsPackage = IsPackage(slice);
                var dependencyTargets = SelectDependencies(
                    slice,
                    _sliceList,
                    NameOf,
                    target => target.Types,
                    defaultOptions
                );

                if (sliceIsPackage)
                {
                    _dependencies.AddRange(
                        dependencyTargets.Select(target => new PlantUmlDependency(
                            NameOf(slice),
                            NameOf(target),
                            DependencyType.PackageToOne
                        ))
                    );
                }
                else
                {
                    _dependencies.AddRange(
                        dependencyTargets.Select(target => new PlantUmlDependency(
                            NameOf(slice),
                            NameOf(target),
                            DependencyType.OneToOne
                        ))
                    );
                }

                if (!sliceIsPackage)
                {
                    nodes.Add(
                        slice,
                        NameOf(slice).Contains(package)
                            ? new PlantUmlSlice(NameOf(slice), NameSpaceOf(slice), "99ffd1")
                            : new PlantUmlSlice(NameOf(slice), NameSpaceOf(slice))
                    );
                }
                else if (!NameOf(slice).Contains(package))
                {
                    nodes.Add(slice, new PlantUmlSlice(NameOf(slice) + ".", NameSpaceOf(slice)));
                }
            }

            if (!defaultOptions.IncludeNodesWithoutDependencies)
            {
                RemoveNodesNotContainedInDependencies(nodes, NameOf);
            }

            RemoveDuplicateDependenciesWhenShowingPackages();
            ReplaceCirclesWithAppropriateDependencyType();
            RemoveDuplicatedArrowsIfExist();

            for (var j = _dependencies.Count - 1; j >= 0; j--)
            {
                if (
                    !_dependencies[j].Origin.Contains(package)
                    && !_dependencies[j].Target.Contains(package)
                )
                {
                    _dependencies.RemoveAt(j);
                }
            }

            for (var i = nodes.Count - 1; i >= 0; i--)
            {
                var nodeExist = _dependencies.Any(dep =>
                    NameOf(nodes.ElementAt(i).Key) == dep.Origin
                    || NameOf(nodes.ElementAt(i).Key) == dep.Target
                );
                if (!nodeExist)
                {
                    nodes.Remove(nodes.ElementAt(i).Key);
                }
            }

            _diagram.AddElements(nodes.Values);

            var tempList = _sliceList
                .Where(slice => IsPackage(slice) && NameOf(slice).Contains(package))
                .Select(slice => new PlantUmlSlice(
                    NameOf(slice) + ".",
                    NameSpaceOf(slice),
                    "99ffd1"
                ))
                .ToList();
            if (tempList.Count > 0)
            {
                _diagram.AddElements(tempList.OrderByDescending(element => element.ToString()));
            }
            _diagram.AddElements(_dependencies);

            return this;
        }

        /// <summary>
        /// Removes the nodes whose name no dependency starts or ends at.
        /// </summary>
        private void RemoveNodesNotContainedInDependencies<T>(
            Dictionary<T, IPlantUmlElement> nodes,
            Func<T, string> nameOf
        )
        {
            var namesInDependencies = new HashSet<string>(
                _dependencies.SelectMany(dep => new[] { dep.Origin, dep.Target })
            );
            // Collected first: removing entries while enumerating the dictionary throws on
            // .NET Framework.
            foreach (
                var key in nodes
                    .Keys.Where(key => !namesInDependencies.Contains(nameOf(key)))
                    .ToList()
            )
            {
                nodes.Remove(key);
            }
        }

        /// <summary>
        /// The candidates, other than <paramref name="origin"/> itself, that one of the origin's
        /// dependencies passing <see cref="GenerationOptions.DependencyFilter"/> points into.
        /// </summary>
        private static IEnumerable<T> SelectDependencies<T>(
            T origin,
            IEnumerable<T> candidates,
            Func<T, string> nameOf,
            Func<T, IEnumerable<IType>> typesOf,
            GenerationOptions generationOptions
        )
            where T : IHasDependencies
        {
            var originName = nameOf(origin);
            return candidates.Where(target =>
                nameOf(target) != originName
                && origin
                    .Dependencies.Where(dep =>
                        generationOptions.DependencyFilter?.Invoke(dep) ?? true
                    )
                    .Any(dep => typesOf(target).Contains(dep.Target))
            );
        }

        private void RemovePatternInappropriateSlices(string thatContainsThisString = null)
        {
            if (_sliceList.Any(slice => slice.CountOfAsteriskInPattern == null))
            {
                return;
            }
            if (
                thatContainsThisString != null
                && !_sliceList.Any(slice =>
                    (NameSpaceOf(slice) + NameOf(slice)).Contains(thatContainsThisString)
                )
            )
            {
                return;
            }

            for (var i = _sliceList.Count - 1; i >= 0; i--)
            {
                var dots = 0;

                if (
                    thatContainsThisString != null
                    && !NameOf(_sliceList[i]).Contains(thatContainsThisString)
                )
                {
                    continue;
                }

                // The description is never qualified, so its dots are those below the namespace
                // the pattern matched before its capture group.
                dots += _sliceList[i].Description.Count(c => c == '.');

                if (dots >= _sliceList[i].CountOfAsteriskInPattern)
                {
                    _sliceList.RemoveAt(i);
                }
            }
        }

        private void RemoveDuplicatedArrowsIfExist()
        {
            for (var i = _dependencies.Count - 1; i >= 0; i--)
            {
                for (var j = i - 1; j >= 0; j--)
                {
                    if (
                        _dependencies[i].GetPlantUmlString() == _dependencies[j].GetPlantUmlString()
                    )
                    {
                        _dependencies.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        private void RemoveDuplicateDependenciesWhenShowingPackages()
        {
            if (_sliceList.Any(slice => NameSpaceOf(slice) == null))
            {
                return;
            }

            for (var j = _dependencies.Count - 1; j >= 0; j--)
            {
                if (
                    _sliceList.Any(slice =>
                        NameOf(slice).Contains(_dependencies[j].Target)
                        && NameOf(slice) != _dependencies[j].Target
                    )
                )
                {
                    if (_dependencies[j].Origin.Contains(_dependencies[j].Target))
                    {
                        _dependencies.RemoveAt(j);
                    }
                    else if (_dependencies[j].DependencyType == DependencyType.PackageToOne)
                    {
                        _dependencies[j] = new PlantUmlDependency(
                            _dependencies[j].Origin,
                            _dependencies[j].Target,
                            DependencyType.PackageToPackage
                        );
                    }
                    else
                    {
                        _dependencies[j] = new PlantUmlDependency(
                            _dependencies[j].Origin,
                            _dependencies[j].Target,
                            DependencyType.OneToPackage
                        );
                    }
                    continue;
                }

                if (_dependencies[j].DependencyType != DependencyType.PackageToOne)
                {
                    continue;
                }

                if (_dependencies[j].Target.Contains(_dependencies[j].Origin))
                {
                    _dependencies.RemoveAt(j);
                }
            }

            for (var i = _dependencies.Count - 1; i >= 0; i--)
            {
                if (
                    _dependencies[i].DependencyType == DependencyType.PackageToOne
                    || _dependencies[i].DependencyType == DependencyType.PackageToPackage
                )
                {
                    if (
                        _dependencies.Any(dependency =>
                            _dependencies[i].Target == dependency.Target
                            && dependency.Origin.Contains(_dependencies[i].Origin)
                            && dependency.Origin != _dependencies[i].Origin
                        )
                    )
                    {
                        _dependencies.RemoveAt(i);
                        continue;
                    }
                }

                if (
                    _dependencies[i].DependencyType == DependencyType.OneToPackage
                    || _dependencies[i].DependencyType == DependencyType.PackageToPackage
                )
                {
                    if (
                        _dependencies.Any(dependency =>
                            dependency.Target.Contains(_dependencies[i].Target)
                            && dependency.Target != _dependencies[i].Target
                            && dependency.Origin.Contains(_dependencies[i].Origin)
                        )
                    )
                    {
                        _dependencies.RemoveAt(i);
                        continue;
                    }
                }

                if (
                    _dependencies[i].Target.Contains(_dependencies[i].Origin)
                    && _dependencies[i].Target != _dependencies[i].Origin
                )
                {
                    _dependencies.RemoveAt(i);
                }
            }
        }

        private void ReplaceCirclesWithAppropriateDependencyType()
        {
            for (var i = _dependencies.Count - 1; i >= 0; i--)
            {
                for (var j = i - 1; j >= 0; j--)
                {
                    if (
                        _dependencies[i].Target != _dependencies[j].Origin
                        || _dependencies[i].Origin != _dependencies[j].Target
                    )
                    {
                        continue;
                    }

                    _dependencies.RemoveAt(i);
                    _dependencies[j] = new PlantUmlDependency(
                        _dependencies[j].Origin,
                        _dependencies[j].Target,
                        DependencyType.Circle
                    );
                    break;
                }
            }
        }

        /// <summary>
        /// Decides for every slice the name it is drawn under and the root package it is nested
        /// in, if any.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The slice exporter works out the layout from these names alone: which slice is the
        /// package of another, which arrows to merge, how deep <c>LimitDependencies</c> goes. A
        /// nested slice is therefore drawn under its qualified name, the namespace its types
        /// matched before the first capture group followed by its
        /// <see cref="SliceIdentifier.DiagramPath"/>, e.g. "App.Orders.Http", with that namespace
        /// as its root package. Every other slice is drawn under its description, without a
        /// package.
        /// </para>
        /// <para>
        /// Qualified names of one pattern are distinct, but a description can coincide with a
        /// qualified name. Drawing two slices as one node would be wrong, so that is rejected.
        /// </para>
        /// </remarks>
        /// <param name="nestSlicesInPackages">
        /// Whether the options ask for nesting; slices of <c>MatchingWithPackages</c> are nested
        /// regardless.
        /// </param>
        private void AssignDiagramNames(bool nestSlicesInPackages)
        {
            var nest = nestSlicesInPackages || _sliceList.Any(slice => slice.NestByDefault);
            _diagramNames = new Dictionary<Slice, (string Name, string NameSpace)>();
            var slicesByName = new Dictionary<string, Slice>();
            foreach (var slice in _sliceList)
            {
                var nameSpace = slice.DiagramNamespace;
                var diagramPath = slice.Identifier.DiagramPath;
                (string Name, string NameSpace) diagramName =
                    nest && nameSpace != null && diagramPath != null
                        ? (nameSpace + diagramPath, nameSpace == "" ? null : nameSpace)
                        : (slice.Description, null);

                if (slicesByName.TryGetValue(diagramName.Name, out var other))
                {
                    throw new ArgumentException(
                        "The slices \""
                            + other.Identifier.Identifier
                            + "\" and \""
                            + slice.Identifier.Identifier
                            + "\" would both be drawn as \""
                            + diagramName.Name
                            + "\"."
                    );
                }

                slicesByName.Add(diagramName.Name, slice);
                _diagramNames.Add(slice, diagramName);
            }
        }

        /// <summary>The name the diagram draws a slice under; see <see cref="AssignDiagramNames"/>.</summary>
        private string NameOf(Slice slice)
        {
            return _diagramNames[slice].Name;
        }

        /// <summary>
        /// The root package a slice is nested in, with a trailing ".", or <c>null</c> if it is
        /// drawn on its own.
        /// </summary>
        private string NameSpaceOf(Slice slice)
        {
            return _diagramNames[slice].NameSpace;
        }

        private bool IsPackage(Slice slice)
        {
            if (NameSpaceOf(slice) == null)
            {
                return false;
            }

            for (var i = _sliceList.Count - 1; i >= 0; i--)
            {
                if (
                    NameOf(_sliceList[i]) != NameOf(slice)
                    & NameOf(_sliceList[i]).StartsWith(NameOf(slice))
                )
                {
                    return true;
                }
            }

            return false;
        }
    }
}
