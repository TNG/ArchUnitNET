// Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
//
// 	SPDX-License-Identifier: Apache-2.0

namespace ArchUnitNET.Loader
{
    public class ArchLoaderException : System.Exception
    {
        public ArchLoaderException(string message)
            : base(message) { }

        public ArchLoaderException(string message, System.Exception innerException)
            : base(message, innerException) { }
    }
}
