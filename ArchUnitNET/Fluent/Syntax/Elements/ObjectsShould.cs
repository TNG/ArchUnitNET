using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Classes;
using static ArchUnitNET.Domain.Visibility;
using static ArchUnitNET.Fluent.Syntax.ActivatorHandler;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public abstract class ObjectsShould<TRuleTypeShouldConjunction, TRuleType> : SyntaxElement<TRuleType>,
        IObjectsShould<TRuleTypeShouldConjunction, TRuleType>
        where TRuleType : ICanBeAnalyzed
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
    {
        //TODO Create ConditionDefinition Class

        protected ObjectsShould(ArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TRuleTypeShouldConjunction Exist()
        {
            _ruleCreator.AddIsNullOrEmptyCondition(false, "exist", "does not exist");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction Be(ICanBeAnalyzed obj)
        {
            _ruleCreator.AddSimpleCondition(o => o.Equals(obj), "be \"" + obj.FullName + "\"",
                "is not \"" + obj.FullName + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DependOn(string pattern)
        {
            _ruleCreator.AddSimpleCondition(obj => obj.DependsOn(pattern), "depend on \"" + pattern + "\"",
                "does not depend on \"" + pattern + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveName(string name)
        {
            _ruleCreator.AddSimpleCondition(obj => obj.Name.Equals(name), "have name \"" + name + "\"",
                "does not have name \"" + name + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFullName(string fullname)
        {
            _ruleCreator.AddSimpleCondition(obj => obj.FullName.Equals(fullname),
                "have full name \"" + fullname + "\"", "does not have full name \"" + fullname + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNameStartingWith(string pattern)
        {
            _ruleCreator.AddSimpleCondition(obj => obj.NameStartsWith(pattern),
                "have name starting with \"" + pattern + "\"", "does not have name starting with \"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNameEndingWith(string pattern)
        {
            _ruleCreator.AddSimpleCondition(obj => obj.NameEndsWith(pattern),
                "have name ending with \"" + pattern + "\"", "does not have name ending with \"" + pattern + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNameContaining(string pattern)
        {
            _ruleCreator.AddSimpleCondition(obj => obj.NameContains(pattern),
                "have name containing \"" + pattern + "\"", "does not have name containing \"" + pattern + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BePrivate()
        {
            _ruleCreator.AddSimpleCondition(obj => obj.Visibility == Private, "be private", "is not private");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BePublic()
        {
            _ruleCreator.AddSimpleCondition(obj => obj.Visibility == Public, "be public", "is not public");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeProtected()
        {
            _ruleCreator.AddSimpleCondition(obj => obj.Visibility == Protected, "be protected", "is not protected");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeInternal()
        {
            _ruleCreator.AddSimpleCondition(obj => obj.Visibility == Internal, "be internal", "is not internal");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeProtectedInternal()
        {
            _ruleCreator.AddSimpleCondition(obj => obj.Visibility == ProtectedInternal, "be protected internal",
                "is not protected internal");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BePrivateProtected()
        {
            _ruleCreator.AddSimpleCondition(obj => obj.Visibility == PrivateProtected, "be private protected",
                "is not private protected");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public ClassesShouldThat<TRuleTypeShouldConjunction, TRuleType> DependOnClassesThat()
        {
            _ruleCreator.BeginComplexCondition<Class>((obj, cls) => obj.DependsOn(cls.FullName),
                "depend on classes that", "does not depend on classes that");
            return new ClassesShouldThat<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public AttributesShouldThat<TRuleTypeShouldConjunction, TRuleType> HaveAttributesThat()
        {
            _ruleCreator.BeginComplexCondition<Attribute>((obj, attribute) => obj.Attributes.Contains(attribute),
                "have attributes that", "does not have attributes that");
            return new AttributesShouldThat<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction NotExist()
        {
            _ruleCreator.AddIsNullOrEmptyCondition(true, "not exist", "does exist");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBe(ICanBeAnalyzed obj)
        {
            _ruleCreator.AddSimpleCondition(o => !o.Equals(obj), "not be\"" + obj.FullName + "\"",
                "is \"" + obj.FullName + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotDependOn(string pattern)
        {
            _ruleCreator.AddSimpleCondition(obj => !obj.DependsOn(pattern), "not depend on \"" + pattern + "\"",
                "does depend on \"" + pattern + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveName(string name)
        {
            _ruleCreator.AddSimpleCondition(obj => !obj.Name.Equals(name), "not have name \"" + name + "\"",
                "does have name \"" + name + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveFullName(string fullname)
        {
            _ruleCreator.AddSimpleCondition(obj => !obj.FullName.Equals(fullname),
                "not have full name \"" + fullname + "\"", "does have full name \"" + fullname + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveNameStartingWith(string pattern)
        {
            _ruleCreator.AddSimpleCondition(obj => !obj.NameStartsWith(pattern),
                "not have name starting with \"" + pattern + "\"", "does have name starting with \"" + pattern + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveNameEndingWith(string pattern)
        {
            _ruleCreator.AddSimpleCondition(obj => !obj.NameEndsWith(pattern),
                "not have name ending with \"" + pattern + "\"", "does have name ending with \"" + pattern + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveNameContaining(string pattern)
        {
            _ruleCreator.AddSimpleCondition(obj => !obj.NameContains(pattern),
                "not have name containing \"" + pattern + "\"", "does have name containing \"" + pattern + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBePrivate()
        {
            _ruleCreator.AddSimpleCondition(obj => obj.Visibility != Private, "not be private", "is private");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBePublic()
        {
            _ruleCreator.AddSimpleCondition(obj => obj.Visibility != Public, "not be public", "is public");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeProtected()
        {
            _ruleCreator.AddSimpleCondition(obj => obj.Visibility != Protected, "not be protected", "is protected");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeInternal()
        {
            _ruleCreator.AddSimpleCondition(obj => obj.Visibility != Internal, "not be internal", "is internal");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeProtectedInternal()
        {
            _ruleCreator.AddSimpleCondition(obj => obj.Visibility != ProtectedInternal, "not be protected internal",
                "is protected internal");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBePrivateProtected()
        {
            _ruleCreator.AddSimpleCondition(obj => obj.Visibility != PrivateProtected, "not be private protected",
                "is private protected");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public ClassesShouldThat<TRuleTypeShouldConjunction, TRuleType> NotDependOnClassesThat()
        {
            _ruleCreator.BeginComplexCondition<Class>((obj, cls) => !obj.DependsOn(cls.FullName),
                "not depend on classes that", "does depend on classes that");
            return new ClassesShouldThat<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public AttributesShouldThat<TRuleTypeShouldConjunction, TRuleType> NotHaveAttributesThat()
        {
            _ruleCreator.BeginComplexCondition<Attribute>((obj, attribute) => !obj.Attributes.Contains(attribute),
                "not have attributes that", "does have attributes that");
            return new AttributesShouldThat<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}