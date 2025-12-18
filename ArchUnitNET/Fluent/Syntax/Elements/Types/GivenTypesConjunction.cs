using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public class GivenTypesConjunction
        : GivenObjectsConjunction<
            GivenTypesThat,
            TypesShould,
            GivenTypesConjunctionWithDescription,
            IType
        >
    {
        public GivenTypesConjunction(IArchRuleCreator<IType> ruleCreator)
            : base(ruleCreator) { }
    }
}
