using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Freeze
{
    public interface IViolationStore
    {
        bool RuleAlreadyFrozen(IArchRule rule);
        IEnumerable<StringIdentifier> GetFrozenViolations(IArchRule rule);
        void StoreCurrentViolations(IArchRule rule, IEnumerable<StringIdentifier> violations);
    }
}
