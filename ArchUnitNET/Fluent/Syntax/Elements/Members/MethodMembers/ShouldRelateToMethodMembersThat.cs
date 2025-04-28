using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public class ShouldRelateToMethodMembersThat<TRuleTypeShouldConjunction, TRuleType>
        : ShouldRelateToMembersThat<TRuleTypeShouldConjunction, MethodMember, TRuleType>,
            IMethodMemberPredicates<TRuleTypeShouldConjunction, MethodMember>
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        public ShouldRelateToMethodMembersThat(IArchRuleCreator<TRuleType> ruleCreator)
            : base(ruleCreator) { }

        public TRuleTypeShouldConjunction AreConstructors()
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.AreConstructors()
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreVirtual()
        {
            _ruleCreator.ContinueComplexCondition(MethodMemberPredicatesDefinition.AreVirtual());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use AreCalledBy(Types().That().HaveFullName()) instead"
        )]
        public TRuleTypeShouldConjunction AreCalledBy(
            string pattern,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.AreCalledBy(pattern, useRegularExpressions)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use AreCalledBy(Types().That().HaveFullName()) instead"
        )]
        public TRuleTypeShouldConjunction AreCalledBy(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.AreCalledBy(patterns, useRegularExpressions)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreCalledBy(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.AreCalledBy(firstType, moreTypes)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreCalledBy(Type type, params Type[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.AreCalledBy(type, moreTypes)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreCalledBy(IObjectProvider<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.AreCalledBy(types)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreCalledBy(IEnumerable<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.AreCalledBy(types)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreCalledBy(IEnumerable<Type> types)
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.AreCalledBy(types)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use HaveDependencyInMethodBodyTo(Types().That().HaveFullName()) instead"
        )]
        public TRuleTypeShouldConjunction HaveDependencyInMethodBodyTo(
            string pattern,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.HaveDependencyInMethodBodyTo(
                    pattern,
                    useRegularExpressions
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use HaveDependencyInMethodBodyTo(Types().That().HaveFullName()) instead"
        )]
        public TRuleTypeShouldConjunction HaveDependencyInMethodBodyTo(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.HaveDependencyInMethodBodyTo(
                    patterns,
                    useRegularExpressions
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveDependencyInMethodBodyTo(
            IType firstType,
            params IType[] moreTypes
        )
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.HaveDependencyInMethodBodyTo(firstType, moreTypes)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveDependencyInMethodBodyTo(
            Type type,
            params Type[] moreTypes
        )
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.HaveDependencyInMethodBodyTo(type, moreTypes)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveDependencyInMethodBodyTo(IObjectProvider<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.HaveDependencyInMethodBodyTo(types)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveDependencyInMethodBodyTo(IEnumerable<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.HaveDependencyInMethodBodyTo(types)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveDependencyInMethodBodyTo(IEnumerable<Type> types)
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.HaveDependencyInMethodBodyTo(types)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use HaveReturnType(Types().That().HaveFullName()) instead"
        )]
        public TRuleTypeShouldConjunction HaveReturnType(
            string pattern,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.HaveReturnType(pattern, useRegularExpressions)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use HaveReturnType(Types().That().HaveFullName()) instead"
        )]
        public TRuleTypeShouldConjunction HaveReturnType(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.HaveReturnType(patterns, useRegularExpressions)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveReturnType(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.HaveReturnType(firstType, moreTypes)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveReturnType(IEnumerable<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.HaveReturnType(types)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveReturnType(IObjectProvider<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.HaveReturnType(types)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveReturnType(Type type, params Type[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.HaveReturnType(type, moreTypes)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveReturnType(IEnumerable<Type> types)
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.HaveReturnType(types)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        //Negations


        public TRuleTypeShouldConjunction AreNoConstructors()
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.AreNoConstructors()
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotVirtual()
        {
            _ruleCreator.ContinueComplexCondition(MethodMemberPredicatesDefinition.AreNotVirtual());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use AreNotCalledBy(Types().That().HaveFullName()) instead"
        )]
        public TRuleTypeShouldConjunction AreNotCalledBy(
            string pattern,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.AreNotCalledBy(pattern, useRegularExpressions)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use AreNotCalledBy(Types().That().HaveFullName()) instead"
        )]
        public TRuleTypeShouldConjunction AreNotCalledBy(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.AreNotCalledBy(patterns, useRegularExpressions)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotCalledBy(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.AreNotCalledBy(firstType, moreTypes)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotCalledBy(Type type, params Type[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.AreNotCalledBy(type, moreTypes)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotCalledBy(IObjectProvider<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.AreNotCalledBy(types)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotCalledBy(IEnumerable<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.AreNotCalledBy(types)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotCalledBy(IEnumerable<Type> types)
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.AreNotCalledBy(types)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use DoNotHaveDependencyInMethodBodyTo(Types().That().HaveFullName()) instead"
        )]
        public TRuleTypeShouldConjunction DoNotHaveDependencyInMethodBodyTo(
            string pattern,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.DoNotHaveDependencyInMethodBodyTo(
                    pattern,
                    useRegularExpressions
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use DoNotHaveDependencyInMethodBodyTo(Types().That().HaveFullName()) instead"
        )]
        public TRuleTypeShouldConjunction DoNotHaveDependencyInMethodBodyTo(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.DoNotHaveDependencyInMethodBodyTo(
                    patterns,
                    useRegularExpressions
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveDependencyInMethodBodyTo(
            IType firstType,
            params IType[] moreTypes
        )
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.DoNotHaveDependencyInMethodBodyTo(
                    firstType,
                    moreTypes
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveDependencyInMethodBodyTo(
            Type type,
            params Type[] moreTypes
        )
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.DoNotHaveDependencyInMethodBodyTo(type, moreTypes)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveDependencyInMethodBodyTo(
            IObjectProvider<IType> types
        )
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.DoNotHaveDependencyInMethodBodyTo(types)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveDependencyInMethodBodyTo(
            IEnumerable<IType> types
        )
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.DoNotHaveDependencyInMethodBodyTo(types)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveDependencyInMethodBodyTo(IEnumerable<Type> types)
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.DoNotHaveDependencyInMethodBodyTo(types)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use DoNotHaveReturnType(Types().That().HaveFullName()) instead"
        )]
        public TRuleTypeShouldConjunction DoNotHaveReturnType(
            string pattern,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.DoNotHaveReturnType(pattern, useRegularExpressions)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use DoNotHaveReturnType(Types().That().HaveFullName()) instead"
        )]
        public TRuleTypeShouldConjunction DoNotHaveReturnType(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.DoNotHaveReturnType(
                    patterns,
                    useRegularExpressions
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveReturnType(
            IType firstType,
            params IType[] moreTypes
        )
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.DoNotHaveReturnType(firstType, moreTypes)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveReturnType(IEnumerable<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.DoNotHaveReturnType(types)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveReturnType(IObjectProvider<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.DoNotHaveReturnType(types)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveReturnType(Type type, params Type[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.DoNotHaveReturnType(type, moreTypes)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveReturnType(IEnumerable<Type> types)
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.DoNotHaveReturnType(types)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}
