namespace ArchUnitNET.Fluent.Slices
{
    /// <summary>
    ///     Entry point for defining slice-based architecture rules. Use <see cref="Slices"/> to
    ///     begin the fluent rule definition chain.
    /// </summary>
    /// <example>
    ///     <code>
    ///     using static ArchUnitNET.Fluent.Slices.SliceRuleDefinition;
    ///
    ///     IArchRule rule = Slices().Matching("Module.(*)").Should().BeFreeOfCycles();
    ///     </code>
    /// </example>
    public static class SliceRuleDefinition
    {
        /// <summary>
        ///     Begins a new slice rule definition. Continue with <c>.Matching(pattern)</c> or
        ///     <c>.MatchingWithPackages(pattern)</c> to specify how types are assigned to slices.
        /// </summary>
        /// <returns>
        ///     A <see cref="SliceRuleInitializer"/> that provides methods to define the slice
        ///     assignment pattern.
        /// </returns>
        public static SliceRuleInitializer Slices()
        {
            var ruleCreator = new SliceRuleCreator();
            return new SliceRuleInitializer(ruleCreator);
        }
    }
}
