using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public class GivenClassesConjunction : GivenObjectsConjunction<GivenClassesThat, ClassesShould,
        GivenClassesConjunctionWithoutBecause, Class>
    {
        public GivenClassesConjunction(ArchRuleCreator<Class> ruleCreator) : base(ruleCreator)
        {
        }
    }
}