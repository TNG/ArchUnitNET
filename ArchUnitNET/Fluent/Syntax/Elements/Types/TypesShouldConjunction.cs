using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public class TypesShouldConjunction : ObjectsShouldConjunction<TypesShould<TypesShouldConjunction, IType>, IType>
    {
        public TypesShouldConjunction(ArchRuleCreator<IType> ruleCreator) : base(ruleCreator)
        {
        }
    }
}