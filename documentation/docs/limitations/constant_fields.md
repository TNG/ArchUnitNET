#Constant Fields

```
class ClassWithStaticField
{
    public const string ConstField = "ConstField";
}

class ClassAccessingField
    {
        public void MethodAccessingConstField()
        {
            var a = ClassWithStaticField.ConstField;
        }
}
```

Looking at the above example one might expect, that MethodAccessingConstField 
should have a type dependency to ClassWithStaticField. In fact the compiler only generates a Ldstr opcode
indicating that a constant string is stored in var a. It therefore is not possible to find out from where the constant field came.

````
public void MethodAccessToConstFieldFound()
{
    var method = Architecture.GetClassOfType(typeof(ClassAccessingField)).GetMethodMembers()
        .First(member => member.FullNameContains("MethodAccessingConstField"));
    var methodTypeDependencies = method.GetTypeDependencies().ToList();
    Assert.Contains(Architecture.GetClassOfType(typeof(ClassWithStaticField)), methodTypeDependencies);
}
````
This test for example would fail, because no methodTypeDependency is created.