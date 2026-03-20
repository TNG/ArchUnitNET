namespace TypeNamespace;

// Enums
public enum SimpleEnum
{
    Value1,
    Value2,
    Value3,
}

public enum OtherEnum
{
    A,
    B,
}

// Structs
public struct SimpleStruct
{
    public int Value;
}

public struct OtherStruct
{
    public string Name;
}

// Regular classes (not value types)
public class RegularClass { }

public class OtherRegularClass { }

// Member classes
public class ClassWithProperty
{
    public string PropertyA { get; set; } = "";
}

public class ClassWithField
{
    public int FieldA;
}

public class ClassWithMethod
{
    public void MethodA() { }
}

public class ClassWithAllMembers
{
    public string PropertyB { get; set; } = "";
    public int FieldB;

    public void MethodB() { }
}

public class ClassWithoutMembers { }

// Nested classes
public class OuterClassA
{
    public class InnerClassA { }

    public class OtherInnerClassA { }
}

public class OuterClassB
{
    public class InnerClassB { }
}

public class NonNestedClass { }

// Interfaces
public interface ITestInterface { }

public interface IOtherTestInterface { }

public interface IChildTestInterface : ITestInterface { }

public interface IOtherChildTestInterface : IOtherTestInterface { }

public interface IMultiParentTestInterface : ITestInterface, IOtherTestInterface { }

public interface IStandaloneTestInterface { }

// Interface implementation
public class ClassImplementingInterface : ITestInterface { }

public class ClassNotImplementingInterface { }

// Assignability
public class BaseClassForAssign { }

public class OtherBaseClassForAssign { }

public class DerivedClassForAssign : BaseClassForAssign { }

public class OtherDerivedClassForAssign : BaseClassForAssign { }

public class UnrelatedClassForAssign { }

public class OtherUnrelatedClassForAssign { }

// Member test classes
public class ClassWithStaticField
{
    public static int StaticField;
}

public class ClassWithNonStaticField
{
    public int NonStaticField;
}

public class ClassWithReadOnlyField
{
    public readonly string ReadOnlyField = "";
}

public class ClassWithWritableProperty
{
    public string WritableProperty { get; set; } = "";
}

public class OtherClassWithWritableProperty
{
    public string OtherWritableProperty { get; set; } = "";
}

public class ClassWithInitOnlyProperty
{
    public string InitOnlyProperty { get; init; } = "";
}

public class ClassWithGetOnlyProperty
{
    public string GetOnlyProperty { get; } = "";
}

public class OtherClassWithStaticField
{
    public static int OtherStaticField;
}

public class ClassWithStaticProperty
{
    public static string StaticProperty { get; set; } = "";
}

public class ClassWithStaticMethod
{
    public static void StaticMethod() { }
}
