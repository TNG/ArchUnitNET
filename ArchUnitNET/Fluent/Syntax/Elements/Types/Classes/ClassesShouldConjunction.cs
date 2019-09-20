using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public class
        ClassesShouldConjunction : ObjectsShouldConjunction<ClassesShould, ClassesShouldConjunctionWithoutBecause, Class
        >
    {
        public ClassesShouldConjunction(ArchRuleCreator<Class> ruleCreator) : base(ruleCreator)
        {
        }
    }
}