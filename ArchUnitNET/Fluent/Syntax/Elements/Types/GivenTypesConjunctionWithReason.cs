using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public class GivenTypesConjunctionWithReason :
        GivenObjectsConjunctionWithReason<GivenTypesThat<GivenTypesConjunction, IType>,
            TypesShould<TypesShouldConjunction, IType>, IType>
    {
        public GivenTypesConjunctionWithReason(IArchRuleCreator<IType> ruleCreator) : base(ruleCreator)
        {
        }
    }
}