namespace ArchUnitNET.Fluent
{
    public interface IArchRule : ICanBeEvaluated
    {
        CombinedArchRuleDefinition And();
        CombinedArchRuleDefinition Or();
        IArchRule And(IArchRule archRule);
        IArchRule Or(IArchRule archRule);
    }
}
