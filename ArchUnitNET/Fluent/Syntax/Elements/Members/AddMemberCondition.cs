using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax.Elements.Types;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public abstract class AddMemberCondition<TNextElement, TRuleType>
        : AddObjectCondition<TNextElement, TRuleType>,
            IAddMemberCondition<TNextElement, TRuleType>
        where TRuleType : IMember
    {
        internal AddMemberCondition(
            PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<TRuleType> objectProvider
        )
            : base(partialArchRuleConjunction, objectProvider) { }

        // csharpier-ignore-start
        public TNextElement BeDeclaredIn(IType firstType, params IType[] moreTypes) => CreateNextElement(MemberConditionsDefinition<TRuleType>.BeDeclaredIn(firstType, moreTypes));
        public TNextElement BeDeclaredIn(Type firstType, params Type[] moreTypes) => CreateNextElement(MemberConditionsDefinition<TRuleType>.BeDeclaredIn(firstType, moreTypes));
        public TNextElement BeDeclaredIn(IObjectProvider<IType> types) => CreateNextElement(MemberConditionsDefinition<TRuleType>.BeDeclaredIn(types));
        public TNextElement BeDeclaredIn(IEnumerable<IType> types) => CreateNextElement(MemberConditionsDefinition<TRuleType>.BeDeclaredIn(types));
        public TNextElement BeDeclaredIn(IEnumerable<Type> types) => CreateNextElement(MemberConditionsDefinition<TRuleType>.BeDeclaredIn(types));

        public TNextElement BeStatic() => CreateNextElement(MemberConditionsDefinition<TRuleType>.BeStatic());
        public TNextElement BeReadOnly() => CreateNextElement(MemberConditionsDefinition<TRuleType>.BeReadOnly());
        public TNextElement BeImmutable() => CreateNextElement(MemberConditionsDefinition<TRuleType>.BeImmutable());

        //Relation Conditions

        public ShouldRelateToTypesThat<TNextElement, TRuleType> BeDeclaredInTypesThat() => BeginComplexTypeCondition(MemberConditionsDefinition<TRuleType>.BeDeclaredInTypesThat());

        //Negations

        public TNextElement NotBeDeclaredIn(IType firstType, params IType[] moreTypes) => CreateNextElement(MemberConditionsDefinition<TRuleType>.NotBeDeclaredIn(firstType, moreTypes));
        public TNextElement NotBeDeclaredIn(Type firstType, params Type[] moreTypes) => CreateNextElement(MemberConditionsDefinition<TRuleType>.NotBeDeclaredIn(firstType, moreTypes));
        public TNextElement NotBeDeclaredIn(IObjectProvider<IType> types) => CreateNextElement(MemberConditionsDefinition<TRuleType>.NotBeDeclaredIn(types));
        public TNextElement NotBeDeclaredIn(IEnumerable<IType> types) => CreateNextElement(MemberConditionsDefinition<TRuleType>.NotBeDeclaredIn(types));
        public TNextElement NotBeDeclaredIn(IEnumerable<Type> types) => CreateNextElement(MemberConditionsDefinition<TRuleType>.NotBeDeclaredIn(types));

        public TNextElement NotBeStatic() => CreateNextElement(MemberConditionsDefinition<TRuleType>.NotBeStatic());
        public TNextElement NotBeReadOnly() => CreateNextElement(MemberConditionsDefinition<TRuleType>.NotBeReadOnly());
        public TNextElement NotBeImmutable() => CreateNextElement(MemberConditionsDefinition<TRuleType>.NotBeImmutable());

        //Relation Condition Negations

        public ShouldRelateToTypesThat<TNextElement, TRuleType> NotBeDeclaredInTypesThat() => BeginComplexTypeCondition(MemberConditionsDefinition<TRuleType>.NotBeDeclaredInTypesThat());
        // csharpier-ignore-end
    }
}
