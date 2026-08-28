using System;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using VisibilityNamespace;
using Xunit;

namespace ArchUnitNETTests.AssemblyTestHelper;

public class VisibilityAssemblyTestHelper : AssemblyTestHelper
{
    public sealed override Architecture Architecture =>
        StaticTestArchitectures.VisibilityArchitecture;

    public readonly Class PublicClass;

    public readonly Class OtherPublicClass;

    public readonly Class InternalClass;

    public readonly Class OtherInternalClass;

    private readonly Class OuterClass;

    public readonly Class PublicInnerClass;

    private readonly Class OtherPublicInnerClass;

    public readonly Class InternalInnerClass;

    private readonly Class OtherInternalInnerClass;

    public readonly Class ProtectedInternalInnerClass;

    public readonly Class OtherProtectedInternalInnerClass;

    public readonly Class ProtectedInnerClass;

    public readonly Class OtherProtectedInnerClass;

    public readonly Class PrivateProtectedInnerClass;

    public readonly Class OtherPrivateProtectedInnerClass;

    public readonly Class PrivateInnerClass;

    public readonly Class OtherPrivateInnerClass;

    public VisibilityAssemblyTestHelper()
    {
        PublicClass = Architecture.GetClassOfType(typeof(PublicClass));
        OtherPublicClass = Architecture.GetClassOfType(typeof(OtherPublicClass));
        InternalClass = Architecture.Classes.WhereNameIs("InternalClass").First();
        OtherInternalClass = Architecture.Classes.WhereNameIs("OtherInternalClass").First();
        OuterClass = Architecture.GetClassOfType(typeof(OuterClass));
        PublicInnerClass = Architecture.GetClassOfType(typeof(OuterClass.PublicInnerClass));
        OtherPublicInnerClass = Architecture.GetClassOfType(
            typeof(OuterClass.OtherPublicInnerClass)
        );
        InternalInnerClass = Architecture.Classes.WhereNameIs("InternalInnerClass").First();
        OtherInternalInnerClass = Architecture
            .Classes.WhereNameIs("OtherInternalInnerClass")
            .First();
        ProtectedInternalInnerClass = Architecture
            .Classes.WhereNameIs("ProtectedInternalInnerClass")
            .First();
        OtherProtectedInternalInnerClass = Architecture
            .Classes.WhereNameIs("OtherProtectedInternalInnerClass")
            .First();
        ProtectedInnerClass = Architecture.Classes.WhereNameIs("ProtectedInnerClass").First();
        OtherProtectedInnerClass = Architecture
            .Classes.WhereNameIs("OtherProtectedInnerClass")
            .First();
        PrivateProtectedInnerClass = Architecture
            .Classes.WhereNameIs("PrivateProtectedInnerClass")
            .First();
        OtherPrivateProtectedInnerClass = Architecture
            .Classes.WhereNameIs("OtherPrivateProtectedInnerClass")
            .First();
        PrivateInnerClass = Architecture.Classes.WhereNameIs("PrivateInnerClass").First();
        OtherPrivateInnerClass = Architecture.Classes.WhereNameIs("OtherPrivateInnerClass").First();
    }
}
