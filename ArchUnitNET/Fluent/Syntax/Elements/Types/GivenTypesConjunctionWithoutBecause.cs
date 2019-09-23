using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public class GivenTypesConjunctionWithoutBecause : GivenObjectsConjunctionWithoutBecause<
        GivenTypesThat<GivenTypesConjunction, IType>,
        TypesShould<TypesShouldConjunction, IType>, IType>
    {
        public GivenTypesConjunctionWithoutBecause(IArchRuleCreator<IType> ruleCreator) : base(ruleCreator)
        {
        }
    }
}