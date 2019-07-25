/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paula.ruiz@tngtech.com>
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

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeAccessorOwnerBody

namespace ArchUnitNETTests.Dependencies.Members
{
    public class GetterMethodDependencyExamples
    {
        public Guid AcceptedCase { get; } = new Guid("35353");

        public Guid FirstUnacceptedCase { get; } = Guid.NewGuid();

        public Guid SecondUnacceptedCase => Guid.NewGuid();
    }

    public class SetterMethodDependencyExamples
    {
        internal ChildField _castingPairBacking;
        internal ChildField _castingLambdaPairBacking;
        internal ChildField _constructorPairBacking;
        internal ChildField _constructorLambdaPairBacking;
        internal ChildField _methodPairBacking;
        internal ChildField _methodLambdaPairBacking;
            
        public PropertyType CastingPair
        {
            set { _castingPairBacking = (ChildField) value; }
        }

        public PropertyType CastingLambdaPair
        {
            set => _castingLambdaPairBacking = (ChildField) value;
        }

        public PropertyType ConstructorPair
        {
            set { _constructorPairBacking = new PropertyType(value) as ChildField; }
        }
            
        public PropertyType ConstructorLambdaPair
        {
            set => _constructorLambdaPairBacking = new PropertyType(value) as ChildField;
        }

        public PropertyType MethodPair
        {
            set { _methodPairBacking = ChildField.NewPropertyType(value) as ChildField; }
        }
            
        public PropertyType MethodLambdaPair
        {
            set { _methodLambdaPairBacking = ChildField.NewPropertyType(value) as ChildField; }
        }
    }
    
    public class ChildField : PropertyType
    {
        public static PropertyType NewPropertyType(object thing)
        {
            return new PropertyType(thing);
        }
    }
}