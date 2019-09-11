﻿using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public class ClassesShould : TypesShould<ClassesShouldConjunction, Class>, IClassesShould
    {
        public ClassesShould(ArchRuleCreator<Class> ruleCreator) : base(ruleCreator)
        {
        }

        public ClassesShouldConjunction BeAbstract()
        {
            _ruleCreator.AddSimpleCondition(cls => cls.IsAbstract);
            return new ClassesShouldConjunction(_ruleCreator);
        }


        //Negations


        public ClassesShouldConjunction NotBeAbstract()
        {
            _ruleCreator.AddSimpleCondition(cls => !cls.IsAbstract);
            return new ClassesShouldConjunction(_ruleCreator);
        }
    }
}