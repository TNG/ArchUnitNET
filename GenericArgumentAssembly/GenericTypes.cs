namespace GenericArgumentAssembly;

public class NonGenericClass
{
    public void NonGenericMethod() { }
    
    public void GenericMethod<T>(List<T> arg) { }
}

public class GenericClass<T>
{
    public void NonGenericMethod() { }
    
    public void GenericMethod(List<T> arg) { }
    
    public void AnotherGenericMethod<U>(List<T> arg1, List<U> arg2) { }
}

public class GenericClassWithMultipleParameters<T, U>
{
    public void NonGenericMethod() { }
    
    public void GenericMethod(List<T> arg1, List<U> arg2) { }
    
    public void AnotherGenericMethod<V>(List<T> arg1, List<U> arg2, List<V> arg3) { }
}
