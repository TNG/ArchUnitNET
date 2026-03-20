using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Conditions;
using static ArchUnitNET.Domain.Extensions.EnumerableExtensions;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public static class MemberConditionsDefinition<TRuleType>
        where TRuleType : IMember
    {
        public static IOrderedCondition<TRuleType> BeDeclaredIn(
            IObjectProvider<IType> objectProvider
        )
        {
            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> members,
                Architecture architecture
            )
            {
                var typeList = objectProvider.GetObjects(architecture).ToList();
                var isExpectedType = CreateLookupFn(typeList);
                foreach (var member in members)
                {
                    if (isExpectedType(member.DeclaringType))
                    {
                        yield return new ConditionResult(member, true);
                    }
                    else
                    {
                        yield return new ConditionResult(
                            member,
                            false,
                            "is declared in " + member.DeclaringType.FullName
                        );
                    }
                }
            }

            var description = objectProvider.FormatDescription(
                "be declared in no type (always false)",
                "be declared in",
                "be declared in"
            );
            return new OrderedArchitectureCondition<TRuleType>(Condition, description);
        }

        public static IOrderedCondition<TRuleType> BeStatic()
        {
            return new SimpleCondition<TRuleType>(
                member => !member.IsStatic.HasValue || member.IsStatic.Value,
                "be static",
                "is not static"
            );
        }

        public static IOrderedCondition<TRuleType> BeReadOnly()
        {
            return new SimpleCondition<TRuleType>(
                member => member.Writability == Writability.ReadOnly,
                "be read only",
                "is not read only"
            );
        }

        public static IOrderedCondition<TRuleType> BeImmutable()
        {
            return new SimpleCondition<TRuleType>(
                member => member.Writability.IsImmutable(),
                "be immutable",
                "is not immutable"
            );
        }

        //Relation Conditions

        public static RelationCondition<TRuleType, IType> BeDeclaredInTypesThat()
        {
            return new RelationCondition<TRuleType, IType>(
                BeDeclaredIn,
                "be declared in types that",
                "is not declared in types that"
            );
        }

        //Negations

        public static IOrderedCondition<TRuleType> NotBeDeclaredIn(
            IObjectProvider<IType> objectProvider
        )
        {
            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> members,
                Architecture architecture
            )
            {
                var typeList = objectProvider.GetObjects(architecture).ToList();
                var isForbiddenType = CreateLookupFn(typeList);
                foreach (var member in members)
                {
                    if (!isForbiddenType(member.DeclaringType))
                    {
                        yield return new ConditionResult(member, true);
                    }
                    else
                    {
                        yield return new ConditionResult(
                            member,
                            false,
                            "is declared in " + member.DeclaringType.FullName
                        );
                    }
                }
            }

            var description = objectProvider.FormatDescription(
                "not be declared in no type (always true)",
                "not be declared in",
                "not be declared in"
            );
            return new OrderedArchitectureCondition<TRuleType>(Condition, description);
        }

        public static IOrderedCondition<TRuleType> NotBeStatic()
        {
            return new SimpleCondition<TRuleType>(
                member => !member.IsStatic.HasValue || !member.IsStatic.Value,
                "not be static",
                "is static"
            );
        }

        public static IOrderedCondition<TRuleType> NotBeReadOnly()
        {
            return new SimpleCondition<TRuleType>(
                member => member.Writability != Writability.ReadOnly,
                "not be read only",
                "is read only"
            );
        }

        public static IOrderedCondition<TRuleType> NotBeImmutable()
        {
            return new SimpleCondition<TRuleType>(
                member => !member.Writability.IsImmutable(),
                "not be immutable",
                "is immutable"
            );
        }

        //Relation Condition Negations

        public static RelationCondition<TRuleType, IType> NotBeDeclaredInTypesThat()
        {
            return new RelationCondition<TRuleType, IType>(
                NotBeDeclaredIn,
                "not be declared in types that",
                "is declared in types that"
            );
        }
    }
}
