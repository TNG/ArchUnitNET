using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public class ClassesShould : TypesShould<ClassesShouldConjunction, Class>, IClassesShould
    {
        public ClassesShould(IArchRuleCreator<Class> ruleCreator) : base(ruleCreator)
        {
        }

        public ClassesShouldConjunction BeAbstract()
        {
            _ruleCreator.AddCondition(ClassesConditionDefinition.BeAbstract());
            return new ClassesShouldConjunction(_ruleCreator);
        }

        public ClassesShouldConjunction BeSealed()
        {
            _ruleCreator.AddCondition(ClassesConditionDefinition.BeSealed());
            return new ClassesShouldConjunction(_ruleCreator);
        }

        public ClassesShouldConjunction BeValueTypes()
        {
            _ruleCreator.AddCondition(ClassesConditionDefinition.BeValueTypes());
            return new ClassesShouldConjunction(_ruleCreator);
        }

        public ClassesShouldConjunction BeEnums()
        {
            _ruleCreator.AddCondition(ClassesConditionDefinition.BeEnums());
            return new ClassesShouldConjunction(_ruleCreator);
        }

        public ClassesShouldConjunction BeStructs()
        {
            _ruleCreator.AddCondition(ClassesConditionDefinition.BeStructs());
            return new ClassesShouldConjunction(_ruleCreator);
        }


        //Negations


        public ClassesShouldConjunction NotBeAbstract()
        {
            _ruleCreator.AddCondition(ClassesConditionDefinition.NotBeAbstract());
            return new ClassesShouldConjunction(_ruleCreator);
        }

        public ClassesShouldConjunction NotBeSealed()
        {
            _ruleCreator.AddCondition(ClassesConditionDefinition.NotBeSealed());
            return new ClassesShouldConjunction(_ruleCreator);
        }

        public ClassesShouldConjunction NotBeValueTypes()
        {
            _ruleCreator.AddCondition(ClassesConditionDefinition.NotBeValueTypes());
            return new ClassesShouldConjunction(_ruleCreator);
        }

        public ClassesShouldConjunction NotBeEnums()
        {
            _ruleCreator.AddCondition(ClassesConditionDefinition.NotBeEnums());
            return new ClassesShouldConjunction(_ruleCreator);
        }

        public ClassesShouldConjunction NotBeStructs()
        {
            _ruleCreator.AddCondition(ClassesConditionDefinition.NotBeStructs());
            return new ClassesShouldConjunction(_ruleCreator);
        }
    }
}