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

    public Class BaseClass;
    public Type BaseClassSystemType = typeof(BaseClass);

    public Class ChildClass;
    public Type ChildClassSystemType = typeof(ChildClass);

    public Class OtherChildClass;
    public Type OtherChildClassSystemType = typeof(OtherChildClass);

    public Class BaseClassWithMember;
    public Type BaseClassWithMemberSystemType = typeof(BaseClassWithMember);

    public Class ChildClassWithMember;
    public Type ChildClassWithMemberSystemType = typeof(ChildClassWithMember);

    public Class OtherChildClassWithMember;
    public Type OtherChildClassWithMemberSystemType = typeof(OtherChildClassWithMember);

    public Class BaseClassWithMultipleDependencies;
    public Type BaseClassWithMultipleDependenciesSystemType =
        typeof(BaseClassWithMultipleDependencies);

    public Class ChildClass1;
    public Type ChildClass1SystemType = typeof(ChildClass1);

    public Class ChildClass2;
    public Type ChildClass2SystemType = typeof(ChildClass2);

    public Class OtherBaseClass;
    public Type OtherBaseClassSystemType = typeof(OtherBaseClass);

    public Class ClassWithMultipleDependencies;
    public Type ClassWithMultipleDependenciesSystemType = typeof(ClassWithMultipleDependencies);

    public Class ClassWithoutDependencies;
    public Type ClassWithoutDependenciesSystemType = typeof(ClassWithoutDependencies);

    public Class GenericBaseClass;
    public Type GenericBaseClassSystemType = typeof(GenericBaseClass<>);

    public Class ChildClassOfGeneric;
    public Type ChildClassOfGenericSystemType = typeof(ChildClassOfGeneric);

    public Class OtherClassWithoutDependencies;
    public Type OtherClassWithoutDependenciesSystemType = typeof(OtherClassWithoutDependencies);

    public Class ClassWithReferencedTypeDependency;
    public Type ClassWithReferencedTypeDependencySystemType =
        typeof(ClassWithReferencedTypeDependency);

    public Type ReferencedType = typeof(List<>);

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
