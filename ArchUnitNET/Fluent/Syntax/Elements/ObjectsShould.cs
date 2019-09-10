﻿using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Classes;
using static ArchUnitNET.Domain.Visibility;
using static ArchUnitNET.Fluent.Syntax.ActivatorHandler;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public class ObjectsShould<TRuleTypeShouldConjunction, TRuleType> : SyntaxElement<TRuleType>,
        IObjectsShould<TRuleTypeShouldConjunction, TRuleType>
        where TRuleType : ICanBeAnalyzed
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
    {
        protected ObjectsShould(ArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TRuleTypeShouldConjunction Be(ICanBeAnalyzed obj)
        {
            _ruleCreator.AddSimpleCondition(o => o.Equals(obj));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DependOn(string pattern)
        {
            _ruleCreator.AddSimpleCondition(obj => obj.DependsOn(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveName(string name)
        {
            _ruleCreator.AddSimpleCondition(obj => obj.Name.Equals(name));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFullName(string fullname)
        {
            _ruleCreator.AddSimpleCondition(obj => obj.FullName.Equals(fullname));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNameStartingWith(string pattern)
        {
            _ruleCreator.AddSimpleCondition(obj => obj.NameStartsWith(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNameEndingWith(string pattern)
        {
            _ruleCreator.AddSimpleCondition(obj => obj.NameEndsWith(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNameContaining(string pattern)
        {
            _ruleCreator.AddSimpleCondition(obj => obj.NameContains(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BePrivate()
        {
            _ruleCreator.AddSimpleCondition(obj => obj.Visibility == Private);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BePublic()
        {
            _ruleCreator.AddSimpleCondition(obj => obj.Visibility == Public);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeProtected()
        {
            _ruleCreator.AddSimpleCondition(obj => obj.Visibility == Protected);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeInternal()
        {
            _ruleCreator.AddSimpleCondition(obj => obj.Visibility == Internal);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeProtectedInternal()
        {
            _ruleCreator.AddSimpleCondition(obj => obj.Visibility == ProtectedInternal);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BePrivateProtected()
        {
            _ruleCreator.AddSimpleCondition(obj => obj.Visibility == PrivateProtected);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public ClassesShouldThat<TRuleTypeShouldConjunction, TRuleType> DependOnClassesThat()
        {
            return new ClassesShouldThat<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator,
                (obj, cls) => obj.DependsOn(cls.FullName));
        }

        public AttributesShouldThat<TRuleTypeShouldConjunction, TRuleType> HaveAttributesThat()
        {
            return new AttributesShouldThat<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator,
                (obj, attribute) => obj.Attributes.Contains(attribute));
        }


        //Negations


        public TRuleTypeShouldConjunction NotBe(ICanBeAnalyzed obj)
        {
            _ruleCreator.AddSimpleCondition(o => !o.Equals(obj));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotDependOn(string pattern)
        {
            _ruleCreator.AddSimpleCondition(obj => !obj.DependsOn(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveName(string name)
        {
            _ruleCreator.AddSimpleCondition(obj => !obj.Name.Equals(name));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveFullName(string fullname)
        {
            _ruleCreator.AddSimpleCondition(obj => !obj.FullName.Equals(fullname));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveNameStartingWith(string pattern)
        {
            _ruleCreator.AddSimpleCondition(obj => !obj.NameStartsWith(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveNameEndingWith(string pattern)
        {
            _ruleCreator.AddSimpleCondition(obj => !obj.NameEndsWith(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveNameContaining(string pattern)
        {
            _ruleCreator.AddSimpleCondition(obj => !obj.NameContains(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBePrivate()
        {
            _ruleCreator.AddSimpleCondition(obj => obj.Visibility != Private);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBePublic()
        {
            _ruleCreator.AddSimpleCondition(obj => obj.Visibility != Public);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeProtected()
        {
            _ruleCreator.AddSimpleCondition(obj => obj.Visibility != Protected);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeInternal()
        {
            _ruleCreator.AddSimpleCondition(obj => obj.Visibility != Internal);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeProtectedInternal()
        {
            _ruleCreator.AddSimpleCondition(obj => obj.Visibility != ProtectedInternal);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBePrivateProtected()
        {
            _ruleCreator.AddSimpleCondition(obj => obj.Visibility != PrivateProtected);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public ClassesShouldThat<TRuleTypeShouldConjunction, TRuleType> NotDependOnClassesThat()
        {
            return new ClassesShouldThat<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator,
                (obj, cls) => !obj.DependsOn(cls.FullName));
        }

        public AttributesShouldThat<TRuleTypeShouldConjunction, TRuleType> NotHaveAttributesThat()
        {
            return new AttributesShouldThat<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator,
                (obj, attribute) => !obj.Attributes.Contains(attribute));
        }
    }
}