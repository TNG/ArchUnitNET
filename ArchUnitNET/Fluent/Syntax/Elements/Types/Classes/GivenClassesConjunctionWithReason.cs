using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public class
        GivenClassesConjunctionWithReason : GivenObjectsConjunctionWithReason<GivenClassesThat, ClassesShould,
            Class>
    {
        public GivenClassesConjunctionWithReason(IArchRuleCreator<Class> ruleCreator) : base(ruleCreator)
        {
        }
    }
}