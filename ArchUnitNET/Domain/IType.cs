using System.Collections.Generic;

namespace ArchUnitNET.Domain
{
    public interface IType : ICanBeAnalyzed
    {
        MemberList Members { get; }
        IEnumerable<IType> ImplementedInterfaces { get; }
        bool IsNested { get; }
        bool IsStub { get; }
        bool IsGenericParameter { get; }
    }
}
