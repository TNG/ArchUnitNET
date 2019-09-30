using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public class TypesShouldConjunction : ObjectsShouldConjunction<TypesShould<TypesShouldConjunction, IType>,
        TypesShouldConjunctionWithReason, IType>
    {
        public TypesShouldConjunction(IArchRuleCreator<IType> ruleCreator) : base(ruleCreator)
        {
        }
    }
}