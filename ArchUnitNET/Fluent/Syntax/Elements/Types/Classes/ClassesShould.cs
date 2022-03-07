//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public class ClassesShould : TypesShould<ClassesShouldConjunction, Class>, IComplexClassConditions
    {
        public ClassesShould(IArchRuleCreator<Class> ruleCreator) : base(ruleCreator)
        {
        }

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
    }
}