using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public class ClassesShould
        : TypesShould<ClassesShouldConjunction, Class>,
            IComplexClassConditions
    {
        public ClassesShould(IArchRuleCreator<Class> ruleCreator)
            : base(ruleCreator) { }

        public ClassesShouldConjunction BeAbstract()
        {
            _ruleCreator.AddCondition(ClassConditionsDefinition.BeAbstract());
            return new ClassesShouldConjunction(_ruleCreator);
        }

        public ClassesShouldConjunction BeSealed()
        {
            _ruleCreator.AddCondition(ClassConditionsDefinition.BeSealed());
            return new ClassesShouldConjunction(_ruleCreator);
        }

        public ClassesShouldConjunction BeRecord()
        {
            _ruleCreator.AddCondition(ClassConditionsDefinition.BeRecord());
            return new ClassesShouldConjunction(_ruleCreator);
        }

        public ClassesShouldConjunction BeImmutable()
        {
            _ruleCreator.AddCondition(ClassConditionsDefinition.BeImmutable());
            return new ClassesShouldConjunction(_ruleCreator);
        }

        //Negations

        public ClassesShouldConjunction NotBeAbstract()
        {
            _ruleCreator.AddCondition(ClassConditionsDefinition.NotBeAbstract());
            return new ClassesShouldConjunction(_ruleCreator);
        }

        public ClassesShouldConjunction NotBeSealed()
        {
            _ruleCreator.AddCondition(ClassConditionsDefinition.NotBeSealed());
            return new ClassesShouldConjunction(_ruleCreator);
        }

        public ClassesShouldConjunction NotBeRecord()
        {
            _ruleCreator.AddCondition(ClassConditionsDefinition.NotBeRecord());
            return new ClassesShouldConjunction(_ruleCreator);
        }

        public ClassesShouldConjunction NotBeImmutable()
        {
            _ruleCreator.AddCondition(ClassConditionsDefinition.NotBeImmutable());
            return new ClassesShouldConjunction(_ruleCreator);
        }
    }
}
