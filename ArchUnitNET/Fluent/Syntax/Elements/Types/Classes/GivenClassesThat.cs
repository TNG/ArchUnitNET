﻿//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public class GivenClassesThat : GivenTypesThat<GivenClassesConjunction, Class>,
        IClassPredicates<GivenClassesConjunction, Class>
    {
        public GivenClassesThat(IArchRuleCreator<Class> ruleCreator) : base(ruleCreator)
        {
        }

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

        public GivenClassesConjunction AreValueTypes()
        {
            _ruleCreator.AddPredicate(ClassPredicatesDefinition.AreValueTypes());
            return new GivenClassesConjunction(_ruleCreator);
        }

        public GivenClassesConjunction AreEnums()
        {
            _ruleCreator.AddPredicate(ClassPredicatesDefinition.AreEnums());
            return new GivenClassesConjunction(_ruleCreator);
        }

        public GivenClassesConjunction AreStructs()
        {
            _ruleCreator.AddPredicate(ClassPredicatesDefinition.AreStructs());
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

        public GivenClassesConjunction AreNotValueTypes()
        {
            _ruleCreator.AddPredicate(ClassPredicatesDefinition.AreNotValueTypes());
            return new GivenClassesConjunction(_ruleCreator);
        }

        public GivenClassesConjunction AreNotEnums()
        {
            _ruleCreator.AddPredicate(ClassPredicatesDefinition.AreNotEnums());
            return new GivenClassesConjunction(_ruleCreator);
        }

        public GivenClassesConjunction AreNotStructs()
        {
            _ruleCreator.AddPredicate(ClassPredicatesDefinition.AreNotStructs());
            return new GivenClassesConjunction(_ruleCreator);
        }
    }
}