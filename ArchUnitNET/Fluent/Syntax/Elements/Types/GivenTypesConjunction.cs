using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public class GivenTypesConjunction :
        GivenObjectsConjunction<GivenTypesThat<GivenTypesConjunction, IType>, TypesShould<TypesShouldConjunction, IType>
            , GivenTypesConjunctionWithReason, IType>
    {
        public GivenTypesConjunction(IArchRuleCreator<IType> ruleCreator) : base(ruleCreator)
        {
        }
    }
}