using System;
using System.Collections.Generic;
using System.IO;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;
using Assembly = System.Reflection.Assembly;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public sealed class TypesShould : AddTypeCondition<TypesShouldConjunction, IType>
    {
        public TypesShould(IArchRuleCreator<IType> ruleCreator)
            : base(ruleCreator) { }

        protected override TypesShouldConjunction CreateNextElement(
            IOrderedCondition<IType> condition
        )
        {
            _ruleCreator.AddCondition(condition);
            return new TypesShouldConjunction(_ruleCreator);
        }
    }
}
