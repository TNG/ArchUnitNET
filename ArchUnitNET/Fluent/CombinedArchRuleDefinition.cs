using ArchUnitNET.Domain;
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
    public class CombinedArchRuleDefinition : PartialArchRuleConjunction
    {
        internal override ICanBeEvaluated LeftArchRule { get; }
        internal override LogicalConjunction LogicalConjunction { get; }

        public CombinedArchRuleDefinition(
            ICanBeEvaluated leftRule,
            LogicalConjunction logicalConjunction
        )
        {
            LeftArchRule = leftRule;
            LogicalConjunction = logicalConjunction;
        }

        public GivenTypes Types()
        {
            return new GivenTypes(this, BasicObjectProviderDefinition.Types);
        }

        public GivenAttributes Attributes()
        {
            return new GivenAttributes(this, BasicObjectProviderDefinition.Attributes);
        }

        public GivenClasses Classes()
        {
            return new GivenClasses(this, BasicObjectProviderDefinition.Classes);
        }

        public GivenInterfaces Interfaces()
        {
            return new GivenInterfaces(this, BasicObjectProviderDefinition.Interfaces);
        }

        public GivenMembers Members()
        {
            return new GivenMembers(this, BasicObjectProviderDefinition.Members);
        }

        public GivenFieldMembers FieldMembers()
        {
            return new GivenFieldMembers(this, BasicObjectProviderDefinition.FieldMembers);
        }

        public GivenMethodMembers MethodMembers()
        {
            return new GivenMethodMembers(this, BasicObjectProviderDefinition.MethodMembers);
        }

        public GivenPropertyMembers PropertyMembers()
        {
            return new GivenPropertyMembers(this, BasicObjectProviderDefinition.PropertyMembers);
        }
    }
}
