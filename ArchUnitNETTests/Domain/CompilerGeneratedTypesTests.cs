//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies;
using Xunit;

namespace ArchUnitNETTests.Domain
{
    public class CompilerGeneratedTypesTests
    {
        private static readonly Architecture Architecture =
            StaticTestArchitectures.FullArchUnitNETArchitectureWithDependencies;

        private readonly IEnumerable<ITypeDependency> _dependencies;

        public CompilerGeneratedTypesTests()
        {
            _dependencies = Architecture.Types.SelectMany(type => type.Dependencies);
        }

        [Fact]
        public void RecognizeCompilerGeneratedTypes()
        {
            Assert.DoesNotContain(Architecture.Types,
                type => !type.IsCompilerGenerated && (type.Name.StartsWith("<") || type.Name.StartsWith("!")));
        }

        [Fact]
        public void RecognizeCompilerGeneratedMethods()
        {
            Assert.DoesNotContain(Architecture.MethodMembers,
                method => !method.IsCompilerGenerated && (method.Name.StartsWith("<") || method.Name.StartsWith("!")));
        }

        [Fact]
        public void NoCompilerGeneratedTypeInArchitecture()
        {
            Assert.DoesNotContain(Architecture.Types, type => type.IsCompilerGenerated);
            Assert.DoesNotContain(Architecture.ReferencedTypes, type => type.IsCompilerGenerated);
            Assert.DoesNotContain(Architecture.GenericParameters, type => type.IsCompilerGenerated);
        }

        [Fact]
        public void NoCompilerGeneratedTypesAsDependencyTarget()
        {
            var dependencyTargets = _dependencies.Select(dep => dep.Target);

            Assert.DoesNotContain(dependencyTargets, type => type.IsCompilerGenerated);
        }

        [Fact]
        public void NoCompilerGeneratedTypesAsGenericArguments()
        {
            var genericArgumentTypes =
                _dependencies.SelectMany(dep => dep.TargetGenericArguments.Select(argument => argument.Type));

            Assert.DoesNotContain(genericArgumentTypes, type => type.IsCompilerGenerated);
        }
    }
}