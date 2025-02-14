using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public class ClassesShouldConjunctionWithDescription
        : ObjectsShouldConjunctionWithDescription<ClassesShould, Class>
    {
        public ClassesShouldConjunctionWithDescription(IArchRuleCreator<Class> ruleCreator)
            : base(ruleCreator) { }
    }
}
