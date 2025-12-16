using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public class TypesShouldConjunction
        : ObjectsShouldConjunction<TypesShould, TypesShouldConjunctionWithDescription, IType>
    {
        public TypesShouldConjunction(IArchRuleCreator<IType> ruleCreator)
            : base(ruleCreator) { }
    }
}
