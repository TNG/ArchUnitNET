using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public class GivenClassesConjunction
        : GivenObjectsConjunction<
            GivenClassesThat,
            ClassesShould,
            GivenClassesConjunctionWithDescription,
            Class
        >
    {
        public GivenClassesConjunction(IArchRuleCreator<Class> ruleCreator)
            : base(ruleCreator) { }
    }
}
