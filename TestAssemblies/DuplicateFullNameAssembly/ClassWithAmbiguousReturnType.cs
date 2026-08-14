namespace DuplicateFullNameAssembly;

public class ClassWithAmbiguousReturnType
{
    public DuplicateClassAcrossAssemblies.DuplicateClass MethodWithAmbiguousReturnType()
    {
        return new DuplicateClassAcrossAssemblies.DuplicateClass();
    }
}
