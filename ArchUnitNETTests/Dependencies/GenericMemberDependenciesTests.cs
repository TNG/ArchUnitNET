namespace ArchUnitNETTests.Dependencies
{
    public class GenericMemberDependenciesTests { }

    internal class GenericClass<T>
    {
        public M GenericMethod<M>(T t)
            where M : new()
        {
            return new M();
        }
    }
}
