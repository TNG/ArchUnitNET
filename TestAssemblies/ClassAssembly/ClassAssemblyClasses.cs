namespace ClassNamespace;

// Baseline (not abstract, not sealed, not a record)
public class RegularClass { }

public class OtherRegularClass { }

public abstract class AbstractClass { }

public abstract class OtherAbstractClass { }

public sealed class SealedClass { }

public sealed class OtherSealedClass { }

public record RecordClass { }

public record OtherRecordClass { }

public class ImmutableClass
{
    public readonly string Field = "";
    public string Property { get; } = "";
}

public class OtherImmutableClass
{
    public readonly string OtherField = "";
    public string OtherProperty { get; } = "";
}

public class MutableClass
{
    public string Property { get; set; } = "";
}

public class OtherMutableClass
{
    public string OtherProperty { get; set; } = "";
}

public class ClassWithoutMembers { }

public class ClassWithOnlyStaticMembers
{
    public static string StaticProperty { get; set; } = "";
}
