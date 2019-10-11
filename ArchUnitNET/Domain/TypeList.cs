using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Extensions;
using Equ;

namespace ArchUnitNET.Domain
{
    public class TypeList : MemberwiseEquatable<TypeList>, IList<IObjectProvider<IType>>, IObjectProvider<IType>
    {
        private readonly IList<IObjectProvider<IType>> _typeProviderList;

        public TypeList()
        {
            _typeProviderList = new List<IObjectProvider<IType>>();
        }

        public TypeList(IList<IObjectProvider<IType>> typeProviderList)
        {
            _typeProviderList = typeProviderList;
        }

        public TypeList(Type firstType, params Type[] moreTypes) : this()
        {
            Add(firstType, moreTypes);
        }

        public TypeList(IEnumerable<Type> types) : this()
        {
            Add(types);
        }

        public TypeList(IType firstType, params IType[] moreTypes) : this()
        {
            Add(firstType, moreTypes);
        }

        public TypeList(IEnumerable<IType> types) : this()
        {
            Add(types);
        }

        public TypeList(IEnumerable<string> patterns, bool useRegularExpressions = false) : this()
        {
            Add(patterns, useRegularExpressions);
        }

        public TypeList(string pattern, bool useRegularExpressions = false, params string[] morePatterns) : this()
        {
            Add(pattern, useRegularExpressions, morePatterns);
        }

        public void Add(IObjectProvider<IType> typeProvider)
        {
            _typeProviderList.Add(typeProvider);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _typeProviderList.GetEnumerator();
        }

        public IEnumerator<IObjectProvider<IType>> GetEnumerator()
        {
            return _typeProviderList.GetEnumerator();
        }

        public void Clear()
        {
            _typeProviderList.Clear();
        }

        public bool Contains(IObjectProvider<IType> item)
        {
            return _typeProviderList.Contains(item);
        }

        public void CopyTo(IObjectProvider<IType>[] array, int arrayIndex)
        {
            _typeProviderList.CopyTo(array, arrayIndex);
        }

        public bool Remove(IObjectProvider<IType> item)
        {
            return _typeProviderList.Remove(item);
        }

        public int Count => _typeProviderList.Count;
        public bool IsReadOnly => _typeProviderList.IsReadOnly;

        public int IndexOf(IObjectProvider<IType> item)
        {
            return _typeProviderList.IndexOf(item);
        }

        public void Insert(int index, IObjectProvider<IType> item)
        {
            _typeProviderList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _typeProviderList.RemoveAt(index);
        }

        public IObjectProvider<IType> this[int index]
        {
            get => _typeProviderList[index];
            set => _typeProviderList[index] = value;
        }

        public string Description => _typeProviderList
            .Aggregate("", (current, typeProvider) => current + "\r\n" + typeProvider.Description).Remove(0, 4);

        public IEnumerable<IType> GetObjects(Architecture architecture)
        {
            return _typeProviderList.SelectMany(provider => provider.GetObjects(architecture)).Distinct();
        }

        public void Add(Type firstType, params Type[] moreTypes)
        {
            Add(new TypeCollection(firstType, moreTypes));
        }

        public void Add(IEnumerable<Type> types)
        {
            Add(new TypeCollection(types));
        }

        public void Add(IType firstType, params IType[] moreTypes)
        {
            Add(new ArchTypeCollection(firstType, moreTypes));
        }

        public void Add(IEnumerable<IType> types)
        {
            Add(new ArchTypeCollection(types));
        }

        public void Add(IEnumerable<string> patterns, bool useRegularExpressions = false)
        {
            Add(new PatternCollection(patterns, useRegularExpressions));
        }

        public void Add(string pattern, bool useRegularExpressions = false, params string[] morePatterns)
        {
            Add(new PatternCollection(pattern, useRegularExpressions, morePatterns));
        }

        private abstract class Collection<T> : IObjectProvider<IType>
        {
            protected readonly List<T> Items;

            protected Collection(IEnumerable<T> item)
            {
                Items = item.ToList();
            }

            protected Collection(T firstItem, params T[] moreItems)
            {
                Items = new List<T> {firstItem};
                Items.AddRange(moreItems);
            }

            public abstract string Description { get; }
            public abstract IEnumerable<IType> GetObjects(Architecture architecture);
        }

        private class TypeCollection : Collection<Type>
        {
            public TypeCollection(IEnumerable<Type> types) : base(types)
            {
            }

            public TypeCollection(Type firstType, params Type[] moreTypes) : base(firstType, moreTypes)
            {
            }

            public override string Description => Items
                .Aggregate("", (current, type) => current + ", " + type.Name).Remove(0, 2);

            public override IEnumerable<IType> GetObjects(Architecture architecture)
            {
                return Items.Select(architecture.GetITypeOfType).Distinct();
            }
        }

        private class ArchTypeCollection : Collection<IType>
        {
            public ArchTypeCollection(IEnumerable<IType> types) : base(types)
            {
            }

            public ArchTypeCollection(IType firstType, params IType[] moreTypes) : base(firstType, moreTypes)
            {
            }

            public override string Description => Items
                .Aggregate("", (current, type) => current + ", " + type.Name).Remove(0, 2);

            public override IEnumerable<IType> GetObjects(Architecture architecture)
            {
                return Items.Distinct();
            }
        }

        private class PatternCollection : Collection<string>
        {
            private readonly bool _useRegularExpressions;

            public PatternCollection(IEnumerable<string> patterns, bool useRegularExpressions = false) : base(patterns)
            {
                _useRegularExpressions = useRegularExpressions;
            }

            public PatternCollection(string pattern, bool useRegularExpressions = false,
                params string[] morePatterns) : base(pattern, morePatterns)
            {
                _useRegularExpressions = useRegularExpressions;
            }

            public override string Description => Items
                .Aggregate("", (current, pattern) => current + ", \"" + pattern + "\"").Remove(0, 2);

            public override IEnumerable<IType> GetObjects(Architecture architecture)
            {
                var types = new List<IType>();
                foreach (var pattern in Items)
                {
                    types.AddRange(architecture.Types.Where(type =>
                        type.FullNameMatches(pattern, _useRegularExpressions)));
                }

                return types.Distinct();
            }
        }
    }
}