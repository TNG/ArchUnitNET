using System;

namespace TestAssembly
{
    public class ClassCallingOtherMethod
    {
        public void CallingOther(Class1 cls)
        {
            Console.Write(cls.ToString());
            cls.AccessClass2(1);
        }
    }
}
