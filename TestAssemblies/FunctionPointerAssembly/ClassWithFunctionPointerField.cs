namespace FunctionPointerNamespace;

// The field's type is a function pointer, which the loader resolves to a referenced type
// without a namespace or an assembly.
public unsafe class ClassWithFunctionPointerField
{
    public delegate* <int, void> FunctionPointerField;
}
