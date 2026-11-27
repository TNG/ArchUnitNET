using System.Collections.Generic;

namespace ArchUnitNET.Domain.PlantUml.Import
{
    /// <summary>
    /// A PlantUML grouping element (package, node, folder, frame, cloud, database).
    /// A dependency to or from a group applies to all components nested inside it.
    /// Stereotypes on a group are only drawn and do not assign any types to it.
    /// </summary>
    internal class PlantUmlGroup
    {
        private readonly List<ComponentIdentifier> _members = new List<ComponentIdentifier>();

        public PlantUmlGroup(string name, string alias)
        {
            Name = name;
            Alias = alias;
        }

        public string Name { get; }
        public string Alias { get; }

        public IReadOnlyList<ComponentIdentifier> Members => _members;

        public bool IsReferencedBy(string nameOrAlias)
        {
            return Alias != null ? Alias == nameOrAlias : Name == nameOrAlias;
        }

        public void AddMember(ComponentIdentifier member)
        {
            if (!_members.Contains(member))
            {
                _members.Add(member);
            }
        }
    }
}
