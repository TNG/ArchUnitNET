using System;

namespace TestAssembly
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class AssemblyTestAttribute : Attribute
    {
        private string _testValue;

        public AssemblyTestAttribute(string testValue)
        {
            _testValue = testValue;
        }
    }
}
