using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public class
        GivenClassesConjunctionWithoutBecause : GivenObjectsConjunctionWithoutBecause<GivenClassesThat, ClassesShould,
            Class>
    {
        public GivenClassesConjunctionWithoutBecause(ArchRuleCreator<Class> ruleCreator) : base(ruleCreator)
        {
        }
    }
}