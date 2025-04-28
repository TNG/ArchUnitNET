using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public class GivenClassesThat
        : GivenTypesThat<GivenClassesConjunction, Class>,
            IClassPredicates<GivenClassesConjunction, Class>
    {
        public GivenClassesThat(IArchRuleCreator<Class> ruleCreator)
            : base(ruleCreator) { }

        public GivenClassesConjunction AreAbstract()
        {
            _ruleCreator.AddPredicate(ClassPredicatesDefinition.AreAbstract());
            return new GivenClassesConjunction(_ruleCreator);
        }

        public GivenClassesConjunction AreSealed()
        {
            _ruleCreator.AddPredicate(ClassPredicatesDefinition.AreSealed());
            return new GivenClassesConjunction(_ruleCreator);
        }

        public GivenClassesConjunction AreRecord()
        {
            _ruleCreator.AddPredicate(ClassPredicatesDefinition.AreRecord());
            return new GivenClassesConjunction(_ruleCreator);
        }

        public GivenClassesConjunction AreImmutable()
        {
            _ruleCreator.AddPredicate(ClassPredicatesDefinition.AreImmutable());
            return new GivenClassesConjunction(_ruleCreator);
        }

        //Negations

        public GivenClassesConjunction AreNotAbstract()
        {
            _ruleCreator.AddPredicate(ClassPredicatesDefinition.AreNotAbstract());
            return new GivenClassesConjunction(_ruleCreator);
        }

        public GivenClassesConjunction AreNotSealed()
        {
            _ruleCreator.AddPredicate(ClassPredicatesDefinition.AreNotSealed());
            return new GivenClassesConjunction(_ruleCreator);
        }

        public GivenClassesConjunction AreNotRecord()
        {
            _ruleCreator.AddPredicate(ClassPredicatesDefinition.AreNotRecord());
            return new GivenClassesConjunction(_ruleCreator);
        }

        public GivenClassesConjunction AreNotImmutable()
        {
            _ruleCreator.AddPredicate(ClassPredicatesDefinition.AreNotImmutable());
            return new GivenClassesConjunction(_ruleCreator);
        }
    }
}
