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

    public Class PublicClass;
    public Type PublicSystemType = typeof(PublicClass);

    public Class OtherPublicClass;
    public Type OtherPublicClassSystemType = typeof(OtherPublicClass);

    public Class InternalClass;

    public Class OtherInternalClass;

    public Class OuterClass;
    public Type OuterSystemType = typeof(OuterClass);

    public Class PublicInnerClass;
    public Type PublicInnerSystemType = typeof(OuterClass.PublicInnerClass);

    public Class OtherPublicInnerClass;
    public Type OtherPublicInnerSystemType = typeof(OuterClass.OtherPublicInnerClass);

    public Class InternalInnerClass;

    public Class OtherInternalInnerClass;

    public Class ProtectedInternalInnerClass;

    public Class OtherProtectedInternalInnerClass;

    public Class ProtectedInnerClass;

    public Class OtherProtectedInnerClass;

    public Class PrivateProtectedInnerClass;

    public Class OtherPrivateProtectedInnerClass;

    public Class PrivateInnerClass;

    public Class OtherPrivateInnerClass;

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
