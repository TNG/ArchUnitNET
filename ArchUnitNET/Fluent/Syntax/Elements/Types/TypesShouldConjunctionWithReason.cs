using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public class TypesShouldConjunctionWithReason : ObjectsShouldConjunctionWithReason<
        TypesShould<TypesShouldConjunction, IType>, IType>
    {
        public TypesShouldConjunctionWithReason(IArchRuleCreator<IType> ruleCreator) : base(ruleCreator)
        {
        }
    }
}