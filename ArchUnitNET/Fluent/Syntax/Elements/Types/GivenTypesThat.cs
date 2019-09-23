using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ActivatorHandler;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types

{
    public class GivenTypesThat<TGivenRuleTypeConjunction, TRuleType> :
        GivenObjectsThat<TGivenRuleTypeConjunction, TRuleType>, ITypesThat<TGivenRuleTypeConjunction>
        where TRuleType : IType
    {
        // ReSharper disable once MemberCanBeProtected.Global
        public GivenTypesThat(ArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TGivenRuleTypeConjunction ImplementInterface(string pattern)
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.ImplementInterface(pattern));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction ImplementInterface(Interface intf)
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.ImplementInterface(intf));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction ResideInNamespace(string pattern)
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.ResideInNamespace(pattern));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HavePropertyMemberWithName(string name)
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.HavePropertyMemberWithName(name));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveFieldMemberWithName(string name)
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.HaveFieldMemberWithName(name));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveMethodMemberWithName(string name)
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.HaveMethodMemberWithName(name));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveMemberWithName(string name)
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.HaveMemberWithName(name));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNested()
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.AreNested());
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TGivenRuleTypeConjunction DoNotImplementInterface(string pattern)
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.DoNotImplementInterface(pattern));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotImplementInterface(Interface intf)
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.DoNotImplementInterface(intf));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotResideInNamespace(string pattern)
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.DoNotResideInNamespace(pattern));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHavePropertyMemberWithName(string name)
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.DoNotHavePropertyMemberWithName(name));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveFieldMemberWithName(string name)
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.DoNotHaveFieldMemberWithName(name));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveMethodMemberWithName(string name)
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.DoNotHaveMethodMemberWithName(name));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveMemberWithName(string name)
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.DoNotHaveMemberWithName(name));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotNested()
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.AreNotNested());
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }
    }
}