using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using TypeDependencyNamespace;

namespace ArchUnitNETTests.AssemblyTestHelper;

public class TypeDependencyAssemblyTestHelper : AssemblyTestHelper
{
    public sealed override Architecture Architecture =>
        StaticTestArchitectures.TypeDependencyArchitecture;

    public readonly Class BaseClass;
    public readonly Type BaseClassSystemType = typeof(BaseClass);

    public readonly Class ChildClass;
    public readonly Type ChildClassSystemType = typeof(ChildClass);

    private readonly Class OtherChildClass;

    public readonly Class BaseClassWithMember;
    public readonly Type BaseClassWithMemberSystemType = typeof(BaseClassWithMember);

    private readonly Class ChildClassWithMember;

    private readonly Class OtherChildClassWithMember;

    private readonly Class BaseClassWithMultipleDependencies;
    public readonly Type BaseClassWithMultipleDependenciesSystemType =
        typeof(BaseClassWithMultipleDependencies);

    public readonly Class ChildClass1;

    public readonly Class ChildClass2;

    public readonly Class OtherBaseClass;
    public readonly Type OtherBaseClassSystemType = typeof(OtherBaseClass);

    public readonly Class ClassWithMultipleDependencies;

    public readonly Class ClassWithoutDependencies;
    public readonly Type ClassWithoutDependenciesSystemType = typeof(ClassWithoutDependencies);

    private readonly Class GenericBaseClass;

    private readonly Class ChildClassOfGeneric;

    private readonly Class OtherClassWithoutDependencies;

    public readonly Class ClassWithReferencedTypeDependency;

    public readonly Type ReferencedType = typeof(List<>);

    public TypeDependencyAssemblyTestHelper()
    {
        BaseClass = Architecture.GetClassOfType(typeof(BaseClass));
        ChildClass = Architecture.GetClassOfType(typeof(ChildClass));
        OtherChildClass = Architecture.GetClassOfType(typeof(OtherChildClass));
        BaseClassWithMember = Architecture.GetClassOfType(typeof(BaseClassWithMember));
        ChildClassWithMember = Architecture.GetClassOfType(typeof(ChildClassWithMember));
        OtherChildClassWithMember = Architecture.GetClassOfType(typeof(OtherChildClassWithMember));
        BaseClassWithMultipleDependencies = Architecture.GetClassOfType(
            typeof(BaseClassWithMultipleDependencies)
        );
        ChildClass1 = Architecture.GetClassOfType(typeof(ChildClass1));
        ChildClass2 = Architecture.GetClassOfType(typeof(ChildClass2));
        OtherBaseClass = Architecture.GetClassOfType(typeof(OtherBaseClass));
        ClassWithMultipleDependencies = Architecture.GetClassOfType(
            typeof(ClassWithMultipleDependencies)
        );
        GenericBaseClass = Architecture.GetClassOfType(typeof(GenericBaseClass<>));
        ChildClassOfGeneric = Architecture.GetClassOfType(typeof(ChildClassOfGeneric));
        ClassWithoutDependencies = Architecture.GetClassOfType(typeof(ClassWithoutDependencies));
        OtherClassWithoutDependencies = Architecture.GetClassOfType(
            typeof(OtherClassWithoutDependencies)
        );
        ClassWithReferencedTypeDependency = Architecture.GetClassOfType(
            typeof(ClassWithReferencedTypeDependency)
        );
    }
}
