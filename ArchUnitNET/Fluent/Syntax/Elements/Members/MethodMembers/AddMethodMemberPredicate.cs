using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public abstract class AddMethodMemberPredicate<TNextElement, TRelatedType>
        : AddMemberPredicate<TNextElement, TRelatedType, MethodMember>,
            IAddMethodMemberPredicate<TNextElement, MethodMember>
        where TRelatedType : ICanBeAnalyzed
    {
        internal AddMethodMemberPredicate(IArchRuleCreator<TRelatedType> ruleCreator)
            : base(ruleCreator) { }

        // csharpier-ignore-start
        public TNextElement AreConstructors() => CreateNextElement(MethodMemberPredicatesDefinition.AreConstructors());

        public TNextElement AreVirtual() => CreateNextElement(MethodMemberPredicatesDefinition.AreVirtual());

        public TNextElement AreCalledBy(IType firstType, params IType[] moreTypes) => CreateNextElement(MethodMemberPredicatesDefinition.AreCalledBy(firstType, moreTypes));
        public TNextElement AreCalledBy(Type type, params Type[] moreTypes) => CreateNextElement(MethodMemberPredicatesDefinition.AreCalledBy(type, moreTypes));
        public TNextElement AreCalledBy(IObjectProvider<IType> types) => CreateNextElement(MethodMemberPredicatesDefinition.AreCalledBy(types));
        public TNextElement AreCalledBy(IEnumerable<IType> types) => CreateNextElement(MethodMemberPredicatesDefinition.AreCalledBy(types));
        public TNextElement AreCalledBy(IEnumerable<Type> types) => CreateNextElement(MethodMemberPredicatesDefinition.AreCalledBy(types));

        public TNextElement HaveDependencyInMethodBodyTo(IType firstType, params IType[] moreTypes) => CreateNextElement(MethodMemberPredicatesDefinition.HaveDependencyInMethodBodyTo(firstType, moreTypes));
        public TNextElement HaveDependencyInMethodBodyTo(Type type, params Type[] moreTypes) => CreateNextElement(MethodMemberPredicatesDefinition.HaveDependencyInMethodBodyTo(type, moreTypes));
        public TNextElement HaveDependencyInMethodBodyTo(IObjectProvider<IType> types) => CreateNextElement(MethodMemberPredicatesDefinition.HaveDependencyInMethodBodyTo(types));
        public TNextElement HaveDependencyInMethodBodyTo(IEnumerable<IType> types) => CreateNextElement(MethodMemberPredicatesDefinition.HaveDependencyInMethodBodyTo(types));
        public TNextElement HaveDependencyInMethodBodyTo(IEnumerable<Type> types) => CreateNextElement(MethodMemberPredicatesDefinition.HaveDependencyInMethodBodyTo(types));

        public TNextElement HaveReturnType(IType firstType, params IType[] moreTypes) => CreateNextElement(MethodMemberPredicatesDefinition.HaveReturnType(firstType, moreTypes));
        public TNextElement HaveReturnType(IEnumerable<IType> types) => CreateNextElement(MethodMemberPredicatesDefinition.HaveReturnType(types));
        public TNextElement HaveReturnType(IObjectProvider<IType> types) => CreateNextElement(MethodMemberPredicatesDefinition.HaveReturnType(types));
        public TNextElement HaveReturnType(Type type, params Type[] moreTypes) => CreateNextElement(MethodMemberPredicatesDefinition.HaveReturnType(type, moreTypes));
        public TNextElement HaveReturnType(IEnumerable<Type> types) => CreateNextElement(MethodMemberPredicatesDefinition.HaveReturnType(types));

        //Negations

        public TNextElement AreNoConstructors() => CreateNextElement(MethodMemberPredicatesDefinition.AreNoConstructors());

        public TNextElement AreNotVirtual() => CreateNextElement(MethodMemberPredicatesDefinition.AreNotVirtual());

        public TNextElement AreNotCalledBy(IType firstType, params IType[] moreTypes) => CreateNextElement(MethodMemberPredicatesDefinition.AreNotCalledBy(firstType, moreTypes));
        public TNextElement AreNotCalledBy(Type type, params Type[] moreTypes) => CreateNextElement(MethodMemberPredicatesDefinition.AreNotCalledBy(type, moreTypes));
        public TNextElement AreNotCalledBy(IObjectProvider<IType> types) => CreateNextElement(MethodMemberPredicatesDefinition.AreNotCalledBy(types));
        public TNextElement AreNotCalledBy(IEnumerable<IType> types) => CreateNextElement(MethodMemberPredicatesDefinition.AreNotCalledBy(types));
        public TNextElement AreNotCalledBy(IEnumerable<Type> types) => CreateNextElement(MethodMemberPredicatesDefinition.AreNotCalledBy(types));

        public TNextElement DoNotHaveDependencyInMethodBodyTo(IType firstType, params IType[] moreTypes) => CreateNextElement(MethodMemberPredicatesDefinition.DoNotHaveDependencyInMethodBodyTo(firstType, moreTypes));
        public TNextElement DoNotHaveDependencyInMethodBodyTo(Type type, params Type[] moreTypes) => CreateNextElement(MethodMemberPredicatesDefinition.DoNotHaveDependencyInMethodBodyTo(type, moreTypes));
        public TNextElement DoNotHaveDependencyInMethodBodyTo(IObjectProvider<IType> types) => CreateNextElement(MethodMemberPredicatesDefinition.DoNotHaveDependencyInMethodBodyTo(types));
        public TNextElement DoNotHaveDependencyInMethodBodyTo(IEnumerable<IType> types) => CreateNextElement(MethodMemberPredicatesDefinition.DoNotHaveDependencyInMethodBodyTo(types));
        public TNextElement DoNotHaveDependencyInMethodBodyTo(IEnumerable<Type> types) => CreateNextElement(MethodMemberPredicatesDefinition.DoNotHaveDependencyInMethodBodyTo(types));

        public TNextElement DoNotHaveReturnType(IType firstType, params IType[] moreTypes) => CreateNextElement(MethodMemberPredicatesDefinition.DoNotHaveReturnType(firstType, moreTypes));
        public TNextElement DoNotHaveReturnType(IEnumerable<IType> types) => CreateNextElement(MethodMemberPredicatesDefinition.DoNotHaveReturnType(types));
        public TNextElement DoNotHaveReturnType(IObjectProvider<IType> types) => CreateNextElement(MethodMemberPredicatesDefinition.DoNotHaveReturnType(types));
        public TNextElement DoNotHaveReturnType(Type type, params Type[] moreTypes) => CreateNextElement(MethodMemberPredicatesDefinition.DoNotHaveReturnType(type, moreTypes));
        public TNextElement DoNotHaveReturnType(IEnumerable<Type> types) => CreateNextElement(MethodMemberPredicatesDefinition.DoNotHaveReturnType(types));

        // csharpier-ignore-end
    }
}
