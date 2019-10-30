//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.Fluent.Predicates;

namespace ArchUnitNET.Fluent
{
    public interface IArchRuleCreator<TRuleType> : ICanBeEvaluated where TRuleType : ICanBeAnalyzed
    {
        void AddPredicate(IPredicate<TRuleType> predicate);
        void AddPredicateConjunction(LogicalConjunction logicalConjunction);
        void AddCondition(ICondition<TRuleType> condition);
        void AddConditionConjunction(LogicalConjunction logicalConjunction);
        void AddConditionReason(string reason);
        void AddPredicateReason(string reason);

        void BeginComplexCondition<TReferenceType>(RelationCondition<TRuleType, TReferenceType> relationCondition)
            where TReferenceType : ICanBeAnalyzed;

        void ContinueComplexCondition<TReferenceType>(IPredicate<TReferenceType> predicate)
            where TReferenceType : ICanBeAnalyzed;

        IEnumerable<TRuleType> GetAnalyzedObjects(Architecture architecture);

        void SetCustomPredicateDescription(string description);
        void SetCustomConditionDescription(string description);
    }
}