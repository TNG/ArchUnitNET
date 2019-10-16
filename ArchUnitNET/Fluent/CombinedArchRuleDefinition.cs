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
        private readonly LogicalConjunction _logicalConjunction;
        private readonly ICanBeEvaluated _oldRule;

        public CombinedArchRuleDefinition(ICanBeEvaluated oldRule, LogicalConjunction logicalConjunction)
        {
            _oldRule = oldRule;
            _logicalConjunction = logicalConjunction;
        }

        public GivenTypes Types()
        {
            var combinedRuleCreator = new CombinedArchRuleCreator<IType>(_oldRule, _logicalConjunction,
                BasicObjectProviderDefinition.Types);
            return new GivenTypes(combinedRuleCreator);
        }

        public GivenAttributes Attributes()
        {
            var combinedRuleCreator = new CombinedArchRuleCreator<Attribute>(_oldRule,
                _logicalConjunction, BasicObjectProviderDefinition.Attributes);
            return new GivenAttributes(combinedRuleCreator);
        }

        public GivenClasses Classes()
        {
            var combinedRuleCreator = new CombinedArchRuleCreator<Class>(_oldRule, _logicalConjunction,
                BasicObjectProviderDefinition.Classes);
            return new GivenClasses(combinedRuleCreator);
        }

        public GivenInterfaces Interfaces()
        {
            var combinedRuleCreator = new CombinedArchRuleCreator<Interface>(_oldRule,
                _logicalConjunction, BasicObjectProviderDefinition.Interfaces);
            return new GivenInterfaces(combinedRuleCreator);
        }

        public GivenMembers Members()
        {
            var combinedRuleCreator = new CombinedArchRuleCreator<IMember>(_oldRule, _logicalConjunction,
                BasicObjectProviderDefinition.Members);
            return new GivenMembers(combinedRuleCreator);
        }

        public GivenFieldMembers FieldMembers()
        {
            var combinedRuleCreator = new CombinedArchRuleCreator<FieldMember>(_oldRule,
                _logicalConjunction, BasicObjectProviderDefinition.FieldMembers);
            return new GivenFieldMembers(combinedRuleCreator);
        }

        public GivenMethodMembers MethodMembers()
        {
            var combinedRuleCreator = new CombinedArchRuleCreator<MethodMember>(_oldRule,
                _logicalConjunction, BasicObjectProviderDefinition.MethodMembers);
            return new GivenMethodMembers(combinedRuleCreator);
        }

        public GivenPropertyMembers PropertyMembers()
        {
            var combinedRuleCreator = new CombinedArchRuleCreator<PropertyMember>(_oldRule,
                _logicalConjunction, BasicObjectProviderDefinition.PropertyMembers);
            return new GivenPropertyMembers(combinedRuleCreator);
        }
    }
}