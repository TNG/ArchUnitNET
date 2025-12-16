using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public class GivenTypes : GivenObjects<GivenTypesThat, TypesShould, IType>
    {
        public GivenTypes(IArchRuleCreator<IType> ruleCreator)
            : base(ruleCreator) { }
    }
}
