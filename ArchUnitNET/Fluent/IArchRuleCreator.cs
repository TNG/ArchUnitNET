using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public interface IArchRuleCreator
    {
        bool Check(Architecture architecture);
    }
}