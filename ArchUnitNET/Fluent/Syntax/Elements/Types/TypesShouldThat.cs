using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;
using static ArchUnitNET.Fluent.Syntax.ActivatorHandler;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public class TypesShouldThat<TRuleTypeShouldConjunction, TReferenceType, TRuleType> :
        ObjectsShouldThat<TRuleTypeShouldConjunction, TReferenceType, TRuleType>,
        ITypesThat<TRuleTypeShouldConjunction>
        where TReferenceType : IType
        where TRuleType : ICanBeAnalyzed
    {
        protected TypesShouldThat(ArchRuleCreator<TRuleType> ruleCreator,
            ObjectProvider<TReferenceType> referenceObjectProvider) :
            base(ruleCreator, referenceObjectProvider)
        {
        }

        public TypesShouldThat(ArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator,
            ObjectProviderDefinition.Types as ObjectProvider<TReferenceType>)
        {
        }

        public TRuleTypeShouldConjunction ImplementInterface(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                type => type.ImplementsInterface(pattern),
                "implement interface \"" + pattern + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ImplementInterface(Interface intf)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                type => type.ImplementsInterface(intf),
                "implement interface \"" + intf.FullName + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        public TRuleTypeShouldConjunction ResideInNamespace(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                type => type.ResidesInNamespace(pattern),
                "reside in namespace \"" + pattern + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HavePropertyMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                type => type.HasPropertyMemberWithName(name),
                "have property member with name \"" + name + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFieldMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                type => type.HasFieldMemberWithName(name),
                "have field member with name \"" + name + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveMethodMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                type => type.HasMethodMemberWithName(name),
                "have method member with name \"" + name + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                type => type.HasMemberWithName(name),
                "have member with name \"" + name + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNested()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider, type => type.IsNested, "are nested");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction DoNotImplementInterface(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                type => !type.ImplementsInterface(pattern),
                "do not implement interface \"" + pattern + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotImplementInterface(Interface intf)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                type => !type.ImplementsInterface(intf),
                "do not implement interface \"" + intf.FullName + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotResideInNamespace(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                type => !type.ResidesInNamespace(pattern),
                "do not reside in namespace \"" + pattern + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHavePropertyMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                type => !type.HasPropertyMemberWithName(name),
                "do not have property member with name \"" + name + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveFieldMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                type => !type.HasFieldMemberWithName(name),
                "do not have field member with name \"" + name + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveMethodMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                type => !type.HasMethodMemberWithName(name),
                "do not have method member with name \"" + name + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                type => !type.HasMemberWithName(name),
                "do not have member with name \"" + name + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotNested()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider, type => !type.IsNested, "are not nested");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}