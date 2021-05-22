using System;

namespace TestAssembly
{
    public abstract record AbstractRecord(string TestProperty1, string TestProperty2)
    {
        public AbstractRecord CopyWithNewProperty(string testProperty)
        {
            return this with { TestProperty2 = testProperty };
        }

        public void TestMethod()
        {
        }
    }
}