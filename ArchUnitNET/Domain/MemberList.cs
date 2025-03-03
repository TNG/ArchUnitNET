using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain.Extensions;

namespace ArchUnitNET.Domain
{
    public class MemberList : IList<IMember>
    {
        private readonly IList<IMember> _list = new List<IMember>();

        public MemberList() { }

        public MemberList(IList<IMember> list)
        {
            _list = list;
        }

        public IMember this[string index]
        {
            get => _list.FirstOrDefault(member => member.Name == index);
            set => _list[_list.IndexOf(_list.First(member => member.Name == index))] = value;
        }

        IEnumerator<IMember> IEnumerable<IMember>.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public void Add(IMember item)
        {
            _list.Add(item);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(IMember item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(IMember[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public bool Remove(IMember item)
        {
            return _list.Remove(item);
        }

        public int Count => _list.Count;

        public bool IsReadOnly => _list.IsReadOnly;

        public int IndexOf(IMember item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, IMember item)
        {
            _list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public IMember this[int index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        public IEnumerator<IMember> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public void AddRange(IEnumerable<IMember> memberCollection)
        {
            memberCollection.ForEach(member => _list.Add(member));
        }

        public bool Equals(MemberList other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return _list.SequenceEqual(other._list)
                && Equals(Count, other.Count)
                && Equals(IsReadOnly, other.IsReadOnly);
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

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((MemberList)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _list != null ? _list.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ Count.GetHashCode();
                hashCode = (hashCode * 397) ^ IsReadOnly.GetHashCode();
                return hashCode;
            }
        }
    }
}
