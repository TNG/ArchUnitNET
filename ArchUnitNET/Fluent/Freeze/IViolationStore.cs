using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Freeze
{
    public interface IViolationStore
    {
        bool RuleAlreadyFrozen(IArchRule rule);
        IEnumerable<FrozenRuleIdentifier> GetFrozenViolations(IArchRule rule);
        void StoreCurrentViolations(IArchRule rule, IEnumerable<FrozenRuleIdentifier> violations);
    }
}
