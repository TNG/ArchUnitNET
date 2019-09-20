using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public class TypesShouldConjunction : ObjectsShouldConjunction<TypesShould<TypesShouldConjunction, IType>,
        TypesShouldConjunctionWithoutBecause, IType>
    {
        public TypesShouldConjunction(ArchRuleCreator<IType> ruleCreator) : base(ruleCreator)
        {
        }
    }
}