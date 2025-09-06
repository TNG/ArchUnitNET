namespace TestAssembly.Domain.Entities;

public class ClassWithPrivateParameterlessConstructor
{
    private ClassWithPrivateParameterlessConstructor()
    {
        // Private parameterless constructor for ORM
    }

    public ClassWithPrivateParameterlessConstructor(string name)
    {
        Name = name;
    }

    public string Name { get; private set; }
}

public class ClassWithoutPrivateParameterlessConstructor
{
    public ClassWithoutPrivateParameterlessConstructor()
    {
        // Public parameterless constructor
    }

    public ClassWithoutPrivateParameterlessConstructor(string name)
    {
        Name = name;
    }

    public string Name { get; private set; }
}

public class ClassWithOnlyParameterizedConstructors
{
    public ClassWithOnlyParameterizedConstructors(string name)
    {
        Name = name;
    }

    public ClassWithOnlyParameterizedConstructors(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
}

public abstract class AbstractClassBase
{
    protected AbstractClassBase()
    {
        // Abstract classes should be excluded from checks
    }
}