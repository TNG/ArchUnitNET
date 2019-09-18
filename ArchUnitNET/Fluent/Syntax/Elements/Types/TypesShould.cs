using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;
using static ArchUnitNET.Fluent.Syntax.ActivatorHandler;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public class TypesShould<TRuleTypeShouldConjunction, TRuleType> :
        ObjectsShould<TRuleTypeShouldConjunction, TRuleType>,
        ITypesShould<TRuleTypeShouldConjunction, TRuleType>
        where TRuleType : IType
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
    {
        // ReSharper disable once MemberCanBeProtected.Global
        public TypesShould(ArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TRuleTypeShouldConjunction ImplementInterface(string pattern)
        {
            _ruleCreator.AddSimpleCondition(type => type.ImplementsInterface(pattern),
                "implement interface \"" + pattern + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ImplementInterface(Interface intf)
        {
            _ruleCreator.AddSimpleCondition(type => type.ImplementsInterface(intf),
                "implement interface \"" + intf.FullName + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ResideInNamespace(string pattern)
        {
            _ruleCreator.AddSimpleCondition(type => type.ResidesInNamespace(pattern),
                "reside in namespace \"" + pattern + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HavePropertyMemberWithName(string name)
        {
            _ruleCreator.AddSimpleCondition(type => type.HasPropertyMemberWithName(name),
                "have property member with name \"" + name + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFieldMemberWithName(string name)
        {
            _ruleCreator.AddSimpleCondition(type => type.HasFieldMemberWithName(name),
                "have field member with name \"" + name + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveMethodMemberWithName(string name)
        {
            _ruleCreator.AddSimpleCondition(type => type.HasMethodMemberWithName(name),
                "have method member with name \"" + name + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveMemberWithName(string name)
        {
            _ruleCreator.AddSimpleCondition(type => type.HasMemberWithName(name),
                "have member with name \"" + name + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeNested()
        {
            _ruleCreator.AddSimpleCondition(type => type.IsNested, "be nested");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction NotImplementInterface(string pattern)
        {
            _ruleCreator.AddSimpleCondition(type => !type.ImplementsInterface(pattern),
                "not implement interface \"" + pattern + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotImplementInterface(Interface intf)
        {
            _ruleCreator.AddSimpleCondition(type => !type.ImplementsInterface(intf),
                "not implement interface \"" + intf.FullName + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotResideInNamespace(string pattern)
        {
            _ruleCreator.AddSimpleCondition(type => !type.ResidesInNamespace(pattern),
                "not reside in namespace \"" + pattern + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHavePropertyMemberWithName(string name)
        {
            _ruleCreator.AddSimpleCondition(type => !type.HasPropertyMemberWithName(name),
                "not have property member with name \"" + name + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveFieldMemberWithName(string name)
        {
            _ruleCreator.AddSimpleCondition(type => !type.HasFieldMemberWithName(name),
                "not have field member with name \"" + name + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveMethodMemberWithName(string name)
        {
            _ruleCreator.AddSimpleCondition(type => !type.HasMethodMemberWithName(name),
                "not have method member with name \"" + name + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveMemberWithName(string name)
        {
            _ruleCreator.AddSimpleCondition(type => !type.HasMemberWithName(name),
                "not have member with name \"" + name + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeNested()
        {
            _ruleCreator.AddSimpleCondition(type => !type.IsNested, "not be nested");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}