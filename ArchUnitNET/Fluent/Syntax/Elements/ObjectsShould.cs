using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax.Elements.Types;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Classes;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

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
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction Be(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.Be(firstObject, moreObjects));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction Be(IEnumerable<ICanBeAnalyzed> objects)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.Be(objects));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DependOn(string pattern)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.DependOn(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DependOn(IType type)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.DependOn(type));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DependOn(Type type)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.DependOn(type));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyDependOn(string pattern)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.OnlyDependOn(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyDependOn(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.OnlyDependOn(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyDependOn(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.OnlyDependOn(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyDependOn(IObjectProvider<IType> types)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.OnlyDependOn(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyDependOn(IEnumerable<IType> types)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.OnlyDependOn(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyDependOn(IEnumerable<Type> types)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.OnlyDependOn(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveName(string name)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.HaveName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFullName(string fullname)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.HaveFullName(fullname));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNameStartingWith(string pattern)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.HaveNameStartingWith(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNameEndingWith(string pattern)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.HaveNameEndingWith(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNameContaining(string pattern)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.HaveNameContaining(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BePrivate()
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.BePrivate());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BePublic()
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.BePublic());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeProtected()
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.BeProtected());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeInternal()
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.BeInternal());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeProtectedInternal()
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.BeProtectedInternal());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BePrivateProtected()
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.BePrivateProtected());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        //Relation Conditions

        public ShouldRelateToClassesThat<TRuleTypeShouldConjunction, TRuleType> DependOnClassesThat()
        {
            _ruleCreator.BeginComplexCondition(ObjectsConditionDefinition<TRuleType>.DependOnClassesThat());
            return new ShouldRelateToClassesThat<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public ShouldRelateToClassesThat<TRuleTypeShouldConjunction, TRuleType> OnlyDependOnClassesThat()
        {
            _ruleCreator.BeginComplexCondition(ObjectsConditionDefinition<TRuleType>.OnlyDependOnClassesThat());
            return new ShouldRelateToClassesThat<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public ShouldRelateToInterfacesThat<TRuleTypeShouldConjunction, TRuleType> DependOnInterfacesThat()
        {
            _ruleCreator.BeginComplexCondition(ObjectsConditionDefinition<TRuleType>.DependOnInterfacesThat());
            return new ShouldRelateToInterfacesThat<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public ShouldRelateToInterfacesThat<TRuleTypeShouldConjunction, TRuleType> OnlyDependOnInterfacesThat()
        {
            _ruleCreator.BeginComplexCondition(ObjectsConditionDefinition<TRuleType>.OnlyDependOnInterfacesThat());
            return new ShouldRelateToInterfacesThat<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType> DependOnTypesThat()
        {
            _ruleCreator.BeginComplexCondition(ObjectsConditionDefinition<TRuleType>.DependOnTypesThat());
            return new ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType>(_ruleCreator);
        }

        public ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType> OnlyDependOnTypesThat()
        {
            _ruleCreator.BeginComplexCondition(ObjectsConditionDefinition<TRuleType>.OnlyDependOnTypesThat());
            return new ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction NotExist()
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotExist());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBe(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotBe(firstObject, moreObjects));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBe(IEnumerable<ICanBeAnalyzed> objects)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotBe(objects));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotDependOn(string pattern)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotDependOn(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotDependOn(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotDependOn(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotDependOn(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotDependOn(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotDependOn(IObjectProvider<IType> types)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotDependOn(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotDependOn(IEnumerable<IType> types)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotDependOn(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotDependOn(IEnumerable<Type> types)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotDependOn(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveName(string name)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotHaveName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveFullName(string fullname)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotHaveFullName(fullname));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveNameStartingWith(string pattern)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotHaveNameStartingWith(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveNameEndingWith(string pattern)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotHaveNameEndingWith(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveNameContaining(string pattern)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotHaveNameContaining(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBePrivate()
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotBePrivate());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBePublic()
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotBePublic());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeProtected()
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotBeProtected());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeInternal()
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotBeInternal());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeProtectedInternal()
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotBeProtectedInternal());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBePrivateProtected()
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotBePrivateProtected());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        //Relation Condition Negations

        public ShouldRelateToClassesThat<TRuleTypeShouldConjunction, TRuleType> NotDependOnClassesThat()
        {
            _ruleCreator.BeginComplexCondition(ObjectsConditionDefinition<TRuleType>.NotDependOnClassesThat());
            return new ShouldRelateToClassesThat<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public ShouldRelateToInterfacesThat<TRuleTypeShouldConjunction, TRuleType> NotDependOnInterfacesThat()
        {
            _ruleCreator.BeginComplexCondition(ObjectsConditionDefinition<TRuleType>.NotDependOnInterfacesThat());
            return new ShouldRelateToInterfacesThat<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType> NotDependOnTypesThat()
        {
            _ruleCreator.BeginComplexCondition(ObjectsConditionDefinition<TRuleType>.NotDependOnTypesThat());
            return new ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType>(_ruleCreator);
        }
    }
}