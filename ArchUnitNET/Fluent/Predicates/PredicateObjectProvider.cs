using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Predicates
{
    public class PredicateObjectProvider<T> : IObjectProvider<T>
        where T : ICanBeAnalyzed
    {
        internal PredicateObjectProvider(IObjectProvider<T> objectProvider, IPredicate<T> predicate)
        {
            ObjectProvider = objectProvider;
            Predicate = predicate;
        }

        protected IObjectProvider<T> ObjectProvider { get; }
        protected IPredicate<T> Predicate { get; }

        public string Description => $"{ObjectProvider.Description} {Predicate.Description}";

        public IEnumerable<T> GetObjects(Architecture architecture) =>
            Predicate.GetMatchingObjects(
                ObjectProvider.GetObjects(architecture),
                architecture
            );

        public string FormatDescription(
            string emptyDescription,
            string singleDescription,
            string multipleDescription
        ) =>
            $"{multipleDescription} {Description}";

        public override string ToString() => Description;

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
            
            return GetType() == obj.GetType() && Equals((PredicateObjectProvider<T>)obj);
        }
        
        private bool Equals(PredicateObjectProvider<T> other)
        {
            return Equals(ObjectProvider, other.ObjectProvider)
                && Equals(Predicate, other.Predicate);
        }
        
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = ObjectProvider != null ? ObjectProvider.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (Predicate != null ? Predicate.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
