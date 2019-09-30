using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Extensions;

// ReSharper disable once CheckNamespace
namespace Xunit.Sdk
{
    public class FailedArchRuleException : XunitException
    {
        public FailedArchRuleException(Architecture architecture, IArchRule archRule)
            : this(architecture.EvaluateRule(archRule))
        {
        }

        public FailedArchRuleException(IEnumerable<EvaluationResult> evaluationResults)
            : base(evaluationResults.ToErrorMessage())
        {
        }
    }
}