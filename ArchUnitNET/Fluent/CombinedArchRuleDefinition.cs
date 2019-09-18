using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax;
using ArchUnitNET.Fluent.Syntax.Elements.Members;
using ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers;
using ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers;
using ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers;
using ArchUnitNET.Fluent.Syntax.Elements.Types;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Classes;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces;

namespace ArchUnitNET.Fluent
{
    public class CombinedArchRuleDefinition
    {
        private readonly IArchRuleCreator _currentArchRuleCreator;
        private readonly LogicalConjunction _logicalConjunction;

        public CombinedArchRuleDefinition(IArchRuleCreator currentArchRuleCreator,
            LogicalConjunction logicalConjunction)
        {
            _currentArchRuleCreator = currentArchRuleCreator;
            _logicalConjunction = logicalConjunction;
        }

        public GivenTypes Types()
        {
            var combinedRuleCreator = new CombinedArchRuleCreator<IType>(_currentArchRuleCreator, _logicalConjunction,
                architecture => architecture.Types, "Types");
            return new GivenTypes(combinedRuleCreator);
        }

        public GivenAttributes Attributes()
        {
            var combinedRuleCreator = new CombinedArchRuleCreator<Attribute>(_currentArchRuleCreator,
                _logicalConjunction, architecture => architecture.Attributes, "Attributes");
            return new GivenAttributes(combinedRuleCreator);
        }

        public GivenClasses Classes()
        {
            var combinedRuleCreator = new CombinedArchRuleCreator<Class>(_currentArchRuleCreator, _logicalConjunction,
                architecture => architecture.Classes, "Classes");
            return new GivenClasses(combinedRuleCreator);
        }

        public GivenInterfaces Interfaces()
        {
            var combinedRuleCreator = new CombinedArchRuleCreator<Interface>(_currentArchRuleCreator,
                _logicalConjunction, architecture => architecture.Interfaces, "Interfaces");
            return new GivenInterfaces(combinedRuleCreator);
        }

        public GivenMembers Members()
        {
            var combinedRuleCreator = new CombinedArchRuleCreator<IMember>(_currentArchRuleCreator, _logicalConjunction,
                architecture => architecture.Members, "Members");
            return new GivenMembers(combinedRuleCreator);
        }

        public GivenFieldMembers FieldMembers()
        {
            var combinedRuleCreator = new CombinedArchRuleCreator<FieldMember>(_currentArchRuleCreator,
                _logicalConjunction, architecture => architecture.FieldMembers, "Field members");
            return new GivenFieldMembers(combinedRuleCreator);
        }

        public GivenMethodMembers MethodMembers()
        {
            var combinedRuleCreator = new CombinedArchRuleCreator<MethodMember>(_currentArchRuleCreator,
                _logicalConjunction, architecture => architecture.MethodMembers, "Method members");
            return new GivenMethodMembers(combinedRuleCreator);
        }

        public GivenPropertyMembers PropertyMembers()
        {
            var combinedRuleCreator = new CombinedArchRuleCreator<PropertyMember>(_currentArchRuleCreator,
                _logicalConjunction, architecture => architecture.PropertyMembers, "Property members");
            return new GivenPropertyMembers(combinedRuleCreator);
        }
    }
}