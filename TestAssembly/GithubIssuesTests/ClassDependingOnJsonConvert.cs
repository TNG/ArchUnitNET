//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using Newtonsoft.Json;

namespace TestAssembly.GithubIssuesTests
{
    public class ClassDependingOnJsonConvert
    {
        public void TestMethod()
        {
            var testObject = new ClassDependingOnJsonConvert();
            JsonConvert.SerializeObject(testObject);
        }
    }
}