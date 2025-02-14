namespace ArchUnitNET.Fluent.Slices
{
    public static class SliceRuleDefinition
    {
        public static SliceRuleInitializer Slices()
        {
            var ruleCreator = new SliceRuleCreator();
            return new SliceRuleInitializer(ruleCreator);
        }
    }
}
