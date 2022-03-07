using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Domain.PlantUml.Exceptions;

namespace ArchUnitNET.Domain.PlantUml.Import
{
    internal class ClassDiagramAssociation
    {
        private readonly ImmutableHashSet<AssociatedComponent> _components;

        public ClassDiagramAssociation(PlantUmlParsedDiagram diagram)
        {
            var components = ImmutableHashSet.CreateBuilder<AssociatedComponent>();
            ValidateStereotypes(diagram);
            foreach (var component in diagram.AllComponents)
            {
                components.Add(new AssociatedComponent(component));
            }
            _components = components.ToImmutable();
        }

        private void ValidateStereotypes(PlantUmlParsedDiagram plantUmlDiagram)
        {
            ISet<Stereotype> visited = new HashSet<Stereotype>();
            foreach (PlantUmlComponent component in plantUmlDiagram.AllComponents)
            {
                foreach (Stereotype stereotype in component.Stereotypes)
                {
                    if (visited.Contains(stereotype))
                    {
                        throw new IllegalDiagramException(string.Format("Stereotype '{0}' should be unique", stereotype.AsString()));
                    }
                    visited.Add(stereotype);
                }
            }
        }

        public ISet<string> GetTargetNamespaceIdentifiers(IType clazz)
        {
            var result = ImmutableHashSet.CreateBuilder<string>();
            foreach (PlantUmlComponent target in GetComponentOf(clazz).Dependencies)
            {
                foreach (string identifier in GetNamespaceIdentifiersFromComponentOf(target))
                {
                    result.Add(identifier);
                }
            }
            return result.ToImmutable();
        }

        public bool Contains(IType clazz)
        {
            return GetAssociatedComponents(clazz).Count > 0;
        }

        public ISet<string> GetNamespaceIdentifiersFromComponentOf(IType javaIType)
        {
            return GetNamespaceIdentifiersFromComponentOf(GetComponentOf(javaIType));
        }

        private ISet<string> GetNamespaceIdentifiersFromComponentOf(PlantUmlComponent component)
        {
            var result = ImmutableHashSet.CreateBuilder<string>();
            foreach (Stereotype stereotype in component.Stereotypes)
            {
                result.Add(stereotype.AsString());
            }
            return result.ToImmutable();
        }

        private PlantUmlComponent GetComponentOf(IType clazz)
        {
            ISet<PlantUmlComponent> associatedComponents = GetAssociatedComponents(clazz);

            if (associatedComponents.Count > 1)
            {
                throw new ComponentIntersectionException(
                        string.Format("Class {0} may not be contained in more than one component, but is contained in [{1}]",
                                clazz.Name,
                                string.Join(", ", GetComponentNames(associatedComponents))));
            }
            else if (associatedComponents.Count == 0)
            {
                throw new InvalidOperationException(string.Format("Class {0} is not contained in any component", clazz.Name));
            }

            return associatedComponents.Single();
        }

        private ISet<string> GetComponentNames(ISet<PlantUmlComponent> associatedComponents)
        {
            ISet<string> associatedComponentNames = new HashSet<string>();
            foreach (PlantUmlComponent associatedComponent in associatedComponents.OrderBy(component => component.ComponentName.AsString()))
            {
                associatedComponentNames.Add(associatedComponent.ComponentName.AsString());
            }
            return associatedComponentNames;
        }

        private ISet<PlantUmlComponent> GetAssociatedComponents(IType clazz)
        {
            var result = ImmutableHashSet.CreateBuilder<PlantUmlComponent>();
            foreach (AssociatedComponent component in _components)
            {
                if (component.Contains(clazz))
                {
                    result.Add(component.AsPlantUmlComponent);
                }
            }
            return result.ToImmutable();
        }

        private class AssociatedComponent
        {
            private readonly PlantUmlComponent _component;

            public AssociatedComponent(PlantUmlComponent component)
            {
                _component = component;
            }

            public PlantUmlComponent AsPlantUmlComponent
            {
                get { return _component; }
            }

            public bool Contains(IType clazz)
            {
                foreach (Stereotype stereotype in _component.Stereotypes)
                {
                    if (clazz.ResidesInNamespace(stereotype.AsString(), true))
                    {
                        return true;
                    }
                }
                return false;
            }


        }
    }
}