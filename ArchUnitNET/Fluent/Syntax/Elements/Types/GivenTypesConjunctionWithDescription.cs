using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public class GivenTypesConjunctionWithDescription
        : GivenObjectsConjunctionWithDescription<
            GivenTypesThat<GivenTypesConjunction, IType>,
            TypesShould<TypesShouldConjunction, IType>,
            IType
        >
    {
        public GivenTypesConjunctionWithDescription(IArchRuleCreator<IType> ruleCreator)
            : base(ruleCreator) { }
    }
}
