// ReSharper disable UnusedVariable

// ReSharper disable UnusedMember.Global

namespace TestAssembly
{
    public class Class1
    {
        public Class1(string testProperty)
        {
            TestProperty = testProperty;
        }

        public string TestProperty { get; }

        public string AccessClass2(int intparam)
        {
            var class2 = new Class2();
            return "";
        }
    }
}
