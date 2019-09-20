using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;
using static ArchUnitNET.Domain.Visibility;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public static class ObjectsFilterDefinition<T> where T : ICanBeAnalyzed
    {
        public static ObjectFilter<T> Are(ICanBeAnalyzed obj)
        {
            return new ObjectFilter<T>(o => o.Equals(obj), "are \"" + obj.FullName + "\"");
        }

        public static ObjectFilter<T> DependOn(string pattern)
        {
            return new ObjectFilter<T>(obj => obj.DependsOn(pattern), "depend on \"" + pattern + "\"");
        }

        public static ObjectFilter<T> HaveName(string name)
        {
            return new ObjectFilter<T>(obj => obj.Name.Equals(name), "have name \"" + name + "\"");
        }

        public static ObjectFilter<T> HaveFullName(string fullname)
        {
            return new ObjectFilter<T>(obj => obj.FullName.Equals(fullname), "have full name \"" + fullname + "\"");
        }

        public static ObjectFilter<T> HaveNameStartingWith(string pattern)
        {
            return new ObjectFilter<T>(obj => obj.NameStartsWith(pattern),
                "have name starting with \"" + pattern + "\"");
        }

        public static ObjectFilter<T> HaveNameEndingWith(string pattern)
        {
            return new ObjectFilter<T>(obj => obj.NameEndsWith(pattern), "have name ending with \"" + pattern + "\"");
        }

        public static ObjectFilter<T> HaveNameContaining(string pattern)
        {
            return new ObjectFilter<T>(obj => obj.NameContains(pattern), "have name containing \"" + pattern + "\"");
        }

        public static ObjectFilter<T> ArePrivate()
        {
            return new ObjectFilter<T>(obj => obj.Visibility == Private, "are private");
        }

        public static ObjectFilter<T> ArePublic()
        {
            return new ObjectFilter<T>(obj => obj.Visibility == Public, "are public");
        }

        public static ObjectFilter<T> AreProtected()
        {
            return new ObjectFilter<T>(obj => obj.Visibility == Protected, "are protected");
        }

        public static ObjectFilter<T> AreInternal()
        {
            return new ObjectFilter<T>(obj => obj.Visibility == Internal, "are internal");
        }

        public static ObjectFilter<T> AreProtectedInternal()
        {
            return new ObjectFilter<T>(obj => obj.Visibility == ProtectedInternal, "are protected internal");
        }

        public static ObjectFilter<T> ArePrivateProtected()
        {
            return new ObjectFilter<T>(obj => obj.Visibility == PrivateProtected, "are private protected");
        }


        //Negations


        public static ObjectFilter<T> AreNot(ICanBeAnalyzed obj)
        {
            return new ObjectFilter<T>(o => !o.Equals(obj), "are not \"" + obj.FullName + "\"");
        }

        public static ObjectFilter<T> DoNotDependOn(string pattern)
        {
            return new ObjectFilter<T>(obj => !obj.DependsOn(pattern), "do not depend on \"" + pattern + "\"");
        }

        public static ObjectFilter<T> DoNotHaveName(string name)
        {
            return new ObjectFilter<T>(obj => !obj.Name.Equals(name), "do not have name \"" + name + "\"");
        }

        public static ObjectFilter<T> DoNotHaveFullName(string fullname)
        {
            return new ObjectFilter<T>(obj => !obj.FullName.Equals(fullname),
                "do not have full name \"" + fullname + "\"");
        }

        public static ObjectFilter<T> DoNotHaveNameStartingWith(string pattern)
        {
            return new ObjectFilter<T>(obj => !obj.NameStartsWith(pattern),
                "do not have name starting with \"" + pattern + "\"");
        }

        public static ObjectFilter<T> DoNotHaveNameEndingWith(string pattern)
        {
            return new ObjectFilter<T>(obj => !obj.NameEndsWith(pattern),
                "do not have name ending with \"" + pattern + "\"");
        }

        public static ObjectFilter<T> DoNotHaveNameContaining(string pattern)
        {
            return new ObjectFilter<T>(obj => !obj.NameContains(pattern),
                "do not have name containing \"" + pattern + "\"");
        }

        public static ObjectFilter<T> AreNotPrivate()
        {
            return new ObjectFilter<T>(obj => obj.Visibility != Private, "are not private");
        }

        public static ObjectFilter<T> AreNotPublic()
        {
            return new ObjectFilter<T>(obj => obj.Visibility != Public, "are not public");
        }

        public static ObjectFilter<T> AreNotProtected()
        {
            return new ObjectFilter<T>(obj => obj.Visibility != Protected, "are not protected");
        }

        public static ObjectFilter<T> AreNotInternal()
        {
            return new ObjectFilter<T>(obj => obj.Visibility != Internal, "are not internal");
        }

        public static ObjectFilter<T> AreNotProtectedInternal()
        {
            return new ObjectFilter<T>(obj => obj.Visibility != ProtectedInternal, "are not protected internal");
        }

        public static ObjectFilter<T> AreNotPrivateProtected()
        {
            return new ObjectFilter<T>(obj => obj.Visibility != PrivateProtected, "are not private protected");
        }
    }
}