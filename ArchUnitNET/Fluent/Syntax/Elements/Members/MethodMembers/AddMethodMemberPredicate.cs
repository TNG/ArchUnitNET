using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;

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

        public TNextElement AreCalledBy() => AreCalledBy(new ObjectProvider<IType>());
        public TNextElement AreCalledBy(params IType[] types) => AreCalledBy(new ObjectProvider<IType>(types));
        public TNextElement AreCalledBy(params Type[] types) => AreCalledBy(new SystemTypeObjectProvider<IType>(types));
        public TNextElement AreCalledBy(IObjectProvider<IType> types) => CreateNextElement(MethodMemberPredicatesDefinition.AreCalledBy(types));
        public TNextElement AreCalledBy(IEnumerable<IType> types) => AreCalledBy(new ObjectProvider<IType>(types));
        public TNextElement AreCalledBy(IEnumerable<Type> types) => AreCalledBy(new SystemTypeObjectProvider<IType>(types));

        public TNextElement HaveDependencyInMethodBodyTo() => HaveDependencyInMethodBodyTo(new ObjectProvider<IType>());
        public TNextElement HaveDependencyInMethodBodyTo(params IType[] types) => HaveDependencyInMethodBodyTo(new ObjectProvider<IType>(types));
        public TNextElement HaveDependencyInMethodBodyTo(params Type[] types) => HaveDependencyInMethodBodyTo(new SystemTypeObjectProvider<IType>(types));
        public TNextElement HaveDependencyInMethodBodyTo(IObjectProvider<IType> types) => CreateNextElement(MethodMemberPredicatesDefinition.HaveDependencyInMethodBodyTo(types));
        public TNextElement HaveDependencyInMethodBodyTo(IEnumerable<IType> types) => HaveDependencyInMethodBodyTo(new ObjectProvider<IType>(types));
        public TNextElement HaveDependencyInMethodBodyTo(IEnumerable<Type> types) => HaveDependencyInMethodBodyTo(new SystemTypeObjectProvider<IType>(types));

        public TNextElement HaveReturnType() => HaveReturnType(new ObjectProvider<IType>());
        public TNextElement HaveReturnType(params IType[] types) => HaveReturnType(new ObjectProvider<IType>(types));
        public TNextElement HaveReturnType(params Type[] types) => HaveReturnType((IEnumerable<Type>)types);
        public TNextElement HaveReturnType(IObjectProvider<IType> types) => CreateNextElement(MethodMemberPredicatesDefinition.HaveReturnType(types));
        public TNextElement HaveReturnType(IEnumerable<IType> types) => HaveReturnType(new ObjectProvider<IType>(types));
        public TNextElement HaveReturnType(IEnumerable<Type> types) => CreateNextElement(MethodMemberPredicatesDefinition.HaveReturnType(types));

        //Negations

        public TNextElement AreNoConstructors() => CreateNextElement(MethodMemberPredicatesDefinition.AreNoConstructors());

        public TNextElement AreNotVirtual() => CreateNextElement(MethodMemberPredicatesDefinition.AreNotVirtual());

        public TNextElement AreNotCalledBy() => AreNotCalledBy(new ObjectProvider<IType>());
        public TNextElement AreNotCalledBy(params IType[] types) => AreNotCalledBy(new ObjectProvider<IType>(types));
        public TNextElement AreNotCalledBy(params Type[] types) => AreNotCalledBy(new SystemTypeObjectProvider<IType>(types));
        public TNextElement AreNotCalledBy(IObjectProvider<IType> types) => CreateNextElement(MethodMemberPredicatesDefinition.AreNotCalledBy(types));
        public TNextElement AreNotCalledBy(IEnumerable<IType> types) => AreNotCalledBy(new ObjectProvider<IType>(types));
        public TNextElement AreNotCalledBy(IEnumerable<Type> types) => AreNotCalledBy(new SystemTypeObjectProvider<IType>(types));

        public TNextElement DoNotHaveDependencyInMethodBodyTo() => DoNotHaveDependencyInMethodBodyTo(new ObjectProvider<IType>());
        public TNextElement DoNotHaveDependencyInMethodBodyTo(params IType[] types) => DoNotHaveDependencyInMethodBodyTo(new ObjectProvider<IType>(types));
        public TNextElement DoNotHaveDependencyInMethodBodyTo(params Type[] types) => DoNotHaveDependencyInMethodBodyTo(new SystemTypeObjectProvider<IType>(types));
        public TNextElement DoNotHaveDependencyInMethodBodyTo(IObjectProvider<IType> types) => CreateNextElement(MethodMemberPredicatesDefinition.DoNotHaveDependencyInMethodBodyTo(types));
        public TNextElement DoNotHaveDependencyInMethodBodyTo(IEnumerable<IType> types) => DoNotHaveDependencyInMethodBodyTo(new ObjectProvider<IType>(types));
        public TNextElement DoNotHaveDependencyInMethodBodyTo(IEnumerable<Type> types) => DoNotHaveDependencyInMethodBodyTo(new SystemTypeObjectProvider<IType>(types));

        public TNextElement DoNotHaveReturnType() => DoNotHaveReturnType(new ObjectProvider<IType>());
        public TNextElement DoNotHaveReturnType(params IType[] types) => DoNotHaveReturnType(new ObjectProvider<IType>(types));
        public TNextElement DoNotHaveReturnType(params Type[] types) => DoNotHaveReturnType((IEnumerable<Type>)types);
        public TNextElement DoNotHaveReturnType(IObjectProvider<IType> types) => CreateNextElement(MethodMemberPredicatesDefinition.DoNotHaveReturnType(types));
        public TNextElement DoNotHaveReturnType(IEnumerable<IType> types) => DoNotHaveReturnType(new ObjectProvider<IType>(types));
        public TNextElement DoNotHaveReturnType(IEnumerable<Type> types) => CreateNextElement(MethodMemberPredicatesDefinition.DoNotHaveReturnType(types));

        // csharpier-ignore-end
    }
}
