using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public class TypesShouldConjunctionWithoutBecause : ObjectsShouldConjunctionWithoutBecause<
        TypesShould<TypesShouldConjunction, IType>, IType>
    {
        public TypesShouldConjunctionWithoutBecause(ArchRuleCreator<IType> ruleCreator) : base(ruleCreator)
        {
        }
    }
}