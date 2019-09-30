using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Exceptions;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public abstract class
        GivenObjectsConjunctionWithoutBecause<TGivenRuleTypeThat, TRuleTypeShould, TRuleType> :
            SyntaxElement<TRuleType>, IObjectProvider<TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        protected GivenObjectsConjunctionWithoutBecause(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public IEnumerable<TRuleType> GetObjects(Architecture architecture)
        {
            try
            {
                return _ruleCreator.GetFilteredObjects(architecture);
            }
            catch (CannotGetObjectsOfCombinedArchRuleCreatorException exception)
            {
                throw new CannotGetObjectsOfCombinedArchRuleException(
                    "GetObjects() can't be used with CombinedArchRule \"" + ToString() +
                    "\" because the analyzed objects might be of different type. Try to use simple ArchRules instead.",
                    exception);
            }
        }

        public TGivenRuleTypeThat And()
        {
            _ruleCreator.AddObjectFilterConjunction(LogicalConjunctionDefinition.And);
            return Create<TGivenRuleTypeThat, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeThat Or()
        {
            _ruleCreator.AddObjectFilterConjunction(LogicalConjunctionDefinition.Or);
            return Create<TGivenRuleTypeThat, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShould Should()
        {
            return Create<TRuleTypeShould, TRuleType>(_ruleCreator);
        }
    }
}