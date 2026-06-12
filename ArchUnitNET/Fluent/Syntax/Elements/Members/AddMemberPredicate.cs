using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public abstract class AddMemberPredicate<TNextElement, TRelatedType, TRuleType>
        : AddObjectPredicate<TNextElement, TRelatedType, TRuleType>,
            IAddMemberPredicate<TNextElement, TRuleType>
        where TRuleType : IMember
        where TRelatedType : ICanBeAnalyzed
    {
        internal AddMemberPredicate(IArchRuleCreator<TRelatedType> ruleCreator)
            : base(ruleCreator) { }

        // csharpier-ignore-start
        public TNextElement AreDeclaredIn() => AreDeclaredIn(new ObjectProvider<IType>());
        public TNextElement AreDeclaredIn(params IType[] types) => AreDeclaredIn(new ObjectProvider<IType>(types));
        public TNextElement AreDeclaredIn(params Type[] types) => AreDeclaredIn(new SystemTypeObjectProvider<IType>(types));
        public TNextElement AreDeclaredIn(IObjectProvider<IType> types) => CreateNextElement(MemberPredicatesDefinition<TRuleType>.AreDeclaredIn(types));
        public TNextElement AreDeclaredIn(IEnumerable<IType> types) => AreDeclaredIn(new ObjectProvider<IType>(types));
        public TNextElement AreDeclaredIn(IEnumerable<Type> types) => AreDeclaredIn(new SystemTypeObjectProvider<IType>(types));

        public TNextElement AreStatic() => CreateNextElement(MemberPredicatesDefinition<TRuleType>.AreStatic());
        public TNextElement AreReadOnly() => CreateNextElement(MemberPredicatesDefinition<TRuleType>.AreReadOnly());
        public TNextElement AreImmutable() => CreateNextElement(MemberPredicatesDefinition<TRuleType>.AreImmutable());

        //Negations

        public TNextElement AreNotDeclaredIn() => AreNotDeclaredIn(new ObjectProvider<IType>());
        public TNextElement AreNotDeclaredIn(params IType[] types) => AreNotDeclaredIn(new ObjectProvider<IType>(types));
        public TNextElement AreNotDeclaredIn(params Type[] types) => AreNotDeclaredIn(new SystemTypeObjectProvider<IType>(types));
        public TNextElement AreNotDeclaredIn(IObjectProvider<IType> types) => CreateNextElement(MemberPredicatesDefinition<TRuleType>.AreNotDeclaredIn(types));
        public TNextElement AreNotDeclaredIn(IEnumerable<IType> types) => AreNotDeclaredIn(new ObjectProvider<IType>(types));
        public TNextElement AreNotDeclaredIn(IEnumerable<Type> types) => AreNotDeclaredIn(new SystemTypeObjectProvider<IType>(types));

        public TNextElement AreNotStatic() => CreateNextElement(MemberPredicatesDefinition<TRuleType>.AreNotStatic());
        public TNextElement AreNotReadOnly() => CreateNextElement(MemberPredicatesDefinition<TRuleType>.AreNotReadOnly());
        public TNextElement AreNotImmutable() => CreateNextElement(MemberPredicatesDefinition<TRuleType>.AreNotImmutable());

        // csharpier-ignore-end
    }
}
