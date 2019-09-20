﻿using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public class GivenClassesThat : GivenTypesThat<GivenClassesConjunction, Class>,
        IClassesThat<GivenClassesConjunction>
    {
        public GivenClassesThat(ArchRuleCreator<Class> ruleCreator) : base(ruleCreator)
        {
        }

        public GivenClassesConjunction AreAbstract()
        {
            _ruleCreator.AddObjectFilter(ClassesFilterDefinition.AreAbstract());
            return new GivenClassesConjunction(_ruleCreator);
        }


        //Negations


        public GivenClassesConjunction AreNotAbstract()
        {
            _ruleCreator.AddObjectFilter(ClassesFilterDefinition.AreNotAbstract());
            return new GivenClassesConjunction(_ruleCreator);
        }
    }
}