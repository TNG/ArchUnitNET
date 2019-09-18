using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public interface IArchRuleCreator : IHasDescription
    {
        bool Check(Architecture architecture);
        IEnumerable<EvaluationResult> Evaluate(Architecture architecture);
    }
}