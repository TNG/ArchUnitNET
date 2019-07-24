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

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Core.LoadTasks
{
    internal class AddTypesToNamespace : ILoadTask
    {
        private readonly Namespace _ns;
        private readonly List<IType> _types;

        public AddTypesToNamespace(Namespace ns, List<IType> types)
        {
            _ns = ns;
            _types = types;
        }

        public void Execute()
        {
            ((List<IType>) _ns.Types).AddRange(_types.Where(type => type.Namespace == _ns));
        }
    }
}