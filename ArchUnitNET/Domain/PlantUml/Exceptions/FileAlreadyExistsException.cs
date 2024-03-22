//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
//
// 	SPDX-License-Identifier: Apache-2.0
//

using System;
using System.Runtime.Serialization;

namespace ArchUnitNET.Domain.PlantUml.Exceptions
{
    public class FileAlreadyExistsException : Exception
    {
        public FileAlreadyExistsException() { }

        public FileAlreadyExistsException(string message)
            : base(message) { }

        public FileAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException) { }

        protected FileAlreadyExistsException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
