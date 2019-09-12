namespace ArchUnitNET.Fluent
{
    public interface IArchRule : IArchRuleCreator
    {
        CombinedArchRuleDefinition And();
        CombinedArchRuleDefinition Or();
        IArchRule And(IArchRule archRule);
        IArchRule Or(IArchRule archRule);
    }
}