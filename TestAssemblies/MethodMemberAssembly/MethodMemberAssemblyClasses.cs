namespace MethodMemberNamespace;

public class RegularClass { }

public class OtherRegularClass { }

public class ClassWithVirtualMethod
{
    public virtual void VirtualMethod() { }
}

public class OtherClassWithVirtualMethod
{
    public virtual void OtherVirtualMethod() { }
}

public class ClassWithNonVirtualMethod
{
    public void NonVirtualMethod() { }
}

public class ClassWithStringReturnType
{
    public string MethodReturningString()
    {
        return "";
    }
}

public class ClassWithRegularClassReturnType
{
    public RegularClass MethodReturningRegularClass()
    {
        return new RegularClass();
    }
}

public class ClassWithOtherRegularClassReturnType
{
    public OtherRegularClass MethodReturningOtherRegularClass()
    {
        return new OtherRegularClass();
    }
}

public class GenericClass<T> { }

public class GenericClass<T1, T2> { }

public class ClassWithGenericReturnType
{
    public GenericClass<RegularClass> MethodReturningGenericClass()
    {
        return new GenericClass<RegularClass>();
    }

    public GenericClass<RegularClass, OtherRegularClass> MethodReturningTwoArgGenericClass()
    {
        return new GenericClass<RegularClass, OtherRegularClass>();
    }
}

public class MethodDependencyClass
{
    public void MethodWithSingleDependency()
    {
        CalledMethod();
    }

    public void CalledMethod() { }

    public void OtherMethodWithSingleDependency()
    {
        OtherCalledMethod();
    }

    public void OtherCalledMethod() { }

    public void MethodWithoutDependencies() { }
}

public class OtherCallingClass
{
    public void MethodCallingCalledMethod()
    {
        var dep = new MethodDependencyClass();
        dep.CalledMethod();
    }

    public void AnotherMethodCallingCalledMethod()
    {
        var dep = new MethodDependencyClass();
        dep.OtherCalledMethod();
    }
}
