using System.Collections.Generic;

namespace ArchUnitNET.Fluent.Freeze
{
    public class FrozenRule
    {
        public string ArchRuleDescription { get; set; }
        public List<string> Violations { get; set; }

        public FrozenRule(string archRuleDescription, List<string> violations)
        {
            ArchRuleDescription = archRuleDescription;
            Violations = violations;
        }
    }
}
