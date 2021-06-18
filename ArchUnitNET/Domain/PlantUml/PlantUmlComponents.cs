using System;
using System.Collections.Generic;
using System.Linq;

namespace ArchUnitNET.Domain.PlantUml
{
    internal class PlantUmlComponents
    {
        private readonly Dictionary<ComponentName, PlantUmlComponent> _componentsByName;
        private readonly Dictionary<Alias, PlantUmlComponent> _componentsByAlias;

        public PlantUmlComponents(ISet<PlantUmlComponent> components)
        {
            _componentsByName = components
                .GroupBy(c => c.ComponentName)
                .Select(group => group.First())
                .ToDictionary(c => c.ComponentName);
            _componentsByAlias = components.Where(c => c.Alias != null).ToDictionary(c => c.Alias);
        }

        public IEnumerable<PlantUmlComponent> AllComponents
        {
            get
            {
                return _componentsByName.Values;
            }
        }

        public IEnumerable<PlantUmlComponent> CompomenentsWithAlias
        {
            get
            {
                return _componentsByAlias.Values;
            }
        }

        internal PlantUmlComponent FindComponentWith(string nameOrAlias)
        {
            ComponentName componentName = new ComponentName(nameOrAlias);
            Alias alias = new Alias(nameOrAlias);
            PlantUmlComponent result = null;
            if (_componentsByAlias.ContainsKey(alias))
            {
                result = _componentsByAlias[alias];
            }
            else if (_componentsByName.ContainsKey(componentName))
            {
                result = _componentsByName[componentName];
            }
            if (result == null)
            {
                throw new IllegalDiagramException(string.Format("There is no Component with name or alias = '{0}'. {1}", nameOrAlias,
                        "Components must be specified separately from dependencies."));
            }
            return result;
        }

        internal PlantUmlComponent findComponentWith(ComponentIdentifier identifier)
        {
            return _componentsByName[identifier.ComponentName];
        }
    }
}