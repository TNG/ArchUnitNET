//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBeMadeStatic.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeAccessorOwnerBody

namespace ArchUnitNETTests.Domain.Dependencies.Members
{
    public class GetterMethodDependencyExamples
    {
        public Guid AcceptedCase
        {
            get
            {
                return new Guid("35353");
            }
        }

        public Guid FirstUnacceptedCase
        {
            get
            {
                return Guid.NewGuid();
            }
        }

        public Guid SecondUnacceptedCase => Guid.NewGuid();
    }

    public class SetterMethodDependencyExamples
    {
        internal ChildField _customPropertyBacking;
        internal ChildField _lambdaPairBacking;
        internal PropertyType _constructorLambdaPairBacking;
        internal PropertyType _methodPairBacking;
        internal PropertyType _constructorPairBacking;
        internal PropertyType _methodLambdaPairBacking;

        public ChildField CustomProperty
        {
            set
            {
                _customPropertyBacking = value;
            }
            get
            {
                return _customPropertyBacking;
            }
        }

        public ChildField LambdaPair
        {
            set => _lambdaPairBacking = value;
        }

        public PropertyType ConstructorPair
        {
            set {_constructorPairBacking = new PropertyType(value);}
        }

        public PropertyType ConstructorLambdaPair
        {
            set => _constructorLambdaPairBacking = new PropertyType(value);
        }

        public PropertyType MethodPair
        {
            set { _methodPairBacking = ChildField.NewPropertyType(value); }
        }

        public PropertyType MethodLambdaPair
        {
            set { _methodLambdaPairBacking = ChildField.NewPropertyType(value); }
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