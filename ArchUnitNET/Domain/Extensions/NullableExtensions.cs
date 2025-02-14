using ArchUnitNET.Domain.Exceptions;

namespace ArchUnitNET.Domain.Extensions
{
    public static class NullableExtensions
    {
        public static T RequiredNotNull<T>(this T obj)
        {
            if (obj == null)
            {
                throw new InvalidStateException("Expecting value to be not null");
            }

            return obj;
        }
    }
}
