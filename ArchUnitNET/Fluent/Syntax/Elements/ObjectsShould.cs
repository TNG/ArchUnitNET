using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Classes;
using static ArchUnitNET.Fluent.Syntax.ActivatorHandler;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public abstract class ObjectsShould<TRuleTypeShouldConjunction, TRuleType> : SyntaxElement<TRuleType>,
        IObjectsShould<TRuleTypeShouldConjunction, TRuleType>
        where TRuleType : ICanBeAnalyzed
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
    {
        protected ObjectsShould(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TRuleTypeShouldConjunction Exist()
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.Exist());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction Be(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.Be(firstObject, moreObjects));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DependOn(string pattern)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.DependOn(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveName(string name)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.HaveName(name));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFullName(string fullname)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.HaveFullName(fullname));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNameStartingWith(string pattern)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.HaveNameStartingWith(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNameEndingWith(string pattern)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.HaveNameEndingWith(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNameContaining(string pattern)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.HaveNameContaining(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BePrivate()
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.BePrivate());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BePublic()
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.BePublic());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeProtected()
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.BeProtected());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeInternal()
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.BeInternal());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeProtectedInternal()
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.BeProtectedInternal());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BePrivateProtected()
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.BePrivateProtected());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        //Complex Conditions

        public ClassesShouldThat<TRuleTypeShouldConjunction, TRuleType> DependOnClassesThat()
        {
            _ruleCreator.BeginComplexCondition(ObjectsConditionDefinition<TRuleType>.DependOnClassesThat());
            return new ClassesShouldThat<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public AttributesShouldThat<TRuleTypeShouldConjunction, TRuleType> HaveAttributesThat()
        {
            _ruleCreator.BeginComplexCondition(ObjectsConditionDefinition<TRuleType>.HaveAttributesThat());
            return new AttributesShouldThat<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction NotExist()
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotExist());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBe(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotBe(firstObject, moreObjects));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotDependOn(string pattern)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotDependOn(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveName(string name)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotHaveName(name));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveFullName(string fullname)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotHaveFullName(fullname));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveNameStartingWith(string pattern)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotHaveNameStartingWith(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveNameEndingWith(string pattern)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotHaveNameEndingWith(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveNameContaining(string pattern)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotHaveNameContaining(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBePrivate()
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotBePrivate());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBePublic()
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotBePublic());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeProtected()
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotBeProtected());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeInternal()
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotBeInternal());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeProtectedInternal()
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotBeProtectedInternal());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBePrivateProtected()
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotBePrivateProtected());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        //Complex Condition Negations

        public ClassesShouldThat<TRuleTypeShouldConjunction, TRuleType> NotDependOnClassesThat()
        {
            _ruleCreator.BeginComplexCondition(ObjectsConditionDefinition<TRuleType>.NotDependOnClassesThat());
            return new ClassesShouldThat<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public AttributesShouldThat<TRuleTypeShouldConjunction, TRuleType> NotHaveAttributesThat()
        {
            _ruleCreator.BeginComplexCondition(ObjectsConditionDefinition<TRuleType>.NotHaveAttributesThat());
            return new AttributesShouldThat<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}