using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;
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
            _ruleCreator.AddObjectFilter(type => type.ImplementsInterface(pattern),
                "implement interface \"" + pattern + "\"");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction ImplementInterface(Interface intf)
        {
            _ruleCreator.AddObjectFilter(type => type.ImplementsInterface(intf),
                "implement interface \"" + intf.FullName + "\"");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction ResideInNamespace(string pattern)
        {
            _ruleCreator.AddObjectFilter(type => type.ResidesInNamespace(pattern),
                "reside in namespace \"" + pattern + "\"");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HavePropertyMemberWithName(string name)
        {
            _ruleCreator.AddObjectFilter(type => type.HasPropertyMemberWithName(name),
                "have property member with name\"" + name + "\"");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveFieldMemberWithName(string name)
        {
            _ruleCreator.AddObjectFilter(type => type.HasFieldMemberWithName(name),
                "have field member with name \"" + name + "\"");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveMethodMemberWithName(string name)
        {
            _ruleCreator.AddObjectFilter(type => type.HasMethodMemberWithName(name),
                "have method member with name \"" + name + "\"");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveMemberWithName(string name)
        {
            _ruleCreator.AddObjectFilter(type => type.HasMemberWithName(name),
                "have member with name \"" + name + "\"");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNested()
        {
            _ruleCreator.AddObjectFilter(type => type.IsNested, "are nested");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TGivenRuleTypeConjunction DoNotImplementInterface(string pattern)
        {
            _ruleCreator.AddObjectFilter(type => !type.ImplementsInterface(pattern),
                "do not implement interface \"" + pattern + "\"");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotImplementInterface(Interface intf)
        {
            _ruleCreator.AddObjectFilter(type => !type.ImplementsInterface(intf),
                "do not implement interface \"" + intf.FullName + "\"");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotResideInNamespace(string pattern)
        {
            _ruleCreator.AddObjectFilter(type => !type.ResidesInNamespace(pattern),
                "do not reside in namespace \"" + pattern + "\"");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHavePropertyMemberWithName(string name)
        {
            _ruleCreator.AddObjectFilter(type => !type.HasPropertyMemberWithName(name),
                "do not have property member with name \"" + name + "\"");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveFieldMemberWithName(string name)
        {
            _ruleCreator.AddObjectFilter(type => !type.HasFieldMemberWithName(name),
                "do not have field member with name \"" + name + "\"");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveMethodMemberWithName(string name)
        {
            _ruleCreator.AddObjectFilter(type => !type.HasMethodMemberWithName(name),
                "do not have method member with name \"" + name + "\"");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveMemberWithName(string name)
        {
            _ruleCreator.AddObjectFilter(type => !type.HasMemberWithName(name),
                "do not have member with name \"" + name + "\"");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotNested()
        {
            _ruleCreator.AddObjectFilter(type => !type.IsNested, "are not nested");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }
    }
}