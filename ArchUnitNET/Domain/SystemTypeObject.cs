using System;
using ArchUnitNET.Domain.Extensions;

namespace ArchUnitNET.Domain
{
    internal class SystemTypeObject<T> : IObject<T>
        where T : IType
    {
        private readonly Type _type;

        public SystemTypeObject(Type type)
        {
            _type = type ?? throw new ArgumentNullException(nameof(type));
            Description = $"\"{_type.FullName}\"";
        }

        public T Get(Architecture architecture)
        {
            var result = architecture.GetITypeOfType(_type);
            if (!(result is T typedResult))
            {
                throw new ArgumentException($"Type {_type} is not of type {typeof(T)}");
            }
            return typedResult;
        }

        public string Description { get; }

        private bool Equals(SystemTypeObject<T> other)
        {
            return _type == other._type;
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

            return obj.GetType() == GetType() && Equals((SystemTypeObject<T>)obj);
        }

        public override int GetHashCode()
        {
            return _type.GetHashCode();
        }
    }
}
