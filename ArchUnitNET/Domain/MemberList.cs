/*
 * Copyright 2019 TNG Technology Consulting GmbH
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Fluent;
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

        public IEnumerator<IMember> GetEnumerator()
        {
            return _list.GetEnumerator();
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

        public void AddRange(IEnumerable<IMember> memberCollection)
        {
            memberCollection.ForEach(member => _list.Add(member));
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
    }
}