﻿using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public class GivenClasses : GivenObjects<GivenClassesThat, ClassesShould, Class>
    {
        public GivenClasses(ArchRuleCreator<Class> ruleCreator) : base(ruleCreator)
        {
        }
    }
}