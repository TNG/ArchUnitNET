//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Fluent.Extensions;
using Equ;

namespace ArchUnitNET.Domain
{
    public class MemberList : MemberwiseEquatable<MemberList>, IList<IMember>
    {
        private readonly IList<IMember> _list = new List<IMember>();

        public MemberList()
        {
        }

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
    }
}