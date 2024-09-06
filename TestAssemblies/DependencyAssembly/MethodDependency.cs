namespace MethodDependencyNamespace;

public class MethodDependencyClass
{
    public void MethodWithSingleDependency()
    {
        CalledMethod();
    }

    public void CalledMethod() { }

    public void MethodWithMultipleDependencies()
    {
        CalledMethod1();
        CalledMethod2();
        CalledMethod3();
    }

    public void CalledMethod1() { }

    public void CalledMethod2() { }

    public void CalledMethod3() { }

    public void MethodWithoutDependencies() { }
}
