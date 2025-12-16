using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public class TypesShouldConjunctionWithDescription
        : ObjectsShouldConjunctionWithDescription<TypesShould, IType>
    {
        public TypesShouldConjunctionWithDescription(IArchRuleCreator<IType> ruleCreator)
            : base(ruleCreator) { }
    }
}
