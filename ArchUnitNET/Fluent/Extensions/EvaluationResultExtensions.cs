using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArchUnitNET.Domain.Extensions;

namespace ArchUnitNET.Fluent.Extensions
{
    public static class EvaluationResultExtensions
    {
        public static string ToErrorMessage(this IEnumerable<EvaluationResult> evaluationResults)
        {
            var failedResults = evaluationResults.Where(result => !result.Passed).ToList();
            if (failedResults.IsNullOrEmpty())
            {
                return "All Evaluations passed";
            }

            var analyzedRules = failedResults.Select(result => result.ArchRule).Distinct();
            var errorMessage = new StringBuilder();
            foreach (var rule in analyzedRules)
            {
                errorMessage.AppendLine("\"" + rule.Description + "\" failed:");
                foreach (var result in failedResults.Where(result => result.ArchRule.Equals(rule)))
                {
                    errorMessage.AppendLine("\t" + result.Description);
                }

                errorMessage.AppendLine();
            }

            return errorMessage.ToString();
        }
    }
}
