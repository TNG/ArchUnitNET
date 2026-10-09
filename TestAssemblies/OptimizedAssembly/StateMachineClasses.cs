namespace OptimizedAssembly;

public class CalledClass
{
    public static void CalledMethod() { }
}

public class ClassWithAsyncMethod
{
    public async Task AsyncMethod()
    {
        await Task.Yield();
        CalledClass.CalledMethod();
    }
}

public class ClassWithIteratorMethod
{
    public IEnumerable<int> IteratorMethod()
    {
        CalledClass.CalledMethod();
        yield return 0;
    }
}
