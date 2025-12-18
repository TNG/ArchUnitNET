using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
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

        public TNextElement BeCalledBy(IType firstType, params IType[] moreTypes) => CreateNextElement(MethodMemberConditionsDefinition.BeCalledBy(firstType, moreTypes));
        public TNextElement BeCalledBy(Type type, params Type[] moreTypes) => CreateNextElement(MethodMemberConditionsDefinition.BeCalledBy(type, moreTypes));
        public TNextElement BeCalledBy(IObjectProvider<IType> types) => CreateNextElement(MethodMemberConditionsDefinition.BeCalledBy(types));
        public TNextElement BeCalledBy(IEnumerable<IType> types) => CreateNextElement(MethodMemberConditionsDefinition.BeCalledBy(types));
        public TNextElement BeCalledBy(IEnumerable<Type> types) => CreateNextElement(MethodMemberConditionsDefinition.BeCalledBy(types));

        public TNextElement HaveDependencyInMethodBodyTo(IType firstType, params IType[] moreTypes) => CreateNextElement(MethodMemberConditionsDefinition.HaveDependencyInMethodBodyTo(firstType, moreTypes));
        public TNextElement HaveDependencyInMethodBodyTo(Type type, params Type[] moreTypes) => CreateNextElement(MethodMemberConditionsDefinition.HaveDependencyInMethodBodyTo(type, moreTypes));
        public TNextElement HaveDependencyInMethodBodyTo(IObjectProvider<IType> types) => CreateNextElement(MethodMemberConditionsDefinition.HaveDependencyInMethodBodyTo(types));
        public TNextElement HaveDependencyInMethodBodyTo(IEnumerable<IType> types) => CreateNextElement(MethodMemberConditionsDefinition.HaveDependencyInMethodBodyTo(types));
        public TNextElement HaveDependencyInMethodBodyTo(IEnumerable<Type> types) => CreateNextElement(MethodMemberConditionsDefinition.HaveDependencyInMethodBodyTo(types));

        public TNextElement HaveReturnType(IType firstType, params IType[] moreTypes) => CreateNextElement(MethodMemberConditionsDefinition.HaveReturnType(firstType, moreTypes));
        public TNextElement HaveReturnType(IEnumerable<IType> types) => CreateNextElement(MethodMemberConditionsDefinition.HaveReturnType(types));
        public TNextElement HaveReturnType(IObjectProvider<IType> types) => CreateNextElement(MethodMemberConditionsDefinition.HaveReturnType(types));
        public TNextElement HaveReturnType(Type type, params Type[] moreTypes) => CreateNextElement(MethodMemberConditionsDefinition.HaveReturnType(type, moreTypes));
        public TNextElement HaveReturnType(IEnumerable<Type> types) => CreateNextElement(MethodMemberConditionsDefinition.HaveReturnType(types));

        public ShouldRelateToMethodMembersThat<TNextElement, MethodMember> BeMethodMembersThat() => BeginComplexMethodMemberCondition(MethodMemberConditionsDefinition.BeMethodMembersThat());

        //Negations

        public TNextElement BeNoConstructor() => CreateNextElement(MethodMemberConditionsDefinition.BeNoConstructor());

        public TNextElement NotBeVirtual() => CreateNextElement(MethodMemberConditionsDefinition.NotBeVirtual());

        public TNextElement NotBeCalledBy(IType firstType, params IType[] moreTypes) => CreateNextElement(MethodMemberConditionsDefinition.NotBeCalledBy(firstType, moreTypes));
        public TNextElement NotBeCalledBy(Type type, params Type[] moreTypes) => CreateNextElement(MethodMemberConditionsDefinition.NotBeCalledBy(type, moreTypes));
        public TNextElement NotBeCalledBy(IObjectProvider<IType> types) => CreateNextElement(MethodMemberConditionsDefinition.NotBeCalledBy(types));
        public TNextElement NotBeCalledBy(IEnumerable<IType> types) => CreateNextElement(MethodMemberConditionsDefinition.NotBeCalledBy(types));
        public TNextElement NotBeCalledBy(IEnumerable<Type> types) => CreateNextElement(MethodMemberConditionsDefinition.NotBeCalledBy(types));

        public TNextElement NotHaveDependencyInMethodBodyTo(IType firstType, params IType[] moreTypes) => CreateNextElement(MethodMemberConditionsDefinition.NotHaveDependencyInMethodBodyTo(firstType, moreTypes));
        public TNextElement NotHaveDependencyInMethodBodyTo(Type type, params Type[] moreTypes) => CreateNextElement(MethodMemberConditionsDefinition.NotHaveDependencyInMethodBodyTo(type, moreTypes));
        public TNextElement NotHaveDependencyInMethodBodyTo(IObjectProvider<IType> types) => CreateNextElement(MethodMemberConditionsDefinition.NotHaveDependencyInMethodBodyTo(types));
        public TNextElement NotHaveDependencyInMethodBodyTo(IEnumerable<IType> types) => CreateNextElement(MethodMemberConditionsDefinition.NotHaveDependencyInMethodBodyTo(types));
        public TNextElement NotHaveDependencyInMethodBodyTo(IEnumerable<Type> types) => CreateNextElement(MethodMemberConditionsDefinition.NotHaveDependencyInMethodBodyTo(types));

        public TNextElement NotHaveReturnType(IType firstType, params IType[] moreTypes) => CreateNextElement(MethodMemberConditionsDefinition.NotHaveReturnType(firstType, moreTypes));
        public TNextElement NotHaveReturnType(IEnumerable<IType> types) => CreateNextElement(MethodMemberConditionsDefinition.NotHaveReturnType(types));
        public TNextElement NotHaveReturnType(IObjectProvider<IType> types) => CreateNextElement(MethodMemberConditionsDefinition.NotHaveReturnType(types));
        public TNextElement NotHaveReturnType(Type type, params Type[] moreTypes) => CreateNextElement(MethodMemberConditionsDefinition.NotHaveReturnType(type, moreTypes));
        public TNextElement NotHaveReturnType(IEnumerable<Type> types) => CreateNextElement(MethodMemberConditionsDefinition.NotHaveReturnType(types));
        // csharpier-ignore-end
    }
}
