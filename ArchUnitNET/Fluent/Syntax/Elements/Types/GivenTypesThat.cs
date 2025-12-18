using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;
using Assembly = System.Reflection.Assembly;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public sealed class GivenTypesThat : AddTypePredicate<GivenTypesConjunction, IType, IType>
    {
        public GivenTypesThat(IArchRuleCreator<IType> ruleCreator)
            : base(ruleCreator) { }

        protected override GivenTypesConjunction CreateNextElement(IPredicate<IType> predicate)
        {
            _ruleCreator.AddPredicate(predicate);
            return new GivenTypesConjunction(_ruleCreator);
        }
    }
}
