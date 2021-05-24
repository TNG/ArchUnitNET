//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Loader;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Xunit;

namespace ArchUnitNETTests.Loader
{
    public class InstructionExtensionTests
    {
        public InstructionExtensionTests()
        {
            var mockModuleDefinition = LoadMockModule(typeof(InstructionExtensionTests).Assembly.Location);
            _nextOpNull = mockModuleDefinition.Assembly.Modules.First().Types
                .First(type => type.Name == "ClassWithPropertyA").Methods.First().Body.Instructions
                .First(instruction => instruction.OpCode.Equals(OpCodes.Ret));
            _nextOperandIsNotFieldReference = mockModuleDefinition.Assembly.Modules.First().Types
                .First(type => type.Name == "ClassWithMethodSignatureA").Methods.First().Body.Instructions
                .First(instruction => instruction.OpCode.Equals(OpCodes.Newobj));
        }

        private readonly Instruction _nullInstruction = null;
        private readonly Instruction _nextOpNull;
        private readonly Instruction _nextOperandIsNotFieldReference;

        [Theory]
        [ClassData(typeof(InstructionExtensionBuild.InstructionExtensionTestData))]
        public void InstructionExtensionMethodsProperlyHandleNullArgument(Func<Instruction, bool> instructionFunc)
        {
            Assert.Throws<ArgumentNullException>(() => instructionFunc(_nullInstruction));
        }

        private static ModuleDefinition LoadMockModule(string fileName)
        {
            try
            {
                return ModuleDefinition.ReadModule(fileName, new ReaderParameters());
            }
            catch (BadImageFormatException)
            {
                // invalid file format of DLL or executable, therefore ignored
            }

            return null;
        }

        [Fact]
        public void EndOfOperationsDetectedForSetBackingField()
        {
            Assert.False(_nextOpNull.IsMethodCallToSetBackingFieldDefinition());
        }

        [Fact]
        public void GetAssigneeFieldDefinitionProperlyHandlesNextOpNotFieldReference()
        {
            Assert.Null(_nextOperandIsNotFieldReference.GetAssigneeFieldDefinition());
        }

        [Fact]
        public void GetAssigneeFieldDefinitionProperlyHandlesNextOpNull()
        {
            Assert.Null(_nextOpNull.GetAssigneeFieldDefinition());
        }

        [Fact]
        public void GetAssigneeFieldDefinitionProperlyHandlesNullArgument()
        {
            Assert.Throws<ArgumentNullException>(() => _nullInstruction.GetAssigneeFieldDefinition());
        }
    }

    public class InstructionExtensionBuild
    {
        private static object[] BuildInstructionExtensionTestData(Func<Instruction, bool> instructionFunc)
        {
            return new object[] {instructionFunc};
        }

        public class InstructionExtensionTestData : IEnumerable<object[]>
        {
            private readonly List<object[]> _instructionExtensionData = new List<object[]>
            {
                BuildInstructionExtensionTestData(InstructionExtensions.IsOperationForBackedProperty),
                BuildInstructionExtensionTestData(InstructionExtensions.IsMethodCallAssignment),
                BuildInstructionExtensionTestData(InstructionExtensions.IsMethodCallOp),
                BuildInstructionExtensionTestData(InstructionExtensions.IsMethodCallToSetBackingFieldDefinition),
                BuildInstructionExtensionTestData(InstructionExtensions.IsReturnOp),
                BuildInstructionExtensionTestData(InstructionExtensions.IsNewObjectOp),
                BuildInstructionExtensionTestData(InstructionExtensions.IsOperandBackingField),
                BuildInstructionExtensionTestData(InstructionExtensions.IsSetFieldOp),
                BuildInstructionExtensionTestData(InstructionExtensions.IsNewObjectToSetBackingField)
            };

            public IEnumerator<object[]> GetEnumerator()
            {
                return _instructionExtensionData.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}