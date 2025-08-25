namespace ArchUnitNET.Domain
{
    internal class Object<T> : IObject<T>
        where T : ICanBeAnalyzed
    {
        private readonly T _object;

        public Object(T obj)
        {
            _object = obj;
            Description = $"\"{obj.FullName}\"";
        }

        public T Get(Architecture architecture)
        {
            return _object;
        }

        public string Description { get; }

        private bool Equals(Object<T> other)
        {
            return _object.Equals(other._object);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((Object<T>)obj);
        }

        public override int GetHashCode()
        {
            return _object.GetHashCode();
        }
    }
}
