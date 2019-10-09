using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public class GivenClassesThat : GivenTypesThat<GivenClassesConjunction, Class>,
        IClassPredicates<GivenClassesConjunction>
    {
        public GivenClassesThat(IArchRuleCreator<Class> ruleCreator) : base(ruleCreator)
        {
        }

        public GivenClassesConjunction AreAbstract()
        {
            _ruleCreator.AddObjectFilter(ClassesFilterDefinition.AreAbstract());
            return new GivenClassesConjunction(_ruleCreator);
        }

        public GivenClassesConjunction AreSealed()
        {
            _ruleCreator.AddObjectFilter(ClassesFilterDefinition.AreSealed());
            return new GivenClassesConjunction(_ruleCreator);
        }

        public GivenClassesConjunction AreValueTypes()
        {
            _ruleCreator.AddObjectFilter(ClassesFilterDefinition.AreValueTypes());
            return new GivenClassesConjunction(_ruleCreator);
        }

        public GivenClassesConjunction AreEnums()
        {
            _ruleCreator.AddObjectFilter(ClassesFilterDefinition.AreEnums());
            return new GivenClassesConjunction(_ruleCreator);
        }

        public GivenClassesConjunction AreStructs()
        {
            _ruleCreator.AddObjectFilter(ClassesFilterDefinition.AreStructs());
            return new GivenClassesConjunction(_ruleCreator);
        }


        //Negations


        public GivenClassesConjunction AreNotAbstract()
        {
            _ruleCreator.AddObjectFilter(ClassesFilterDefinition.AreNotAbstract());
            return new GivenClassesConjunction(_ruleCreator);
        }

        public GivenClassesConjunction AreNotSealed()
        {
            _ruleCreator.AddObjectFilter(ClassesFilterDefinition.AreNotSealed());
            return new GivenClassesConjunction(_ruleCreator);
        }

        public GivenClassesConjunction AreNotValueTypes()
        {
            _ruleCreator.AddObjectFilter(ClassesFilterDefinition.AreNotValueTypes());
            return new GivenClassesConjunction(_ruleCreator);
        }

        public GivenClassesConjunction AreNotEnums()
        {
            _ruleCreator.AddObjectFilter(ClassesFilterDefinition.AreNotEnums());
            return new GivenClassesConjunction(_ruleCreator);
        }

        public GivenClassesConjunction AreNotStructs()
        {
            _ruleCreator.AddObjectFilter(ClassesFilterDefinition.AreNotStructs());
            return new GivenClassesConjunction(_ruleCreator);
        }
    }
}