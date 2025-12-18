using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public class GivenTypesConjunctionWithDescription
        : GivenObjectsConjunctionWithDescription<GivenTypesThat, TypesShould, IType>
    {
        public GivenTypesConjunctionWithDescription(IArchRuleCreator<IType> ruleCreator)
            : base(ruleCreator) { }
    }
}
