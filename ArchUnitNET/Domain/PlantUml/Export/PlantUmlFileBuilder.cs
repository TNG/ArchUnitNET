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
        /// The deepest namespace that contains all types of each slice; see
        /// <see cref="CommonNamespaceOf"/>.
        /// </summary>
        private Dictionary<Slice, string> _commonNamespaces = new Dictionary<Slice, string>();

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
        /// With <see cref="GenerationOptions.NestSlicesInPackages"/>, every slice is drawn in the
        /// <c>package</c> of the deepest namespace that contains all of its types. With
        /// <see cref="GenerationOptions.LimitDependencies"/>, only the arrows between slices whose
        /// types share an equally deep namespace are drawn.</param>
        public PlantUmlFileBuilder WithDependenciesFrom(
            IEnumerable<Slice> slices,
            GenerationOptions generationOptions = null
        )
        {
            _sliceList = slices.Distinct().ToList();
            RemovePatternInappropriateSlices();
            AssignCommonNamespaces();

            if (generationOptions == null)
            {
                generationOptions = new GenerationOptions();
            }

            foreach (var slice in _sliceList)
            {
                var dependencyTargets = SelectDependencies(
                    slice,
                    _sliceList,
                    target => target.Description,
                    target => target.Types,
                    generationOptions
                );

                if (!generationOptions.LimitDependencies)
                {
                    _dependencies.AddRange(
                        dependencyTargets.Select(target => new PlantUmlDependency(
                            slice.Description,
                            target.Description,
                            DependencyType.OneToOne
                        ))
                    );
                }
                else
                {
                    _dependencies.AddRange(
                        dependencyTargets
                            .Where(target =>
                                CountOfSegments(_commonNamespaces[target])
                                == CountOfSegments(_commonNamespaces[slice])
                            )
                            .Select(target => new PlantUmlDependency(
                                slice.Description,
                                target.Description,
                                DependencyType.OneToOneCompact
                            ))
                    );
                }
            }

            if (!generationOptions.LimitDependencies)
            {
                ReplaceCirclesWithAppropriateDependencyType();
            }
            RemoveDuplicatedArrowsIfExist();

            var drawnSlices = generationOptions.IncludeNodesWithoutDependencies
                ? _sliceList
                : SlicesContainedInDependencies();
            var packages = AssignPackages(drawnSlices, generationOptions.NestSlicesInPackages);
            foreach (var slice in drawnSlices)
            {
                var node = CreateNode(slice, packages);
                if (generationOptions.C4Style)
                {
                    node.UseS4Style();
                }
                _diagram.AddElement(node);
            }
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
                        .Where(target =>
                            !generationOptions.LimitDependencies
                            || CountOfSegments(target.FullName) == CountOfSegments(ns.FullName)
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
        /// Draws only the slices whose types all lie in the namespace <paramref name="package"/>
        /// or below it, colored, and the slices they depend on or that depend on them, with the
        /// arrows between them that start or end at one of the former.
        /// </summary>
        /// <param name="slices">The slices to draw, e.g. from <c>GivenSlices.GetObjects</c>.</param>
        /// <param name="package">The namespace to focus on.</param>
        /// <param name="generationOptions">Only <see cref="GenerationOptions.NestSlicesInPackages"/>
        /// is read; the defaults if <c>null</c>.</param>
        /// <exception cref="ArgumentException">The package is empty, a single dot or a namespace
        /// that no slice lies in.</exception>
        public PlantUmlFileBuilder WithDependenciesFromFocusOn(
            IEnumerable<Slice> slices,
            string package,
            GenerationOptions generationOptions = null
        )
        {
            _sliceList = slices.Distinct().ToList();

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

            RemovePatternInappropriateSlices();
            AssignCommonNamespaces();

            var focusedSlices = new HashSet<string>(
                _sliceList
                    .Where(slice => LiesIn(_commonNamespaces[slice], package))
                    .Select(slice => slice.Description)
            );
            if (focusedSlices.Count == 0)
            {
                throw new ArgumentException("No slice lies in the namespace [" + package + "]");
            }

            var defaultOptions = new GenerationOptions();

            foreach (var slice in _sliceList)
            {
                var dependencyTargets = SelectDependencies(
                    slice,
                    _sliceList,
                    target => target.Description,
                    target => target.Types,
                    defaultOptions
                );

                _dependencies.AddRange(
                    dependencyTargets.Select(target => new PlantUmlDependency(
                        slice.Description,
                        target.Description,
                        DependencyType.OneToOne
                    ))
                );
            }

            ReplaceCirclesWithAppropriateDependencyType();
            RemoveDuplicatedArrowsIfExist();

            _dependencies.RemoveAll(dependency =>
                !focusedSlices.Contains(dependency.Origin)
                && !focusedSlices.Contains(dependency.Target)
            );

            var drawnSlices = SlicesContainedInDependencies();
            var packages = AssignPackages(
                drawnSlices,
                generationOptions?.NestSlicesInPackages ?? false
            );
            _diagram.AddElements(
                drawnSlices.Select(slice =>
                    CreateNode(
                        slice,
                        packages,
                        focusedSlices.Contains(slice.Description) ? "99ffd1" : null
                    )
                )
            );
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

        private void RemovePatternInappropriateSlices()
        {
            if (_sliceList.Any(slice => slice.CountOfAsteriskInPattern == null))
            {
                return;
            }

            for (var i = _sliceList.Count - 1; i >= 0; i--)
            {
                var dots = 0;

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
        /// The slices that a dependency starts or ends at, in the order of the slice list.
        /// </summary>
        private List<Slice> SlicesContainedInDependencies()
        {
            var namesInDependencies = new HashSet<string>(
                _dependencies.SelectMany(dep => new[] { dep.Origin, dep.Target })
            );
            return _sliceList
                .Where(slice => namesInDependencies.Contains(slice.Description))
                .ToList();
        }

        private void AssignCommonNamespaces()
        {
            _commonNamespaces = _sliceList.ToDictionary(slice => slice, CommonNamespaceOf);
        }

        /// <summary>
        /// The deepest namespace that contains all types of the slice, i.e. the longest sequence
        /// of leading segments their namespaces share, or an empty string if they share none.
        /// </summary>
        private static string CommonNamespaceOf(Slice slice)
        {
            string[] commonSegments = null;
            var length = 0;
            foreach (var type in slice.Types)
            {
                var segments = (type.Namespace?.FullName ?? "").Split('.');
                if (commonSegments == null)
                {
                    commonSegments = segments;
                    length = segments.Length;
                    continue;
                }

                length = Math.Min(length, segments.Length);
                for (var i = 0; i < length; i++)
                {
                    if (segments[i] != commonSegments[i])
                    {
                        length = i;
                        break;
                    }
                }
            }

            return commonSegments == null ? "" : string.Join(".", commonSegments.Take(length));
        }

        private static int CountOfSegments(string nameSpace)
        {
            return nameSpace == "" ? 0 : nameSpace.Count(c => c == '.') + 1;
        }

        /// <summary>
        /// Whether <paramref name="nameSpace"/> is <paramref name="package"/> or lies below it.
        /// </summary>
        private static bool LiesIn(string nameSpace, string package)
        {
            return nameSpace == package
                || nameSpace.StartsWith(package + ".", StringComparison.Ordinal);
        }

        /// <summary>
        /// Decides for every slice the packages it is drawn in, outermost first, if slices are
        /// nested at all.
        /// </summary>
        /// <remarks>
        /// The packages are the namespace tree of the slices' common namespaces, and each slice
        /// sits in the package of its own common namespace. A package that holds no slice and
        /// exactly one package is merged with that package into one labelled with both, e.g.
        /// "Orders.Domain", down to the first package that holds a slice or several packages. So
        /// the outermost package is a whole namespace such as "App.Orders". A slice whose types
        /// share no namespace is in no package and is drawn on its own.
        /// </remarks>
        /// <param name="slices">The slices that are drawn.</param>
        /// <param name="nestSlicesInPackages">
        /// Whether the options ask for nesting; slices of <c>MatchingWithPackages</c> are nested
        /// regardless.
        /// </param>
        private Dictionary<Slice, List<string>> AssignPackages(
            List<Slice> slices,
            bool nestSlicesInPackages
        )
        {
            var packages = new Dictionary<Slice, List<string>>();
            if (!nestSlicesInPackages && !slices.Any(slice => slice.NestByDefault))
            {
                return packages;
            }

            var nestedSlices = slices.Where(slice => _commonNamespaces[slice] != "").ToList();
            var root = new NamespaceNode();
            foreach (var slice in nestedSlices)
            {
                var node = root;
                foreach (var segment in _commonNamespaces[slice].Split('.'))
                {
                    node = node.GetOrAddChild(segment);
                }
                node.HoldsSlice = true;
            }

            foreach (var slice in nestedSlices)
            {
                var path = new List<string>();
                var node = root;
                foreach (var segment in _commonNamespaces[slice].Split('.'))
                {
                    var mergesWithChild =
                        node != root && !node.HoldsSlice && node.Children.Count == 1;
                    node = node.Children[segment];
                    if (mergesWithChild)
                    {
                        path[path.Count - 1] += "." + segment;
                    }
                    else
                    {
                        path.Add(segment);
                    }
                }
                packages.Add(slice, path);
            }

            return packages;
        }

        private static PlantUmlSlice CreateNode(
            Slice slice,
            Dictionary<Slice, List<string>> packages,
            string color = null
        )
        {
            if (!packages.TryGetValue(slice, out var path))
            {
                return new PlantUmlSlice(slice.Description, null, color);
            }

            return new PlantUmlSlice(
                slice.Description,
                path[0] + ".",
                path.Skip(1).ToList(),
                slice.Description,
                color
            );
        }

        /// <summary>A namespace in the tree that <see cref="AssignPackages"/> builds.</summary>
        private class NamespaceNode
        {
            public readonly Dictionary<string, NamespaceNode> Children =
                new Dictionary<string, NamespaceNode>();

            public bool HoldsSlice;

            public NamespaceNode GetOrAddChild(string segment)
            {
                if (!Children.TryGetValue(segment, out var child))
                {
                    child = new NamespaceNode();
                    Children.Add(segment, child);
                }

                return child;
            }
        }
    }
}
