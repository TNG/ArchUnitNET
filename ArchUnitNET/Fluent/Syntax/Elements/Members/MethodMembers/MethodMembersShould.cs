using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public class MethodMembersShould
        : MembersShould<MethodMembersShouldConjunction, MethodMember>,
            IComplexMethodMemberConditions
    {
        public MethodMembersShould(IArchRuleCreator<MethodMember> ruleCreator)
            : base(ruleCreator) { }

        public ShouldRelateToMethodMembersThat<
            MethodMembersShouldConjunction,
            MethodMember
        > BeMethodMembersThat()
        {
            _ruleCreator.BeginComplexCondition(
                ArchRuleDefinition.MethodMembers(),
                MethodMemberConditionsDefinition.BeMethodMembersThat()
            );
            return new ShouldRelateToMethodMembersThat<
                MethodMembersShouldConjunction,
                MethodMember
            >(_ruleCreator);
        }

        public MethodMembersShouldConjunction BeConstructor()
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.BeConstructor());
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction BeVirtual()
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.BeVirtual());
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction BeCalledBy(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddCondition(
                MethodMemberConditionsDefinition.BeCalledBy(firstType, moreTypes)
            );
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction BeCalledBy(Type type, params Type[] moreTypes)
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.BeCalledBy(type, moreTypes));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction BeCalledBy(IObjectProvider<IType> types)
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.BeCalledBy(types));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction BeCalledBy(IEnumerable<IType> types)
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.BeCalledBy(types));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction BeCalledBy(IEnumerable<Type> types)
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.BeCalledBy(types));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction HaveDependencyInMethodBodyTo(
            IType firstType,
            params IType[] moreTypes
        )
        {
            _ruleCreator.AddCondition(
                MethodMemberConditionsDefinition.HaveDependencyInMethodBodyTo(firstType, moreTypes)
            );
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction HaveDependencyInMethodBodyTo(
            Type type,
            params Type[] moreTypes
        )
        {
            _ruleCreator.AddCondition(
                MethodMemberConditionsDefinition.HaveDependencyInMethodBodyTo(type, moreTypes)
            );
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction HaveDependencyInMethodBodyTo(
            IObjectProvider<IType> types
        )
        {
            _ruleCreator.AddCondition(
                MethodMemberConditionsDefinition.HaveDependencyInMethodBodyTo(types)
            );
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction HaveDependencyInMethodBodyTo(IEnumerable<IType> types)
        {
            _ruleCreator.AddCondition(
                MethodMemberConditionsDefinition.HaveDependencyInMethodBodyTo(types)
            );
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction HaveDependencyInMethodBodyTo(IEnumerable<Type> types)
        {
            _ruleCreator.AddCondition(
                MethodMemberConditionsDefinition.HaveDependencyInMethodBodyTo(types)
            );
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction HaveReturnType(
            IType firstType,
            params IType[] moreTypes
        )
        {
            _ruleCreator.AddCondition(
                MethodMemberConditionsDefinition.HaveReturnType(firstType, moreTypes)
            );
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction HaveReturnType(IEnumerable<IType> types)
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.HaveReturnType(types));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction HaveReturnType(IObjectProvider<IType> types)
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.HaveReturnType(types));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction HaveReturnType(Type type, params Type[] moreTypes)
        {
            _ruleCreator.AddCondition(
                MethodMemberConditionsDefinition.HaveReturnType(type, moreTypes)
            );
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction HaveReturnType(IEnumerable<Type> types)
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.HaveReturnType(types));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        //Negations

        public MethodMembersShouldConjunction BeNoConstructor()
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.BeNoConstructor());
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction NotBeVirtual()
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.NotBeVirtual());
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction NotBeCalledBy(
            IType firstType,
            params IType[] moreTypes
        )
        {
            _ruleCreator.AddCondition(
                MethodMemberConditionsDefinition.NotBeCalledBy(firstType, moreTypes)
            );
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction NotBeCalledBy(Type type, params Type[] moreTypes)
        {
            _ruleCreator.AddCondition(
                MethodMemberConditionsDefinition.NotBeCalledBy(type, moreTypes)
            );
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction NotBeCalledBy(IObjectProvider<IType> types)
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.NotBeCalledBy(types));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction NotBeCalledBy(IEnumerable<IType> types)
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.NotBeCalledBy(types));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction NotBeCalledBy(IEnumerable<Type> types)
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.NotBeCalledBy(types));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction NotHaveDependencyInMethodBodyTo(
            IType firstType,
            params IType[] moreTypes
        )
        {
            _ruleCreator.AddCondition(
                MethodMemberConditionsDefinition.NotHaveDependencyInMethodBodyTo(
                    firstType,
                    moreTypes
                )
            );
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction NotHaveDependencyInMethodBodyTo(
            Type type,
            params Type[] moreTypes
        )
        {
            _ruleCreator.AddCondition(
                MethodMemberConditionsDefinition.NotHaveDependencyInMethodBodyTo(type, moreTypes)
            );
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction NotHaveDependencyInMethodBodyTo(
            IObjectProvider<IType> types
        )
        {
            _ruleCreator.AddCondition(
                MethodMemberConditionsDefinition.NotHaveDependencyInMethodBodyTo(types)
            );
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction NotHaveDependencyInMethodBodyTo(
            IEnumerable<IType> types
        )
        {
            _ruleCreator.AddCondition(
                MethodMemberConditionsDefinition.NotHaveDependencyInMethodBodyTo(types)
            );
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction NotHaveDependencyInMethodBodyTo(
            IEnumerable<Type> types
        )
        {
            _ruleCreator.AddCondition(
                MethodMemberConditionsDefinition.NotHaveDependencyInMethodBodyTo(types)
            );
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction NotHaveReturnType(
            IType firstType,
            params IType[] moreTypes
        )
        {
            _ruleCreator.AddCondition(
                MethodMemberConditionsDefinition.NotHaveReturnType(firstType, moreTypes)
            );
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction NotHaveReturnType(IEnumerable<IType> types)
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.NotHaveReturnType(types));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction NotHaveReturnType(IObjectProvider<IType> types)
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.NotHaveReturnType(types));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction NotHaveReturnType(Type type, params Type[] moreTypes)
        {
            _ruleCreator.AddCondition(
                MethodMemberConditionsDefinition.NotHaveReturnType(type, moreTypes)
            );
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction NotHaveReturnType(IEnumerable<Type> types)
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.NotHaveReturnType(types));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }
        
        
        public MethodMembersShouldConjunction HaveAnyParameters()
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.HaveAnyParameters());
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction NotHaveAnyParameters()
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.NotHaveAnyParameters());
            return new MethodMembersShouldConjunction(_ruleCreator);
        }
    }
}
