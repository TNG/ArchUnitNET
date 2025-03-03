namespace VisibilityNamespace;

public class PublicClass { }

public class OtherPublicClass { }

internal class InternalClass { }

internal class OtherInternalClass { }

public class OuterClass
{
    public class PublicInnerClass { }

    public class OtherPublicInnerClass { }

    internal class InternalInnerClass { }

    internal class OtherInternalInnerClass { }

    protected internal class ProtectedInternalInnerClass { }

    protected internal class OtherProtectedInternalInnerClass { }

    protected class ProtectedInnerClass { }

    protected class OtherProtectedInnerClass { }

    private protected class PrivateProtectedInnerClass { }

    private protected class OtherPrivateProtectedInnerClass { }

    private class PrivateInnerClass { }

    private class OtherPrivateInnerClass { }
}
