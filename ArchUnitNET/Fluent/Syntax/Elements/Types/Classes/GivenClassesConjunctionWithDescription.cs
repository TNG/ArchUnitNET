using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public class
        GivenClassesConjunctionWithDescription : GivenObjectsConjunctionWithDescription<GivenClassesThat, ClassesShould,
            Class>
    {
        public GivenClassesConjunctionWithDescription(IArchRuleCreator<Class> ruleCreator) : base(ruleCreator)
        {
        }
    }
}