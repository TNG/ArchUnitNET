using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public class TypesShouldConjunctionWithoutBecause : ObjectsShouldConjunctionWithoutBecause<
        TypesShould<TypesShouldConjunction, IType>, IType>
    {
        public TypesShouldConjunctionWithoutBecause(IArchRuleCreator<IType> ruleCreator) : base(ruleCreator)
        {
        }
    }
}