using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Conditions
{
    public class OrderedArchitectureCondition<TRuleType>
        : ArchitectureCondition<TRuleType>,
            IOrderedCondition<TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        public OrderedArchitectureCondition(
            Func<TRuleType, Architecture, bool> condition,
            string description,
            string failDescription
        )
            : base(
                (ruleTypes, architecture) =>
                    ruleTypes.Select(type => new ConditionResult(
                        type,
                        condition(type, architecture),
                        failDescription
                    )),
                description
            ) { }

        public OrderedArchitectureCondition(
            Func<TRuleType, Architecture, bool> condition,
            Func<TRuleType, Architecture, string> dynamicFailDescription,
            string description
        )
            : base(
                (ruleTypes, architecture) =>
                    ruleTypes.Select(type => new ConditionResult(
                        type,
                        condition(type, architecture),
                        dynamicFailDescription(type, architecture)
                    )),
                description
            ) { }

        public OrderedArchitectureCondition(
            Func<TRuleType, Architecture, ConditionResult> condition,
            string description
        )
            : base(
                (ruleTypes, architecture) =>
                    ruleTypes.Select(type => condition(type, architecture)),
                description
            ) { }

        public OrderedArchitectureCondition(
            Func<IEnumerable<TRuleType>, Architecture, IEnumerable<ConditionResult>> condition,
            string description
        )
            : base(condition, description) { }
    }
}
