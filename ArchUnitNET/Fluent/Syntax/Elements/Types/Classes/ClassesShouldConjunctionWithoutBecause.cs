using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public class ClassesShouldConjunctionWithoutBecause : ObjectsShouldConjunctionWithoutBecause<ClassesShould, Class>
    {
        public ClassesShouldConjunctionWithoutBecause(IArchRuleCreator<Class> ruleCreator) : base(ruleCreator)
        {
        }
    }
}