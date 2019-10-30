//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

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