//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Exceptions;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public abstract class GivenObjects<TRuleTypeThat, TRuleTypeShould, TRuleType> : SyntaxElement<TRuleType>,
        IObjectProvider<TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        protected GivenObjects(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public IEnumerable<TRuleType> GetObjects(Architecture architecture)
        {
            try
            {
                return architecture.GetOrCreateObjects(this, _ruleCreator.GetAnalyzedObjects);
            }
            catch (CannotGetObjectsOfCombinedArchRuleCreatorException exception)
            {
                throw new CannotGetObjectsOfCombinedArchRuleException(
                    "GetObjects() can't be used with CombinedArchRule \"" + ToString() +
                    "\" because the analyzed objects might be of different type. Try to use simple ArchRules instead.",
                    exception);
            }
        }

        public TRuleTypeThat That()
        {
            return Create<TRuleTypeThat, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShould Should()
        {
            return Create<TRuleTypeShould, TRuleType>(_ruleCreator);
        }
    }
}