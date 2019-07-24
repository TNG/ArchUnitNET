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

using System;
using JetBrains.Annotations;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace ArchUnitNET.Fluent
{
    public static class InstructionExtensions
    {
        public static bool IsOperationForBackedProperty([NotNull] this Instruction methodBodyInstruction)
        {
            if (methodBodyInstruction == null) throw new ArgumentNullException(nameof(methodBodyInstruction));
            return IsMethodCallToSetBackingFieldDefinition(methodBodyInstruction) ||
                   IsNewObjectToSetBackingField(methodBodyInstruction);
        }

        public static bool IsMethodCallAssignment([NotNull] this Instruction instruction)
        {
            if (instruction == null) throw new ArgumentNullException(nameof(instruction));
            var nextOp = instruction.Next;
            return IsReturnOp(nextOp) && IsMethodCallOp(instruction);
        }

        public static FieldDefinition GetAssigneeFieldDefinition([NotNull] this Instruction methodCallInstruction)
        {
            if (methodCallInstruction == null) throw new ArgumentNullException(nameof(methodCallInstruction));
            var methodCallAssignment = methodCallInstruction.Next;
            if (methodCallAssignment != null && methodCallAssignment.Operand is FieldReference fieldReference)
            {
                return fieldReference.Resolve();
            }

            return null;
        }

        public static bool IsMethodCallToSetBackingFieldDefinition([NotNull] this Instruction instruction)
        {
            if (instruction == null) throw new ArgumentNullException(nameof(instruction));
            var nextOp = instruction.Next;
            if (nextOp == null) return false;
            var assigneeIsBackingField = nextOp.IsOperandBackingField();
            var isValidSetBakingFieldOp = instruction.IsMethodCallOp() && nextOp.IsSetFieldOp() && assigneeIsBackingField;
            return isValidSetBakingFieldOp;
        }

        public static bool IsNewObjectToSetBackingField([NotNull] this Instruction instruction)
        {
            if (instruction == null) throw new ArgumentNullException(nameof(instruction));
            var nextOp = instruction.Next;
            return instruction.IsNewObjectOp() && nextOp.IsOperandBackingField();
        }

        public static bool IsNewObjectOp(this Instruction instruction)
        {
            if (instruction == null) throw new ArgumentNullException(nameof(instruction));
            return instruction.OpCode == OpCodes.Newobj;
        }

        public static bool IsOperandBackingField([NotNull] this Instruction instruction)
        {
            if (instruction == null) throw new ArgumentNullException(nameof(instruction));
            if (instruction.Operand is FieldReference fieldReference)
            {
                return fieldReference.IsBackingField();
            }

            return false;
        }

        public static bool IsMethodCallOp([NotNull] this Instruction methodBodyInstruction)
        {
            if (methodBodyInstruction == null) throw new ArgumentNullException(nameof(methodBodyInstruction));
            return methodBodyInstruction.OpCode == OpCodes.Call || methodBodyInstruction.OpCode == OpCodes.Callvirt;
        }

        public static bool IsSetFieldOp([NotNull] this Instruction nextOp)
        {
            if (nextOp == null) throw new ArgumentNullException(nameof(nextOp));
            return nextOp.OpCode == OpCodes.Stfld;
        }

        public static bool IsReturnOp([NotNull] this Instruction operation)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));
            return operation.OpCode == OpCodes.Ret;
        }
    }
}