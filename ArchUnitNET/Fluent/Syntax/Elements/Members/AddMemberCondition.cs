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
        internal AddMemberCondition(IArchRuleCreator<TRuleType> ruleCreator)
            : base(ruleCreator) { }

        // csharpier-ignore-start
        public TNextElement BeDeclaredIn() => BeDeclaredIn(new ObjectProvider<IType>());
        public TNextElement BeDeclaredIn(params IType[] types) => BeDeclaredIn(new ObjectProvider<IType>(types));
        public TNextElement BeDeclaredIn(params Type[] types) => BeDeclaredIn(new SystemTypeObjectProvider<IType>(types));
        public TNextElement BeDeclaredIn(IObjectProvider<IType> types) => CreateNextElement(MemberConditionsDefinition<TRuleType>.BeDeclaredIn(types));
        public TNextElement BeDeclaredIn(IEnumerable<IType> types) => BeDeclaredIn(new ObjectProvider<IType>(types));
        public TNextElement BeDeclaredIn(IEnumerable<Type> types) => BeDeclaredIn(new SystemTypeObjectProvider<IType>(types));

        public TNextElement BeStatic() => CreateNextElement(MemberConditionsDefinition<TRuleType>.BeStatic());
        public TNextElement BeReadOnly() => CreateNextElement(MemberConditionsDefinition<TRuleType>.BeReadOnly());
        public TNextElement BeImmutable() => CreateNextElement(MemberConditionsDefinition<TRuleType>.BeImmutable());

        //Relation Conditions

        public ShouldRelateToTypesThat<TNextElement, TRuleType> BeDeclaredInTypesThat() => BeginComplexTypeCondition(MemberConditionsDefinition<TRuleType>.BeDeclaredInTypesThat());

        //Negations

        public TNextElement NotBeDeclaredIn() => NotBeDeclaredIn(new ObjectProvider<IType>());
        public TNextElement NotBeDeclaredIn(params IType[] types) => NotBeDeclaredIn(new ObjectProvider<IType>(types));
        public TNextElement NotBeDeclaredIn(params Type[] types) => NotBeDeclaredIn(new SystemTypeObjectProvider<IType>(types));
        public TNextElement NotBeDeclaredIn(IObjectProvider<IType> types) => CreateNextElement(MemberConditionsDefinition<TRuleType>.NotBeDeclaredIn(types));
        public TNextElement NotBeDeclaredIn(IEnumerable<IType> types) => NotBeDeclaredIn(new ObjectProvider<IType>(types));
        public TNextElement NotBeDeclaredIn(IEnumerable<Type> types) => NotBeDeclaredIn(new SystemTypeObjectProvider<IType>(types));

        public TNextElement NotBeStatic() => CreateNextElement(MemberConditionsDefinition<TRuleType>.NotBeStatic());
        public TNextElement NotBeReadOnly() => CreateNextElement(MemberConditionsDefinition<TRuleType>.NotBeReadOnly());
        public TNextElement NotBeImmutable() => CreateNextElement(MemberConditionsDefinition<TRuleType>.NotBeImmutable());

        //Relation Condition Negations

        public ShouldRelateToTypesThat<TNextElement, TRuleType> NotBeDeclaredInTypesThat() => BeginComplexTypeCondition(MemberConditionsDefinition<TRuleType>.NotBeDeclaredInTypesThat());
        // csharpier-ignore-end
    }
}
