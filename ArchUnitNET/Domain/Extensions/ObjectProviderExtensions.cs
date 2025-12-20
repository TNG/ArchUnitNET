using System;
using System.Collections;
using System.Collections.Generic;

namespace ArchUnitNET.Domain.Extensions
{
    internal static class ObjectProviderExtensions
    {
        private sealed class ReplaceDescriptionObjectProvider<T> : IObjectProvider<T>
        {
            private readonly IObjectProvider<T> _innerObjectProvider;

            public ReplaceDescriptionObjectProvider(
                IObjectProvider<T> innerObjectProvider,
                string newDescription
            )
            {
                _innerObjectProvider = innerObjectProvider;
                Description = newDescription;
            }

            public string Description { get; }

            public IEnumerable<T> GetObjects(Architecture architecture) =>
                _innerObjectProvider.GetObjects(architecture);

            public string FormatDescription(
                string emptyDescription,
                string singleDescription,
                string multipleDescription
            ) =>
                _innerObjectProvider.FormatDescription(
                    emptyDescription,
                    singleDescription,
                    multipleDescription
                );

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

                return GetType() == obj.GetType()
                    && Equals((ReplaceDescriptionObjectProvider<T>)obj);
            }

            private bool Equals(ReplaceDescriptionObjectProvider<T> other)
            {
                return Equals(_innerObjectProvider, other._innerObjectProvider)
                    && string.Equals(Description, other.Description, StringComparison.Ordinal);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (
                            (_innerObjectProvider != null ? _innerObjectProvider.GetHashCode() : 0)
                            * 397
                        ) ^ (Description != null ? Description.GetHashCode() : 0);
                }
            }
        }

        internal static IObjectProvider<T> WithDescription<T>(
            this IObjectProvider<T> objectProvider,
            string newDescription
        )
        {
            return new ReplaceDescriptionObjectProvider<T>(objectProvider, newDescription);
        }

        private sealed class DescriptionSuffixObjectProvider<T> : IObjectProvider<T>
        {
            private readonly IObjectProvider<T> _innerObjectProvider;
            private readonly string _descriptionSuffix;

            public DescriptionSuffixObjectProvider(
                IObjectProvider<T> innerObjectProvider,
                string descriptionSuffix
            )
            {
                _innerObjectProvider = innerObjectProvider;
                _descriptionSuffix = descriptionSuffix;
            }

            public string Description => $"{_innerObjectProvider.Description} {_descriptionSuffix}";

            public IEnumerable<T> GetObjects(Architecture architecture) =>
                _innerObjectProvider.GetObjects(architecture);

            public string FormatDescription(
                string emptyDescription,
                string singleDescription,
                string multipleDescription
            ) =>
                _innerObjectProvider.FormatDescription(
                    emptyDescription,
                    singleDescription,
                    multipleDescription
                );

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

                return GetType() == obj.GetType()
                    && Equals((DescriptionSuffixObjectProvider<T>)obj);
            }

            private bool Equals(DescriptionSuffixObjectProvider<T> other)
            {
                return Equals(_innerObjectProvider, other._innerObjectProvider)
                    && string.Equals(
                        _descriptionSuffix,
                        other._descriptionSuffix,
                        StringComparison.Ordinal
                    );
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (
                            (_innerObjectProvider != null ? _innerObjectProvider.GetHashCode() : 0)
                            * 397
                        ) ^ (_descriptionSuffix != null ? _descriptionSuffix.GetHashCode() : 0);
                }
            }
        }

        internal static IObjectProvider<T> WithDescriptionSuffix<T>(
            this IObjectProvider<T> objectProvider,
            string descriptionSuffix
        )
        {
            return new DescriptionSuffixObjectProvider<T>(objectProvider, descriptionSuffix);
        }
    }
}
