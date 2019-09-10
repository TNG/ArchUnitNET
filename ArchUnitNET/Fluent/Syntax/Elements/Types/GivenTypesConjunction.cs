using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public class GivenTypesConjunction : GivenObjectsConjunction<GivenTypesThat<GivenTypesConjunction, IType>,
        TypesShould<TypesShouldConjunction, IType>, IType>
    {
        public GivenTypesConjunction(ArchRuleCreator<IType> ruleCreator) : base(ruleCreator)
        {
        }
    }
}