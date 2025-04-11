namespace TypeDependencyNamespace;

public abstract class BaseClass { }

public class ChildClass : BaseClass { }

public class OtherChildClass : BaseClass { }

public abstract class BaseClassWithMember
{
    public string BaseClassMember { get; set; } = "";
}

public class ChildClassWithMember : BaseClassWithMember
{
    public string ChildClassMember { get; set; } = "";
}

public class OtherChildClassWithMember : BaseClassWithMember
{
    public string OtherChildClassMember { get; set; } = "";
}

public abstract class BaseClassWithMultipleDependencies { }

public class ChildClass1 : BaseClassWithMultipleDependencies { }

public class ChildClass2 : BaseClassWithMultipleDependencies { }

public class OtherBaseClass { }

public class ClassWithMultipleDependencies
{
    public BaseClassWithMember? _baseClass;
    public OtherBaseClass? _otherBaseClass;
}

public abstract class GenericBaseClass<TSelf>
    where TSelf : class { }

public class ChildClassOfGeneric : GenericBaseClass<ChildClassOfGeneric> { }

public class ClassWithoutDependencies { }

public class OtherClassWithoutDependencies { }

// https://github.com/TNG/ArchUnitNET/issues/351
class Issue351
{
    public void OuterFunc()
    {
        LocalFunc<string>();

        void LocalFunc<T>()
        {
            var list = new List<string>();
            list.GroupBy(x => x).ToDictionary(g => g.Key, g => (IReadOnlyCollection<T>)g.ToList());
        }
    }
}
