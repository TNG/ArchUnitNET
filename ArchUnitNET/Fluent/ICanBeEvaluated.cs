using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public interface ICanBeEvaluated : IHasDescription
    {
        bool HasViolations(Architecture architecture);
        IEnumerable<EvaluationResult> Evaluate(Architecture architecture);
    }
}