using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public abstract class AddMemberPredicate<TNextElement, TRuleType>
        : AddObjectPredicate<TNextElement, TRuleType>,
            IAddMemberPredicate<TNextElement, TRuleType>
        where TRuleType : IMember
    {
        internal AddMemberPredicate(
            PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<TRuleType> objectProvider
        )
            : base(partialArchRuleConjunction, objectProvider) { }

        // csharpier-ignore-start
        public TNextElement AreDeclaredIn(IType firstType, params IType[] moreTypes) => CreateNextElement(MemberPredicatesDefinition<TRuleType>.AreDeclaredIn(firstType, moreTypes));
        public TNextElement AreDeclaredIn(Type firstType, params Type[] moreTypes) => CreateNextElement(MemberPredicatesDefinition<TRuleType>.AreDeclaredIn(firstType, moreTypes));
        public TNextElement AreDeclaredIn(IObjectProvider<IType> types) => CreateNextElement(MemberPredicatesDefinition<TRuleType>.AreDeclaredIn(types));
        public TNextElement AreDeclaredIn(IEnumerable<IType> types) => CreateNextElement(MemberPredicatesDefinition<TRuleType>.AreDeclaredIn(types));
        public TNextElement AreDeclaredIn(IEnumerable<Type> types) => CreateNextElement(MemberPredicatesDefinition<TRuleType>.AreDeclaredIn(types));

        public TNextElement AreStatic() => CreateNextElement(MemberPredicatesDefinition<TRuleType>.AreStatic());
        public TNextElement AreReadOnly() => CreateNextElement(MemberPredicatesDefinition<TRuleType>.AreReadOnly());
        public TNextElement AreImmutable() => CreateNextElement(MemberPredicatesDefinition<TRuleType>.AreImmutable());

        //Negations

        public TNextElement AreNotDeclaredIn(IType firstType, params IType[] moreTypes) => CreateNextElement(MemberPredicatesDefinition<TRuleType>.AreNotDeclaredIn(firstType, moreTypes));
        public TNextElement AreNotDeclaredIn(Type firstType, params Type[] moreTypes) => CreateNextElement(MemberPredicatesDefinition<TRuleType>.AreNotDeclaredIn(firstType, moreTypes));
        public TNextElement AreNotDeclaredIn(IObjectProvider<IType> types) => CreateNextElement(MemberPredicatesDefinition<TRuleType>.AreNotDeclaredIn(types));
        public TNextElement AreNotDeclaredIn(IEnumerable<IType> types) => CreateNextElement(MemberPredicatesDefinition<TRuleType>.AreNotDeclaredIn(types));
        public TNextElement AreNotDeclaredIn(IEnumerable<Type> types) => CreateNextElement(MemberPredicatesDefinition<TRuleType>.AreNotDeclaredIn(types));

        public TNextElement AreNotStatic() => CreateNextElement(MemberPredicatesDefinition<TRuleType>.AreNotStatic());
        public TNextElement AreNotReadOnly() => CreateNextElement(MemberPredicatesDefinition<TRuleType>.AreNotReadOnly());
        public TNextElement AreNotImmutable() => CreateNextElement(MemberPredicatesDefinition<TRuleType>.AreNotImmutable());

        // csharpier-ignore-end
    }
}
