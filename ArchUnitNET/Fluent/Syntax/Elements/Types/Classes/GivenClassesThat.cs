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
            _ruleCreator.AddObjectFilter(ClassPredicatesDefinition.AreAbstract());
            return new GivenClassesConjunction(_ruleCreator);
        }

        public GivenClassesConjunction AreSealed()
        {
            _ruleCreator.AddObjectFilter(ClassPredicatesDefinition.AreSealed());
            return new GivenClassesConjunction(_ruleCreator);
        }

        public GivenClassesConjunction AreValueTypes()
        {
            _ruleCreator.AddObjectFilter(ClassPredicatesDefinition.AreValueTypes());
            return new GivenClassesConjunction(_ruleCreator);
        }

        public GivenClassesConjunction AreEnums()
        {
            _ruleCreator.AddObjectFilter(ClassPredicatesDefinition.AreEnums());
            return new GivenClassesConjunction(_ruleCreator);
        }

        public GivenClassesConjunction AreStructs()
        {
            _ruleCreator.AddObjectFilter(ClassPredicatesDefinition.AreStructs());
            return new GivenClassesConjunction(_ruleCreator);
        }


        //Negations


        public GivenClassesConjunction AreNotAbstract()
        {
            _ruleCreator.AddObjectFilter(ClassPredicatesDefinition.AreNotAbstract());
            return new GivenClassesConjunction(_ruleCreator);
        }

        public GivenClassesConjunction AreNotSealed()
        {
            _ruleCreator.AddObjectFilter(ClassPredicatesDefinition.AreNotSealed());
            return new GivenClassesConjunction(_ruleCreator);
        }

        public GivenClassesConjunction AreNotValueTypes()
        {
            _ruleCreator.AddObjectFilter(ClassPredicatesDefinition.AreNotValueTypes());
            return new GivenClassesConjunction(_ruleCreator);
        }

        public GivenClassesConjunction AreNotEnums()
        {
            _ruleCreator.AddObjectFilter(ClassPredicatesDefinition.AreNotEnums());
            return new GivenClassesConjunction(_ruleCreator);
        }

        public GivenClassesConjunction AreNotStructs()
        {
            _ruleCreator.AddObjectFilter(ClassPredicatesDefinition.AreNotStructs());
            return new GivenClassesConjunction(_ruleCreator);
        }
    }
}