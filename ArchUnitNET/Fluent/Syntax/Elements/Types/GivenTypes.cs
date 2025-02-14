using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public class GivenTypes
        : GivenObjects<
            GivenTypesThat<GivenTypesConjunction, IType>,
            TypesShould<TypesShouldConjunction, IType>,
            IType
        >
    {
        public GivenTypes(IArchRuleCreator<IType> ruleCreator)
            : base(ruleCreator) { }
    }
}
