using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax.Elements.Types;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Classes;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public abstract class ObjectsShould<TRuleTypeShouldConjunction, TRuleType> : SyntaxElement<TRuleType>,
        IObjectConditions<TRuleTypeShouldConjunction, TRuleType>
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

        public TRuleTypeShouldConjunction DependOnAnyTypesWithFullNameMatching(string pattern)
        {
            _ruleCreator.AddCondition(
                ObjectsConditionDefinition<TRuleType>.DependOnAnyTypesWithFullNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DependOnAny(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.DependOnAny(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DependOnAny(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.DependOnAny(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DependOnAny(IObjectProvider<IType> types)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.DependOnAny(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DependOnAny(IEnumerable<IType> types)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.DependOnAny(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DependOnAny(IEnumerable<Type> types)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.DependOnAny(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyDependOnTypesWithFullNameMatching(string pattern)
        {
            _ruleCreator.AddCondition(
                ObjectsConditionDefinition<TRuleType>.OnlyDependOnTypesWithFullNameMatching(pattern));
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

        public TRuleTypeShouldConjunction HaveNameMatching(string pattern)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.HaveNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFullName(string fullname)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.HaveFullName(fullname));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFullNameMatching(string pattern)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.HaveFullNameMatching(pattern));
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

        public ShouldRelateToClassesThat<TRuleTypeShouldConjunction, TRuleType> DependOnAnyClassesThat()
        {
            _ruleCreator.BeginComplexCondition(ObjectsConditionDefinition<TRuleType>.DependOnAnyClassesThat());
            return new ShouldRelateToClassesThat<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public ShouldRelateToClassesThat<TRuleTypeShouldConjunction, TRuleType> OnlyDependOnClassesThat()
        {
            _ruleCreator.BeginComplexCondition(ObjectsConditionDefinition<TRuleType>.OnlyDependOnClassesThat());
            return new ShouldRelateToClassesThat<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public ShouldRelateToInterfacesThat<TRuleTypeShouldConjunction, TRuleType> DependOnAnyInterfacesThat()
        {
            _ruleCreator.BeginComplexCondition(ObjectsConditionDefinition<TRuleType>.DependOnAnyInterfacesThat());
            return new ShouldRelateToInterfacesThat<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public ShouldRelateToInterfacesThat<TRuleTypeShouldConjunction, TRuleType> OnlyDependOnInterfacesThat()
        {
            _ruleCreator.BeginComplexCondition(ObjectsConditionDefinition<TRuleType>.OnlyDependOnInterfacesThat());
            return new ShouldRelateToInterfacesThat<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType> DependOnAnyTypesThat()
        {
            _ruleCreator.BeginComplexCondition(ObjectsConditionDefinition<TRuleType>.DependOnAnyTypesThat());
            return new ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType>(_ruleCreator);
        }

        public ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType> OnlyDependOnTypesThat()
        {
            _ruleCreator.BeginComplexCondition(ObjectsConditionDefinition<TRuleType>.OnlyDependOnTypesThat());
            return new ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType>(_ruleCreator);
        }

        public ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType> HaveAttributesThat()
        {
            _ruleCreator.BeginComplexCondition(ObjectsConditionDefinition<TRuleType>.HaveAttributesThat());
            return new ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType> OnlyHaveAttributesThat()
        {
            _ruleCreator.BeginComplexCondition(ObjectsConditionDefinition<TRuleType>.OnlyHaveAttributesThat());
            return new ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
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

        public TRuleTypeShouldConjunction NotDependOnAnyTypesWithFullNameMatching(string pattern)
        {
            _ruleCreator.AddCondition(
                ObjectsConditionDefinition<TRuleType>.NotDependOnAnyTypesWithFullNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotDependOnAny(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotDependOnAny(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotDependOnAny(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotDependOnAny(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotDependOnAny(IObjectProvider<IType> types)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotDependOnAny(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotDependOnAny(IEnumerable<IType> types)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotDependOnAny(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotDependOnAny(IEnumerable<Type> types)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotDependOnAny(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveName(string name)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotHaveName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveNameMatching(string pattern)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotHaveNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveFullName(string fullname)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotHaveFullName(fullname));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveFullNameMatching(string pattern)
        {
            _ruleCreator.AddCondition(ObjectsConditionDefinition<TRuleType>.NotHaveFullNameMatching(pattern));
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

        public ShouldRelateToClassesThat<TRuleTypeShouldConjunction, TRuleType> NotDependOnAnyClassesThat()
        {
            _ruleCreator.BeginComplexCondition(ObjectsConditionDefinition<TRuleType>.NotDependOnAnyClassesThat());
            return new ShouldRelateToClassesThat<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public ShouldRelateToInterfacesThat<TRuleTypeShouldConjunction, TRuleType> NotDependOnAnyInterfacesThat()
        {
            _ruleCreator.BeginComplexCondition(ObjectsConditionDefinition<TRuleType>.NotDependOnAnyInterfacesThat());
            return new ShouldRelateToInterfacesThat<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType> NotDependOnAnyTypesThat()
        {
            _ruleCreator.BeginComplexCondition(ObjectsConditionDefinition<TRuleType>.NotDependOnAnyTypesThat());
            return new ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType>(_ruleCreator);
        }

        public ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType> NotHaveAttributesThat()
        {
            _ruleCreator.BeginComplexCondition(ObjectsConditionDefinition<TRuleType>.NotHaveAttributesThat());
            return new ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}