using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public class EvaluationResult //TODO change type to readonly struct and change c# version to 7.2?
    {
        public EvaluationResult(ICanBeAnalyzed obj, bool pass, string description, string archRuleDescription)
        {
            Object = obj;
            Pass = pass;
            Description = description;
            ArchRuleDescription = archRuleDescription;
        }

        public ICanBeAnalyzed Object { get; }
        public bool Pass { get; }
        public string Description { get; }
        public string ArchRuleDescription { get; }

        public override string ToString()
        {
            return Description;
        }
    }
}