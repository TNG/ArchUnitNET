using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Conditions;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public abstract class AddMethodMemberCondition<TNextElement>
        : AddMemberCondition<TNextElement, MethodMember>,
            IAddMethodMemberCondition<TNextElement, MethodMember>
    {
        internal AddMethodMemberCondition(IArchRuleCreator<MethodMember> ruleCreator)
            : base(ruleCreator) { }

        // csharpier-ignore-start
        public TNextElement BeConstructor() => CreateNextElement(MethodMemberConditionsDefinition.BeConstructor());

        public TNextElement BeVirtual() => CreateNextElement(MethodMemberConditionsDefinition.BeVirtual());

        public TNextElement BeCalledBy() => BeCalledBy(new ObjectProvider<IType>());
        public TNextElement BeCalledBy(params IType[] types) => BeCalledBy(new ObjectProvider<IType>(types));
        public TNextElement BeCalledBy(params Type[] types) => BeCalledBy(new SystemTypeObjectProvider<IType>(types));
        public TNextElement BeCalledBy(IObjectProvider<IType> types) => CreateNextElement(MethodMemberConditionsDefinition.BeCalledBy(types));
        public TNextElement BeCalledBy(IEnumerable<IType> types) => BeCalledBy(new ObjectProvider<IType>(types));
        public TNextElement BeCalledBy(IEnumerable<Type> types) => BeCalledBy(new SystemTypeObjectProvider<IType>(types));

        public TNextElement HaveDependencyInMethodBodyTo() => HaveDependencyInMethodBodyTo(new ObjectProvider<IType>());
        public TNextElement HaveDependencyInMethodBodyTo(params IType[] types) => HaveDependencyInMethodBodyTo(new ObjectProvider<IType>(types));
        public TNextElement HaveDependencyInMethodBodyTo(params Type[] types) => HaveDependencyInMethodBodyTo(new SystemTypeObjectProvider<IType>(types));
        public TNextElement HaveDependencyInMethodBodyTo(IObjectProvider<IType> types) => CreateNextElement(MethodMemberConditionsDefinition.HaveDependencyInMethodBodyTo(types));
        public TNextElement HaveDependencyInMethodBodyTo(IEnumerable<IType> types) => HaveDependencyInMethodBodyTo(new ObjectProvider<IType>(types));
        public TNextElement HaveDependencyInMethodBodyTo(IEnumerable<Type> types) => HaveDependencyInMethodBodyTo(new SystemTypeObjectProvider<IType>(types));

        public TNextElement HaveReturnType() => HaveReturnType(new ObjectProvider<IType>());
        public TNextElement HaveReturnType(params IType[] types) => HaveReturnType(new ObjectProvider<IType>(types));
        public TNextElement HaveReturnType(params Type[] types) => HaveReturnType((IEnumerable<Type>)types);
        public TNextElement HaveReturnType(IObjectProvider<IType> types) => CreateNextElement(MethodMemberConditionsDefinition.HaveReturnType(types));
        public TNextElement HaveReturnType(IEnumerable<IType> types) => HaveReturnType(new ObjectProvider<IType>(types));
        public TNextElement HaveReturnType(IEnumerable<Type> types) => CreateNextElement(MethodMemberConditionsDefinition.HaveReturnType(types));

        public ShouldRelateToMethodMembersThat<TNextElement, MethodMember> BeMethodMembersThat() => BeginComplexMethodMemberCondition(MethodMemberConditionsDefinition.BeMethodMembersThat());

        //Negations

        public TNextElement BeNoConstructor() => CreateNextElement(MethodMemberConditionsDefinition.BeNoConstructor());

        public TNextElement NotBeVirtual() => CreateNextElement(MethodMemberConditionsDefinition.NotBeVirtual());

        public TNextElement NotBeCalledBy() => NotBeCalledBy(new ObjectProvider<IType>());
        public TNextElement NotBeCalledBy(params IType[] types) => NotBeCalledBy(new ObjectProvider<IType>(types));
        public TNextElement NotBeCalledBy(params Type[] types) => NotBeCalledBy(new SystemTypeObjectProvider<IType>(types));
        public TNextElement NotBeCalledBy(IObjectProvider<IType> types) => CreateNextElement(MethodMemberConditionsDefinition.NotBeCalledBy(types));
        public TNextElement NotBeCalledBy(IEnumerable<IType> types) => NotBeCalledBy(new ObjectProvider<IType>(types));
        public TNextElement NotBeCalledBy(IEnumerable<Type> types) => NotBeCalledBy(new SystemTypeObjectProvider<IType>(types));

        public TNextElement NotHaveDependencyInMethodBodyTo() => NotHaveDependencyInMethodBodyTo(new ObjectProvider<IType>());
        public TNextElement NotHaveDependencyInMethodBodyTo(params IType[] types) => NotHaveDependencyInMethodBodyTo(new ObjectProvider<IType>(types));
        public TNextElement NotHaveDependencyInMethodBodyTo(params Type[] types) => NotHaveDependencyInMethodBodyTo(new SystemTypeObjectProvider<IType>(types));
        public TNextElement NotHaveDependencyInMethodBodyTo(IObjectProvider<IType> types) => CreateNextElement(MethodMemberConditionsDefinition.NotHaveDependencyInMethodBodyTo(types));
        public TNextElement NotHaveDependencyInMethodBodyTo(IEnumerable<IType> types) => NotHaveDependencyInMethodBodyTo(new ObjectProvider<IType>(types));
        public TNextElement NotHaveDependencyInMethodBodyTo(IEnumerable<Type> types) => NotHaveDependencyInMethodBodyTo(new SystemTypeObjectProvider<IType>(types));

        public TNextElement NotHaveReturnType() => NotHaveReturnType(new ObjectProvider<IType>());
        public TNextElement NotHaveReturnType(params IType[] types) => NotHaveReturnType(new ObjectProvider<IType>(types));
        public TNextElement NotHaveReturnType(params Type[] types) => NotHaveReturnType((IEnumerable<Type>)types);
        public TNextElement NotHaveReturnType(IObjectProvider<IType> types) => CreateNextElement(MethodMemberConditionsDefinition.NotHaveReturnType(types));
        public TNextElement NotHaveReturnType(IEnumerable<IType> types) => NotHaveReturnType(new ObjectProvider<IType>(types));
        public TNextElement NotHaveReturnType(IEnumerable<Type> types) => CreateNextElement(MethodMemberConditionsDefinition.NotHaveReturnType(types));
        // csharpier-ignore-end
    }
}
