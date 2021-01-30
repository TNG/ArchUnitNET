//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using Xunit;

namespace ArchUnitNETTests
{
    public sealed class SkipInReleaseBuild : FactAttribute
    {
        public SkipInReleaseBuild()
        {
#if !DEBUG
            Skip = "This test only works in debug build";
#endif
        }
    }
}