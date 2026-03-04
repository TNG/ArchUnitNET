namespace LoaderTestAssembly;

public class LoaderTestAssembly { }

public class ClassWithDuplicateParameters
{
    public void MethodWithSameParameterType(string param1, string param2) { }

    public void MethodWithTripleDuplicate(int a, int b, int c) { }

    public void MethodWithMixedParamTypes(string s1, int i, string s2, bool b, string s3) { }

    public void MethodWithCustomTypeDuplicates(CustomType a, CustomType b) { }
}

public class CustomType { }
