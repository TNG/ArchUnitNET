namespace TestAssembly.Domain.Methods;

public class ClassWithPrivateParameterlessConstructor
{
    // Private parameterless constructor
    private ClassWithPrivateParameterlessConstructor()
    {
    }

    // Public constructor with parameters
    public ClassWithPrivateParameterlessConstructor(string value)
    {
        Value = value;
    }

    public string Value { get; set; }
}

public class ClassWithPublicParameterlessConstructor
{
    // Public parameterless constructor
    public ClassWithPublicParameterlessConstructor()
    {
    }

    // Public constructor with parameters
    public ClassWithPublicParameterlessConstructor(int number)
    {
        Number = number;
    }

    public int Number { get; set; }
}

public class ClassWithOnlyParameterizedConstructors
{
    // Only constructors with parameters
    public ClassWithOnlyParameterizedConstructors(string name)
    {
        Name = name;
    }

    public ClassWithOnlyParameterizedConstructors(string name, int id)
    {
        Name = name;
        Id = id;
    }

    public string Name { get; set; }
    public int Id { get; set; }
}

public class ClassWithMethods
{
    public ClassWithMethods()
    {
    }

    // Method without parameters
    public void MethodWithoutParameters()
    {
    }

    // Method with parameters
    public void MethodWithParameters(string input, int count)
    {
    }

    // Private method without parameters
    private void PrivateMethodWithoutParameters()
    {
    }
}