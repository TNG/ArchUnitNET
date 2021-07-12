//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Predicates;
using JetBrains.Annotations;
using static ArchUnitNET.Domain.Visibility;
using Attribute = ArchUnitNET.Domain.Attribute;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public static class ObjectPredicatesDefinition<T> where T : ICanBeAnalyzed
    {
        public static IPredicate<T> Are(string pattern, bool useRegularExpressions = false)
        {
            return new SimplePredicate<T>(obj => obj.FullNameMatches(pattern, useRegularExpressions),
                "have full name " + (useRegularExpressions ? "matching" : "containing") + " \"" + pattern + "\"");
        }

        public static IPredicate<T> Are(IEnumerable<string> patterns, bool useRegularExpressions = false)
        {
            var patternList = patterns.ToList();
            string description;
            if (patternList.IsNullOrEmpty())
            {
                description = "not exist (impossible)";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList.Where(pattern => !pattern.Equals(firstPattern)).Distinct().Aggregate(
                    "have full name " + (useRegularExpressions ? "matching" : "containing") + " \"" +
                    firstPattern + "\"", (current, pattern) => current + " or \"" + pattern + "\"");
            }

            return new SimplePredicate<T>(
                obj => patternList.Any(pattern => obj.FullNameMatches(pattern, useRegularExpressions)), description);
        }

        public static IPredicate<T> Are(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects)
        {
            var objects = new List<ICanBeAnalyzed> {firstObject}.Union(moreObjects).OfType<T>();
            var description = moreObjects.Aggregate("are \"" + firstObject.FullName + "\"",
                (current, obj) => current + " or \"" + obj.FullName + "\"");
            return new EnumerablePredicate<T>(e => e.Intersect(objects), description);
        }

        public static IPredicate<T> Are(IEnumerable<ICanBeAnalyzed> objects)
        {
            var objectList = objects.ToList();
            string description;
            if (objectList.IsNullOrEmpty())
            {
                description = "do not exist (always empty)";
            }
            else
            {
                var firstObject = objectList.First();
                description = objectList.Where(obj => !obj.Equals(firstObject)).Distinct().Aggregate(
                    "are \"" + firstObject.FullName + "\"",
                    (current, obj) => current + " or \"" + obj.FullName + "\"");
            }

            return new EnumerablePredicate<T>(e => e.Intersect(objectList.OfType<T>()), description);
        }

        public static IPredicate<T> Are(IObjectProvider<ICanBeAnalyzed> objectProvider)
        {
            IEnumerable<T> Filter(IEnumerable<T> objects, Architecture architecture)
            {
                return objects.Intersect(objectProvider.GetObjects(architecture).OfType<T>());
            }

            return new ArchitecturePredicate<T>(Filter, "are " + objectProvider.Description);
        }

        public static IPredicate<T> CallAny(string pattern, bool useRegularExpressions = false)
        {
            return new SimplePredicate<T>(obj => obj.CallsMethod(pattern, useRegularExpressions),
                "calls any method with full name " + (useRegularExpressions ? "matching" : "containing") + " \"" +
                pattern + "\"");
        }

        public static IPredicate<T> CallAny(IEnumerable<string> patterns, bool useRegularExpressions = false)
        {
            var patternList = patterns.ToList();

            bool Filter(T type)
            {
                return patternList.Any(method => type.CallsMethod(method));
            }

            string description;
            if (patternList.IsNullOrEmpty())
            {
                description = "call one of no methods (impossible)";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList.Where(pattern => !pattern.Equals(firstPattern)).Distinct().Aggregate(
                    "call any method with full name " + (useRegularExpressions ? "matching" : "containing") +
                    " \"" +
                    firstPattern + "\"", (current, pattern) => current + " or \"" + pattern + "\"");
            }

            return new SimplePredicate<T>(Filter, description);
        }

        public static IPredicate<T> CallAny(MethodMember method, params MethodMember[] moreMethods)
        {
            var methods = new List<MethodMember> {method};
            methods.AddRange(moreMethods);
            return CallAny(methods);
        }

        public static IPredicate<T> CallAny(IObjectProvider<MethodMember> objectProvider)
        {
            IEnumerable<T> Filter(IEnumerable<T> objects, Architecture architecture)
            {
                var methods = objectProvider.GetObjects(architecture);
                return objects.Where(obj => obj.GetCalledMethods().Intersect(methods).Any());
            }

            var description = "call any " + objectProvider.Description;
            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> CallAny(IEnumerable<MethodMember> methods)
        {
            var methodList = methods.ToList();

            IEnumerable<T> Filter(IEnumerable<T> objects)
            {
                return objects.Where(obj => obj.GetCalledMethods().Intersect(methodList).Any());
            }

            string description;
            if (methodList.IsNullOrEmpty())
            {
                description = "call one of no methods (impossible)";
            }
            else
            {
                var firstMethod = methodList.First();
                description = methodList.Where(obj => !obj.Equals(firstMethod)).Distinct().Aggregate(
                    "call \"" + firstMethod.FullName + "\"",
                    (current, obj) => current + " or \"" + obj.FullName + "\"");
            }

            return new EnumerablePredicate<T>(Filter, description);
        }

        public static IPredicate<T> DependOnAny(string pattern, bool useRegularExpressions = false)
        {
            return new SimplePredicate<T>(obj => obj.DependsOn(pattern, useRegularExpressions),
                "depend on any types with full name " + (useRegularExpressions ? "matching" : "containing") + " \"" +
                pattern + "\"");
        }

        public static IPredicate<T> DependOnAny(IEnumerable<string> patterns, bool useRegularExpressions = false)
        {
            var patternList = patterns.ToList();

            bool Filter(T type)
            {
                return type.GetTypeDependencies().Any(target =>
                    patternList.Any(pattern => target.FullNameMatches(pattern, useRegularExpressions)));
            }

            string description;
            if (patternList.IsNullOrEmpty())
            {
                description = "have no dependencies";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList.Where(pattern => !pattern.Equals(firstPattern)).Distinct().Aggregate(
                    "depend on any types with full name " + (useRegularExpressions ? "matching" : "containing") +
                    " \"" +
                    firstPattern + "\"", (current, pattern) => current + " or \"" + pattern + "\"");
            }

            return new SimplePredicate<T>(Filter, description);
        }

        public static IPredicate<T> DependOnAny(IType firstType, params IType[] moreTypes)
        {
            var types = new List<IType> {firstType};
            types.AddRange(moreTypes);
            return DependOnAny(types);
        }

        public static IPredicate<T> DependOnAny(Type firstType, params Type[] moreTypes)
        {
            var types = new List<Type> {firstType};
            types.AddRange(moreTypes);
            return DependOnAny(types);
        }

        public static IPredicate<T> DependOnAny(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<T> Filter(IEnumerable<T> objects, Architecture architecture)
            {
                var types = objectProvider.GetObjects(architecture);
                return objects.Where(obj => obj.GetTypeDependencies().Intersect(types).Any());
            }

            var description = "depend on any " + objectProvider.Description;
            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> DependOnAny(IEnumerable<IType> types)
        {
            var typeList = types.ToList();

            IEnumerable<T> Filter(IEnumerable<T> objects)
            {
                return objects.Where(obj => obj.GetTypeDependencies().Intersect(typeList).Any());
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "have no dependencies";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(obj => !obj.Equals(firstType)).Distinct().Aggregate(
                    "depend on \"" + firstType.FullName + "\"",
                    (current, obj) => current + " or \"" + obj.FullName + "\"");
            }

            return new EnumerablePredicate<T>(Filter, description);
        }

        public static IPredicate<T> DependOnAny(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            IEnumerable<T> Filter(IEnumerable<T> objects, Architecture architecture)
            {
                var tList = typeList.Select(architecture.GetITypeOfType);
                return objects.Where(obj => obj.GetTypeDependencies().Intersect(tList).Any());
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "have no dependencies";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(obj => obj != firstType).Distinct().Aggregate(
                    "depend on \"" + firstType.FullName + "\"",
                    (current, obj) => current + " or \"" + obj.FullName + "\"");
            }

            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> FollowCustomPredicate(Func<T, bool> predicate, string description)
        {
            return new SimplePredicate<T>(predicate, description);
        }

        public static IPredicate<T> OnlyDependOn(string pattern, bool useRegularExpressions = false)
        {
            return new SimplePredicate<T>(obj => obj.OnlyDependsOn(pattern, useRegularExpressions),
                "only depend on types with full name " + (useRegularExpressions ? "matching" : "containing") + " \"" +
                pattern + "\"");
        }

        public static IPredicate<T> OnlyDependOn(IEnumerable<string> patterns, bool useRegularExpressions = false)
        {
            var patternList = patterns.ToList();

            bool Filter(T type)
            {
                return type.GetTypeDependencies().All(target =>
                    patternList.Any(pattern => target.FullNameMatches(pattern, useRegularExpressions)));
            }

            string description;
            if (patternList.IsNullOrEmpty())
            {
                description = "have no dependencies";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList.Where(pattern => !pattern.Equals(firstPattern)).Distinct().Aggregate(
                    "only depend on types with full name " + (useRegularExpressions ? "matching" : "containing") +
                    " \"" +
                    firstPattern + "\"", (current, pattern) => current + " or \"" + pattern + "\"");
            }

            return new SimplePredicate<T>(Filter, description);
        }

        public static IPredicate<T> OnlyDependOn(IType firstType, params IType[] moreTypes)
        {
            var types = new List<IType> {firstType};
            types.AddRange(moreTypes);
            return OnlyDependOn(types);
        }

        public static IPredicate<T> OnlyDependOn(Type firstType, params Type[] moreTypes)
        {
            var types = new List<Type> {firstType};
            types.AddRange(moreTypes);
            return OnlyDependOn(types);
        }

        public static IPredicate<T> OnlyDependOn(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<T> Filter(IEnumerable<T> objects, Architecture architecture)
            {
                var types = objectProvider.GetObjects(architecture);
                return objects.Where(obj => obj.GetTypeDependencies(architecture).Except(types).IsNullOrEmpty());
            }

            var description = "only depend on " + objectProvider.Description;
            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> OnlyDependOn(IEnumerable<IType> types)
        {
            var typeList = types.ToList();

            IEnumerable<T> Filter(IEnumerable<T> objects, Architecture architecture)
            {
                return objects.Where(obj => obj.GetTypeDependencies(architecture).Except(typeList).IsNullOrEmpty());
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "have no dependencies";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(obj => !obj.Equals(firstType)).Distinct().Aggregate(
                    "only depend on \"" + firstType.FullName + "\"",
                    (current, obj) => current + " or \"" + obj.FullName + "\"");
            }

            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> OnlyDependOn(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            IEnumerable<T> Filter(IEnumerable<T> objects, Architecture architecture)
            {
                var tList = typeList.Select(architecture.GetITypeOfType);
                return objects.Where(obj => obj.GetTypeDependencies(architecture).Except(tList).IsNullOrEmpty());
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "have no dependencies";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(obj => obj != firstType).Distinct().Aggregate(
                    "only depend on \"" + firstType.FullName + "\"",
                    (current, obj) => current + " or \"" + obj.FullName + "\"");
            }

            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> HaveAnyAttributes(string pattern, bool useRegularExpressions = false)
        {
            return new SimplePredicate<T>(obj => obj.HasAttribute(pattern, useRegularExpressions),
                "have any attribute with full name " + (useRegularExpressions ? "matching" : "containing") + " \"" +
                pattern + "\"");
        }

        public static IPredicate<T> HaveAnyAttributes(IEnumerable<string> patterns, bool useRegularExpressions = false)
        {
            var patternList = patterns.ToList();

            bool Filter(T type)
            {
                return type.Attributes.Any(attribute =>
                    patternList.Any(pattern => attribute.FullNameMatches(pattern, useRegularExpressions)));
            }

            string description;
            if (patternList.IsNullOrEmpty())
            {
                description = "have one of no attributes (impossible)";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList.Where(pattern => !pattern.Equals(firstPattern)).Distinct().Aggregate(
                    "have any attribute with full name " + (useRegularExpressions ? "matching" : "containing") +
                    " \"" +
                    firstPattern + "\"", (current, pattern) => current + " or \"" + pattern + "\"");
            }

            return new SimplePredicate<T>(Filter, description);
        }

        public static IPredicate<T> HaveAnyAttributes(Attribute firstAttribute, params Attribute[] moreAttributes)
        {
            var attributes = new List<Attribute> {firstAttribute};
            attributes.AddRange(moreAttributes);
            return HaveAnyAttributes(attributes);
        }

        public static IPredicate<T> HaveAnyAttributes(Type firstAttribute, params Type[] moreAttributes)
        {
            var attributes = new List<Type> {firstAttribute};
            attributes.AddRange(moreAttributes);
            return HaveAnyAttributes(attributes);
        }

        public static IPredicate<T> HaveAnyAttributes(IObjectProvider<Attribute> objectProvider)
        {
            IEnumerable<T> Filter(IEnumerable<T> objects, Architecture architecture)
            {
                var attributes = objectProvider.GetObjects(architecture);
                return objects.Where(obj => obj.Attributes.Intersect(attributes).Any());
            }

            var description = "have any " + objectProvider.Description;
            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> HaveAnyAttributes(IEnumerable<Attribute> attributes)
        {
            var attributeList = attributes.ToList();

            IEnumerable<T> Filter(IEnumerable<T> objects)
            {
                return objects.Where(obj => obj.Attributes.Intersect(attributeList).Any());
            }

            string description;
            if (attributeList.IsNullOrEmpty())
            {
                description = "have one of no attributes (impossible)";
            }
            else
            {
                var firstAttribute = attributeList.First();
                description = attributeList.Where(obj => !obj.Equals(firstAttribute)).Distinct().Aggregate(
                    "have attribute \"" + firstAttribute.FullName + "\"",
                    (current, attribute) => current + " or \"" + attribute.FullName + "\"");
            }

            return new EnumerablePredicate<T>(Filter, description);
        }

        public static IPredicate<T> HaveAnyAttributes(IEnumerable<Type> attributes)
        {
            var attributeList = attributes.ToList();

            IEnumerable<T> Filter(IEnumerable<T> objects, Architecture architecture)
            {
                var archAttributeList = attributeList.Select(architecture.GetAttributeOfType);
                return objects.Where(obj => obj.Attributes.Intersect(archAttributeList).Any());
            }

            string description;
            if (attributeList.IsNullOrEmpty())
            {
                description = "have one of no attributes (impossible)";
            }
            else
            {
                var firstType = attributeList.First();
                description = attributeList.Where(obj => obj != firstType).Distinct().Aggregate(
                    "have attribute \"" + firstType.FullName + "\"",
                    (current, obj) => current + " or \"" + obj.FullName + "\"");
            }

            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> OnlyHaveAttributes(string pattern, bool useRegularExpressions = false)
        {
            return new SimplePredicate<T>(obj => obj.OnlyHasAttributes(pattern, useRegularExpressions),
                "only have attributes with full name " + (useRegularExpressions ? "matching" : "containing") + " \"" +
                pattern + "\"");
        }

        public static IPredicate<T> OnlyHaveAttributes(IEnumerable<string> patterns, bool useRegularExpressions = false)
        {
            var patternList = patterns.ToList();

            bool Filter(T type)
            {
                return type.Attributes.IsNullOrEmpty() || type.Attributes.All(attribute =>
                    patternList.Any(pattern => attribute.FullNameMatches(pattern, useRegularExpressions)));
            }

            string description;
            if (patternList.IsNullOrEmpty())
            {
                description = "have no attributes";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList.Where(pattern => !pattern.Equals(firstPattern)).Distinct().Aggregate(
                    "only have attributes with full name " + (useRegularExpressions ? "matching" : "containing") +
                    " \"" +
                    firstPattern + "\"", (current, pattern) => current + " or \"" + pattern + "\"");
            }

            return new SimplePredicate<T>(Filter, description);
        }

        public static IPredicate<T> OnlyHaveAttributes(Attribute firstAttribute, params Attribute[] moreAttributes)
        {
            var attributes = new List<Attribute> {firstAttribute};
            attributes.AddRange(moreAttributes);
            return OnlyHaveAttributes(attributes);
        }

        public static IPredicate<T> OnlyHaveAttributes(Type firstAttribute, params Type[] moreAttributes)
        {
            var attributes = new List<Type> {firstAttribute};
            attributes.AddRange(moreAttributes);
            return OnlyHaveAttributes(attributes);
        }

        public static IPredicate<T> OnlyHaveAttributes(IObjectProvider<Attribute> objectProvider)
        {
            IEnumerable<T> Filter(IEnumerable<T> objects, Architecture architecture)
            {
                var attributes = objectProvider.GetObjects(architecture);
                return objects.Where(obj => !obj.Attributes.Except(attributes).Any());
            }

            var description = "only have " + objectProvider.Description;
            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> OnlyHaveAttributes(IEnumerable<Attribute> attributes)
        {
            var attributeList = attributes.ToList();

            IEnumerable<T> Filter(IEnumerable<T> objects)
            {
                return objects.Where(obj => !obj.Attributes.Except(attributeList).Any());
            }

            string description;
            if (attributeList.IsNullOrEmpty())
            {
                description = "have no attributes";
            }
            else
            {
                var firstAttribute = attributeList.First();
                description = attributeList.Where(obj => !obj.Equals(firstAttribute)).Distinct().Aggregate(
                    "only have attribute \"" + firstAttribute.FullName + "\"",
                    (current, attribute) => current + " or \"" + attribute.FullName + "\"");
            }

            return new EnumerablePredicate<T>(Filter, description);
        }

        public static IPredicate<T> OnlyHaveAttributes(IEnumerable<Type> attributes)
        {
            var attributeList = attributes.ToList();

            IEnumerable<T> Filter(IEnumerable<T> objects, Architecture architecture)
            {
                var archAttributeList = attributeList.Select(architecture.GetAttributeOfType);
                return objects.Where(obj => !obj.Attributes.Except(archAttributeList).Any());
            }

            string description;
            if (attributeList.IsNullOrEmpty())
            {
                description = "have no attributes";
            }
            else
            {
                var firstType = attributeList.First();
                description = attributeList.Where(obj => obj != firstType).Distinct().Aggregate(
                    "only have attribute \"" + firstType.FullName + "\"",
                    (current, obj) => current + " or \"" + obj.FullName + "\"");
            }

            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> HaveAnyAttributesWithArguments(object firstArgumentValue,
            params object[] moreArgumentValues)
        {
            var argumentValues = new List<object> {firstArgumentValue};
            argumentValues.AddRange(moreArgumentValues);
            return HaveAnyAttributesWithArguments(argumentValues);
        }

        public static IPredicate<T> HaveAttributeWithArguments(string attribute, object firstArgumentValue,
            params object[] moreArgumentValues)
        {
            var argumentValues = new List<object> {firstArgumentValue};
            argumentValues.AddRange(moreArgumentValues);
            return HaveAttributeWithArguments(attribute, argumentValues);
        }

        public static IPredicate<T> HaveAttributeWithArguments(Attribute attribute, object firstArgumentValue,
            params object[] moreArgumentValues)
        {
            var argumentValues = new List<object> {firstArgumentValue};
            argumentValues.AddRange(moreArgumentValues);
            return HaveAttributeWithArguments(attribute, argumentValues);
        }

        public static IPredicate<T> HaveAttributeWithArguments(Type attribute, object firstArgumentValue,
            params object[] moreArgumentValues)
        {
            var argumentValues = new List<object> {firstArgumentValue};
            argumentValues.AddRange(moreArgumentValues);
            return HaveAttributeWithArguments(attribute, argumentValues);
        }

        public static IPredicate<T> HaveAnyAttributesWithNamedArguments((string, object) firstAttributeArgument,
            params (string, object)[] moreAttributeArguments)
        {
            var attributeArguments = new List<(string, object)> {firstAttributeArgument};
            attributeArguments.AddRange(moreAttributeArguments);
            return HaveAnyAttributesWithNamedArguments(attributeArguments);
        }

        public static IPredicate<T> HaveAttributeWithNamedArguments(string attribute,
            (string, object) firstAttributeArgument, params (string, object)[] moreAttributeArguments)
        {
            var attributeArguments = new List<(string, object)> {firstAttributeArgument};
            attributeArguments.AddRange(moreAttributeArguments);
            return HaveAttributeWithNamedArguments(attribute, attributeArguments);
        }

        public static IPredicate<T> HaveAttributeWithNamedArguments(Attribute attribute,
            (string, object) firstAttributeArgument, params (string, object)[] moreAttributeArguments)
        {
            var attributeArguments = new List<(string, object)> {firstAttributeArgument};
            attributeArguments.AddRange(moreAttributeArguments);
            return HaveAttributeWithNamedArguments(attribute, attributeArguments);
        }

        public static IPredicate<T> HaveAttributeWithNamedArguments(Type attribute,
            (string, object) firstAttributeArgument, params (string, object)[] moreAttributeArguments)
        {
            var attributeArguments = new List<(string, object)> {firstAttributeArgument};
            attributeArguments.AddRange(moreAttributeArguments);
            return HaveAttributeWithNamedArguments(attribute, attributeArguments);
        }

        public static IPredicate<T> HaveAnyAttributesWithArguments(IEnumerable<object> argumentValues)
        {
            var argumentValueList = argumentValues.ToList();
            string description;
            if (argumentValueList.IsNullOrEmpty())
            {
                description = "have no or any attributes with arguments (always true)";
            }
            else
            {
                var firstArgument = argumentValueList.First();
                description = argumentValueList.Where(attribute => attribute != firstArgument).Aggregate(
                    "have any attributes with arguments \"" + firstArgument + "\"",
                    (current, argumentValue) => current + " and \"" + argumentValue + "\"");
            }

            bool Predicate(T obj, Architecture architecture)
            {
                var attributeArguments = obj.AttributeInstances
                    .SelectMany(instance => instance.AttributeArguments.Select(arg => arg.Value)).ToList();
                var typeAttributeArguments = attributeArguments.OfType<ITypeInstance<IType>>().Select(t => t.Type)
                    .Union(attributeArguments.OfType<IType>()).ToList();
                foreach (var arg in argumentValueList)
                {
                    if (arg is Type argType)
                    {
                        if (typeAttributeArguments.All(t => t.FullName != argType.FullName))
                        {
                            return false;
                        }
                    }
                    else if (!attributeArguments.Contains(arg))
                    {
                        return false;
                    }
                }

                return true;
            }

            return new ArchitecturePredicate<T>(Predicate, description);
        }

        public static IPredicate<T> HaveAttributeWithArguments([NotNull] string attribute,
            IEnumerable<object> argumentValues)
        {
            string description;
            var argumentValueList = argumentValues.ToList();
            if (argumentValueList.IsNullOrEmpty())
            {
                description = "have attribute \"" + attribute + "\"";
            }
            else
            {
                var firstArgument = argumentValueList.First();
                description = argumentValueList.Where(att => att != firstArgument).Aggregate(
                    "have attribute \"" + attribute + "\" with arguments \"" + firstArgument + "\"",
                    (current, argumentValue) => current + " and \"" + argumentValue + "\"");
            }

            bool Predicate(T obj, Architecture architecture)
            {
                foreach (var attributeInstance in obj.AttributeInstances)
                {
                    if (!attributeInstance.Type.FullNameMatches(attribute))
                    {
                        goto NextAttribute;
                    }

                    var attributeArguments = attributeInstance.AttributeArguments.Select(arg => arg.Value).ToList();
                    var typeAttributeArguments = attributeArguments.OfType<ITypeInstance<IType>>().Select(t => t.Type)
                        .Union(attributeArguments.OfType<IType>()).ToList();
                    foreach (var arg in argumentValueList)
                    {
                        if (arg is Type argType)
                        {
                            if (typeAttributeArguments.All(t => t.FullName != argType.FullName))
                            {
                                goto NextAttribute;
                            }
                        }
                        else if (!attributeArguments.Contains(arg))
                        {
                            goto NextAttribute;
                        }
                    }

                    return true;
                    NextAttribute: ;
                }

                return false;
            }

            return new ArchitecturePredicate<T>(Predicate, description);
        }

        public static IPredicate<T> HaveAttributeWithArguments([NotNull] Attribute attribute,
            IEnumerable<object> argumentValues)
        {
            string description;
            var argumentValueList = argumentValues.ToList();
            if (argumentValueList.IsNullOrEmpty())
            {
                description = "have attribute \"" + attribute.FullName + "\"";
            }
            else
            {
                var firstArgument = argumentValueList.First();
                description = argumentValueList.Where(att => att != firstArgument).Aggregate(
                    "have attribute \"" + attribute.FullName + "\" with arguments \"" + firstArgument + "\"",
                    (current, argumentValue) => current + " and \"" + argumentValue + "\"");
            }

            bool Predicate(T obj, Architecture architecture)
            {
                foreach (var attributeInstance in obj.AttributeInstances)
                {
                    if (!attributeInstance.Type.Equals(attribute))
                    {
                        goto NextAttribute;
                    }

                    var attributeArguments = attributeInstance.AttributeArguments.Select(arg => arg.Value).ToList();
                    var typeAttributeArguments = attributeArguments.OfType<ITypeInstance<IType>>().Select(t => t.Type)
                        .Union(attributeArguments.OfType<IType>()).ToList();
                    foreach (var arg in argumentValueList)
                    {
                        if (arg is Type argType)
                        {
                            if (typeAttributeArguments.All(t => t.FullName != argType.FullName))
                            {
                                goto NextAttribute;
                            }
                        }
                        else if (!attributeArguments.Contains(arg))
                        {
                            goto NextAttribute;
                        }
                    }

                    return true;
                    NextAttribute: ;
                }

                return false;
            }

            return new ArchitecturePredicate<T>(Predicate, description);
        }

        public static IPredicate<T> HaveAttributeWithArguments([NotNull] Type attribute,
            IEnumerable<object> argumentValues)
        {
            string description;
            var argumentValueList = argumentValues.ToList();
            if (argumentValueList.IsNullOrEmpty())
            {
                description = "have attribute \"" + attribute.FullName + "\"";
            }
            else
            {
                var firstArgument = argumentValueList.First();
                description = argumentValueList.Where(att => att != firstArgument).Aggregate(
                    "have attribute \"" + attribute.FullName + "\" with arguments \"" + firstArgument + "\"",
                    (current, argumentValue) => current + " and \"" + argumentValue + "\"");
            }

            bool Predicate(T obj, Architecture architecture)
            {
                foreach (var attributeInstance in obj.AttributeInstances)
                {
                    if (!attributeInstance.Type.Equals(architecture.GetAttributeOfType(attribute)))
                    {
                        goto NextAttribute;
                    }

                    var attributeArguments = attributeInstance.AttributeArguments.Select(arg => arg.Value).ToList();
                    var typeAttributeArguments = attributeArguments.OfType<ITypeInstance<IType>>().Select(t => t.Type)
                        .Union(attributeArguments.OfType<IType>()).ToList();
                    foreach (var arg in argumentValueList)
                    {
                        if (arg is Type argType)
                        {
                            if (typeAttributeArguments.All(t => t.FullName != argType.FullName))
                            {
                                goto NextAttribute;
                            }
                        }
                        else if (!attributeArguments.Contains(arg))
                        {
                            goto NextAttribute;
                        }
                    }

                    return true;
                    NextAttribute: ;
                }

                return false;
            }

            return new ArchitecturePredicate<T>(Predicate, description);
        }

        public static IPredicate<T> HaveAnyAttributesWithNamedArguments(
            IEnumerable<(string, object)> attributeArguments)
        {
            var argumentList = attributeArguments.ToList();
            string description;
            if (argumentList.IsNullOrEmpty())
            {
                description = "have no or any attributes with named arguments (always true)";
            }
            else
            {
                var firstArgument = argumentList.First();
                description = argumentList.Where(attribute => attribute != firstArgument).Aggregate(
                    "have any attributes with named arguments \"" + firstArgument.Item1 + "=" + firstArgument.Item2 +
                    "\"",
                    (current, arg) =>
                        current + " and \"" + arg.Item1 + "=" + arg.Item2 + "\"");
            }

            bool Predicate(T obj, Architecture architecture)
            {
                var attArguments = obj.AttributeInstances.SelectMany(instance =>
                        instance.AttributeArguments.OfType<AttributeNamedArgument>()
                            .Select(arg => (arg.Name, arg.Value)))
                    .ToList();
                var typeAttributeArguments = attArguments
                    .Where(arg => arg.Value is ITypeInstance<IType> || arg.Value is IType).ToList();
                foreach (var arg in argumentList)
                {
                    if (arg.Item2 is Type argType)
                    {
                        if (typeAttributeArguments.All(t =>
                            t.Name != arg.Item1 ||
                            t.Value is ITypeInstance<IType> typeInstance &&
                            typeInstance.Type.FullName != argType.FullName ||
                            t.Value is IType type && type.FullName != argType.FullName))
                        {
                            return false;
                        }
                    }
                    else if (!attArguments.Contains(arg))
                    {
                        return false;
                    }
                }

                return true;
            }

            return new ArchitecturePredicate<T>(Predicate, description);
        }

        public static IPredicate<T> HaveAttributeWithNamedArguments([NotNull] string attribute,
            IEnumerable<(string, object)> attributeArguments)
        {
            string description;
            var argumentList = attributeArguments.ToList();
            if (argumentList.IsNullOrEmpty())
            {
                description = "have attribute \"" + attribute + "\"";
            }
            else
            {
                var firstArgument = argumentList.First();
                description = argumentList.Where(att => att != firstArgument).Aggregate(
                    "have attribute \"" + attribute + "\" with named arguments \"" + firstArgument.Item1 +
                    "=" + firstArgument.Item2 + "\"",
                    (current, arg) => current + " and \"" + arg.Item1 + "=" + arg.Item2 + "\"");
            }

            bool Predicate(T obj, Architecture architecture)
            {
                foreach (var attributeInstance in obj.AttributeInstances)
                {
                    if (!attributeInstance.Type.FullNameMatches(attribute))
                    {
                        goto NextAttribute;
                    }

                    var attributeArgs = attributeInstance.AttributeArguments.OfType<AttributeNamedArgument>()
                        .Select(arg => (arg.Name, arg.Value)).ToList();
                    var typeAttributeArguments = attributeArgs
                        .Where(arg => arg.Value is ITypeInstance<IType> || arg.Value is IType).ToList();
                    foreach (var arg in argumentList)
                    {
                        if (arg.Item2 is Type argType)
                        {
                            if (typeAttributeArguments.All(t =>
                                t.Name != arg.Item1 ||
                                t.Value is ITypeInstance<IType> typeInstance &&
                                typeInstance.Type.FullName != argType.FullName ||
                                t.Value is IType type && type.FullName != argType.FullName))
                            {
                                goto NextAttribute;
                            }
                        }
                        else if (!argumentList.Contains(arg))
                        {
                            goto NextAttribute;
                        }
                    }

                    return true;
                    NextAttribute: ;
                }

                return false;
            }

            return new ArchitecturePredicate<T>(Predicate, description);
        }

        public static IPredicate<T> HaveAttributeWithNamedArguments([NotNull] Attribute attribute,
            IEnumerable<(string, object)> attributeArguments)
        {
            string description;
            var argumentList = attributeArguments.ToList();
            if (argumentList.IsNullOrEmpty())
            {
                description = "have attribute \"" + attribute.FullName + "\"";
            }
            else
            {
                var firstArgument = argumentList.First();
                description = argumentList.Where(att => att != firstArgument).Aggregate(
                    "have attribute \"" + attribute.FullName + "\" with named arguments \"" + firstArgument.Item1 +
                    "=" + firstArgument.Item2 + "\"",
                    (current, arg) => current + " and \"" + arg.Item1 + "=" + arg.Item2 + "\"");
            }

            bool Condition(T obj, Architecture architecture)
            {
                foreach (var attributeInstance in obj.AttributeInstances)
                {
                    if (!attributeInstance.Type.Equals(attribute))
                    {
                        goto NextAttribute;
                    }

                    var attributeArgs = attributeInstance.AttributeArguments.OfType<AttributeNamedArgument>()
                        .Select(arg => (arg.Name, arg.Value)).ToList();
                    var typeAttributeArguments = attributeArgs
                        .Where(arg => arg.Value is ITypeInstance<IType> || arg.Value is IType).ToList();
                    foreach (var arg in argumentList)
                    {
                        if (arg.Item2 is Type argType)
                        {
                            if (typeAttributeArguments.All(t =>
                                t.Name != arg.Item1 ||
                                t.Value is ITypeInstance<IType> typeInstance &&
                                typeInstance.Type.FullName != argType.FullName ||
                                t.Value is IType type && type.FullName != argType.FullName))
                            {
                                goto NextAttribute;
                            }
                        }
                        else if (!argumentList.Contains(arg))
                        {
                            goto NextAttribute;
                        }
                    }

                    return true;
                    NextAttribute: ;
                }

                return false;
            }

            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> HaveAttributeWithNamedArguments([NotNull] Type attribute,
            IEnumerable<(string, object)> attributeArguments)
        {
            string description;
            var argumentList = attributeArguments.ToList();
            if (argumentList.IsNullOrEmpty())
            {
                description = "have attribute \"" + attribute.FullName + "\"";
            }
            else
            {
                var firstArgument = argumentList.First();
                description = argumentList.Where(att => att != firstArgument).Aggregate(
                    "have attribute \"" + attribute.FullName + "\" with named arguments \"" + firstArgument.Item1 +
                    "=" + firstArgument.Item2 + "\"",
                    (current, arg) => current + " and \"" + arg.Item1 + "=" + arg.Item2 + "\"");
            }

            bool Predicate(T obj, Architecture architecture)
            {
                foreach (var attributeInstance in obj.AttributeInstances)
                {
                    if (!attributeInstance.Type.Equals(architecture.GetAttributeOfType(attribute)))
                    {
                        goto NextAttribute;
                    }

                    var attributeArgs = attributeInstance.AttributeArguments.OfType<AttributeNamedArgument>()
                        .Select(arg => (arg.Name, arg.Value)).ToList();
                    var typeAttributeArguments = attributeArgs
                        .Where(arg => arg.Value is ITypeInstance<IType> || arg.Value is IType).ToList();
                    foreach (var arg in argumentList)
                    {
                        if (arg.Item2 is Type argType)
                        {
                            if (typeAttributeArguments.All(t =>
                                t.Name != arg.Item1 ||
                                t.Value is ITypeInstance<IType> typeInstance &&
                                typeInstance.Type.FullName != argType.FullName ||
                                t.Value is IType type && type.FullName != argType.FullName))
                            {
                                goto NextAttribute;
                            }
                        }
                        else if (!argumentList.Contains(arg))
                        {
                            goto NextAttribute;
                        }
                    }

                    return true;
                    NextAttribute: ;
                }

                return false;
            }

            return new ArchitecturePredicate<T>(Predicate, description);
        }

        public static IPredicate<T> HaveName(string name)
        {
            return new SimplePredicate<T>(obj => obj.Name.Equals(name), "have name \"" + name + "\"");
        }

        public static IPredicate<T> HaveNameMatching(string pattern)
        {
            return new SimplePredicate<T>(obj => obj.NameMatches(pattern), "have name matching \"" + pattern + "\"");
        }

        public static IPredicate<T> HaveFullName(string fullname)
        {
            return new SimplePredicate<T>(obj => obj.FullName.Equals(fullname), "have full name \"" + fullname + "\"");
        }

        public static IPredicate<T> HaveFullNameMatching(string pattern)
        {
            return new SimplePredicate<T>(obj => obj.FullNameMatches(pattern),
                "have full name matching \"" + pattern + "\"");
        }

        public static IPredicate<T> HaveNameStartingWith(string pattern)
        {
            return new SimplePredicate<T>(obj => obj.NameStartsWith(pattern),
                "have name starting with \"" + pattern + "\"");
        }

        public static IPredicate<T> HaveNameEndingWith(string pattern)
        {
            return new SimplePredicate<T>(obj => obj.NameEndsWith(pattern),
                "have name ending with \"" + pattern + "\"");
        }

        public static IPredicate<T> HaveNameContaining(string pattern)
        {
            return new SimplePredicate<T>(obj => obj.NameContains(pattern), "have name containing \"" + pattern + "\"");
        }

        public static IPredicate<T> HaveFullNameContaining(string pattern)
        {
            return new SimplePredicate<T>(obj => obj.FullNameContains(pattern),
                "have full name containing \"" + pattern + "\"");
        }

        public static IPredicate<T> ArePrivate()
        {
            return new SimplePredicate<T>(obj => obj.Visibility == Private, "are private");
        }

        public static IPredicate<T> ArePublic()
        {
            return new SimplePredicate<T>(obj => obj.Visibility == Public, "are public");
        }

        public static IPredicate<T> AreProtected()
        {
            return new SimplePredicate<T>(obj => obj.Visibility == Protected, "are protected");
        }

        public static IPredicate<T> AreInternal()
        {
            return new SimplePredicate<T>(obj => obj.Visibility == Internal, "are internal");
        }

        public static IPredicate<T> AreProtectedInternal()
        {
            return new SimplePredicate<T>(obj => obj.Visibility == ProtectedInternal, "are protected internal");
        }

        public static IPredicate<T> ArePrivateProtected()
        {
            return new SimplePredicate<T>(obj => obj.Visibility == PrivateProtected, "are private protected");
        }


        //Negations


        public static IPredicate<T> AreNot(string pattern, bool useRegularExpressions = false)
        {
            return new SimplePredicate<T>(obj => !obj.FullNameMatches(pattern, useRegularExpressions),
                "do not have full name " + (useRegularExpressions ? "matching" : "containing") + " \"" + pattern +
                "\"");
        }

        public static IPredicate<T> AreNot(IEnumerable<string> patterns, bool useRegularExpressions = false)
        {
            var patternList = patterns.ToList();
            string description;
            if (patternList.IsNullOrEmpty())
            {
                description = "exist (always true)";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList.Where(pattern => !pattern.Equals(firstPattern)).Distinct().Aggregate(
                    "do not have full name " + (useRegularExpressions ? "matching" : "containing") + " \"" +
                    firstPattern + "\"", (current, pattern) => current + " or \"" + pattern + "\"");
            }

            return new SimplePredicate<T>(
                obj => patternList.All(pattern => !obj.FullNameMatches(pattern, useRegularExpressions)), description);
        }

        public static IPredicate<T> AreNot(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects)
        {
            var objects = new List<ICanBeAnalyzed> {firstObject}.Concat(moreObjects).OfType<T>();
            var description = moreObjects.Aggregate("are not \"" + firstObject.FullName + "\"",
                (current, obj) => current + " or \"" + obj.FullName + "\"");
            return new EnumerablePredicate<T>(e => e.Except(objects), description);
        }

        public static IPredicate<T> AreNot(IEnumerable<ICanBeAnalyzed> objects)
        {
            var objectList = objects.ToList();
            string description;
            if (objectList.IsNullOrEmpty())
            {
                description = "are not no object (always true)";
            }
            else
            {
                var firstObject = objectList.First();
                description = objectList.Where(obj => !obj.Equals(firstObject)).Distinct().Aggregate(
                    "are not \"" + firstObject.FullName + "\"",
                    (current, obj) => current + " or \"" + obj.FullName + "\"");
            }

            return new EnumerablePredicate<T>(e => e.Except(objectList.OfType<T>()), description);
        }

        public static IPredicate<T> AreNot(IObjectProvider<ICanBeAnalyzed> objectProvider)
        {
            IEnumerable<T> Filter(IEnumerable<T> objects, Architecture architecture)
            {
                return objects.Except(objectProvider.GetObjects(architecture).OfType<T>());
            }

            return new ArchitecturePredicate<T>(Filter, "are not " + objectProvider.Description);
        }

        public static IPredicate<T> DoNotCallAny(string pattern, bool useRegularExpressions = false)
        {
            return new SimplePredicate<T>(obj => !obj.CallsMethod(pattern, useRegularExpressions),
                "do not call any methods with full name " + (useRegularExpressions ? "matching" : "containing") +
                " \"" + pattern + "\"");
        }

        public static IPredicate<T> DoNotCallAny(IEnumerable<string> patterns, bool useRegularExpressions = false)
        {
            var patternList = patterns.ToList();

            bool Filter(T type)
            {
                return patternList.All(pattern => !type.CallsMethod(pattern, useRegularExpressions));
            }

            string description;
            if (patternList.IsNullOrEmpty())
            {
                description = "do not call one of no methods (always true)";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList.Where(pattern => !pattern.Equals(firstPattern)).Distinct().Aggregate(
                    "do not call any methods with full name " + (useRegularExpressions ? "matching" : "containing") +
                    " \"" +
                    firstPattern + "\"", (current, pattern) => current + " or \"" + pattern + "\"");
            }

            return new SimplePredicate<T>(Filter, description);
        }

        public static IPredicate<T> DoNotCallAny(MethodMember method, params MethodMember[] moreMethods)
        {
            var methods = new List<MethodMember> {method};
            methods.AddRange(moreMethods);
            return DoNotCallAny(methods);
        }

        public static IPredicate<T> DoNotCallAny(IObjectProvider<MethodMember> objectProvider)
        {
            IEnumerable<T> Filter(IEnumerable<T> objects, Architecture architecture)
            {
                var methods = objectProvider.GetObjects(architecture);
                return objects.Where(obj => !obj.GetCalledMethods().Intersect(methods).Any());
            }

            var description = "do not call " + objectProvider.Description;
            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> DoNotCallAny(IEnumerable<MethodMember> methods)
        {
            var methodList = methods.ToList();

            IEnumerable<T> Filter(IEnumerable<T> objects)
            {
                return objects.Where(obj => !obj.GetCalledMethods().Intersect(methodList).Any());
            }

            string description;
            if (methodList.IsNullOrEmpty())
            {
                description = "do not call one of no methods (always true)";
            }
            else
            {
                var firstMethod = methodList.First();
                description = methodList.Where(obj => !obj.Equals(firstMethod)).Distinct().Aggregate(
                    "do not call \"" + firstMethod.FullName + "\"",
                    (current, obj) => current + " or \"" + obj.FullName + "\"");
            }

            return new EnumerablePredicate<T>(Filter, description);
        }

        public static IPredicate<T> DoNotDependOnAny(string pattern, bool useRegularExpressions = false)
        {
            return new SimplePredicate<T>(obj => !obj.DependsOn(pattern, useRegularExpressions),
                "do not depend on any types with full name " + (useRegularExpressions ? "matching" : "containing") +
                " \"" + pattern + "\"");
        }

        public static IPredicate<T> DoNotDependOnAny(IEnumerable<string> patterns, bool useRegularExpressions = false)
        {
            var patternList = patterns.ToList();

            bool Filter(T type)
            {
                return type.GetTypeDependencies().All(target =>
                    patternList.All(pattern => target.FullNameMatches(pattern, useRegularExpressions)));
            }

            string description;
            if (patternList.IsNullOrEmpty())
            {
                description = "do not depend on no types (always true)";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList.Where(pattern => !pattern.Equals(firstPattern)).Distinct().Aggregate(
                    "do not depend on any types with full name " + (useRegularExpressions ? "matching" : "containing") +
                    " \"" +
                    firstPattern + "\"", (current, pattern) => current + " or \"" + pattern + "\"");
            }

            return new SimplePredicate<T>(Filter, description);
        }

        public static IPredicate<T> DoNotDependOnAny(IType firstType, params IType[] moreTypes)
        {
            var types = new List<IType> {firstType};
            types.AddRange(moreTypes);
            return DoNotDependOnAny(types);
        }

        public static IPredicate<T> DoNotDependOnAny(Type firstType, params Type[] moreTypes)
        {
            var types = new List<Type> {firstType};
            types.AddRange(moreTypes);
            return DoNotDependOnAny(types);
        }

        public static IPredicate<T> DoNotDependOnAny(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<T> Filter(IEnumerable<T> objects, Architecture architecture)
            {
                var types = objectProvider.GetObjects(architecture);
                return objects.Where(obj => !obj.GetTypeDependencies().Intersect(types).Any());
            }

            var description = "do not depend on any " + objectProvider.Description;
            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> DoNotDependOnAny(IEnumerable<IType> types)
        {
            var typeList = types.ToList();

            IEnumerable<T> Filter(IEnumerable<T> objects)
            {
                return objects.Where(obj => !obj.GetTypeDependencies().Intersect(typeList).Any());
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "do not depend on no types (always true)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(obj => !obj.Equals(firstType)).Distinct().Aggregate(
                    "do not depend on \"" + firstType.FullName + "\"",
                    (current, obj) => current + " or \"" + obj.FullName + "\"");
            }

            return new EnumerablePredicate<T>(Filter, description);
        }

        public static IPredicate<T> DoNotDependOnAny(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            IEnumerable<T> Filter(IEnumerable<T> objects, Architecture architecture)
            {
                var tList = typeList.Select(architecture.GetITypeOfType);
                return objects.Where(obj => !obj.GetTypeDependencies().Intersect(tList).Any());
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "do not depend on no types (always true)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(obj => obj != firstType).Distinct().Aggregate(
                    "do not depend on \"" + firstType.FullName + "\"",
                    (current, obj) => current + " or \"" + obj.FullName + "\"");
            }

            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> DoNotHaveAnyAttributes(string pattern, bool useRegularExpressions = false)
        {
            return new SimplePredicate<T>(obj => !obj.HasAttribute(pattern, useRegularExpressions),
                "do not have any attribute with full name " + (useRegularExpressions ? "matching" : "containing") +
                " \"" +
                pattern + "\"");
        }

        public static IPredicate<T> DoNotHaveAnyAttributes(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            var patternList = patterns.ToList();

            bool Filter(T type)
            {
                return !type.Attributes.Any(attribute =>
                    patternList.Any(pattern => attribute.FullNameMatches(pattern, useRegularExpressions)));
            }

            string description;
            if (patternList.IsNullOrEmpty())
            {
                description = "do not have one of no attributes (always true)";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList.Where(pattern => !pattern.Equals(firstPattern)).Distinct().Aggregate(
                    "do not have any attribute with full name " + (useRegularExpressions ? "matching" : "containing") +
                    " \"" +
                    firstPattern + "\"", (current, pattern) => current + " or \"" + pattern + "\"");
            }

            return new SimplePredicate<T>(Filter, description);
        }

        public static IPredicate<T> DoNotHaveAnyAttributes(Attribute firstAttribute, params Attribute[] moreAttributes)
        {
            var attributes = new List<Attribute> {firstAttribute};
            attributes.AddRange(moreAttributes);
            return DoNotHaveAnyAttributes(attributes);
        }

        public static IPredicate<T> DoNotHaveAnyAttributes(Type firstAttribute, params Type[] moreAttributes)
        {
            var attributes = new List<Type> {firstAttribute};
            attributes.AddRange(moreAttributes);
            return DoNotHaveAnyAttributes(attributes);
        }

        public static IPredicate<T> DoNotHaveAnyAttributes(IObjectProvider<Attribute> objectProvider)
        {
            IEnumerable<T> Filter(IEnumerable<T> objects, Architecture architecture)
            {
                var types = objectProvider.GetObjects(architecture);
                return objects.Where(obj => !obj.Attributes.Intersect(types).Any());
            }

            var description = "do not have any " + objectProvider.Description;
            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> DoNotHaveAnyAttributes(IEnumerable<Attribute> attributes)
        {
            var attributeList = attributes.ToList();

            IEnumerable<T> Filter(IEnumerable<T> objects)
            {
                return objects.Where(obj => !obj.Attributes.Intersect(attributeList).Any());
            }

            string description;
            if (attributeList.IsNullOrEmpty())
            {
                description = "do not have one of no attributes (always true)";
            }
            else
            {
                var firstAttribute = attributeList.First();
                description = attributeList.Where(obj => !obj.Equals(firstAttribute)).Distinct().Aggregate(
                    "do not have attribute \"" + firstAttribute.FullName + "\"",
                    (current, attribute) => current + " or \"" + attribute.FullName + "\"");
            }

            return new EnumerablePredicate<T>(Filter, description);
        }

        public static IPredicate<T> DoNotHaveAnyAttributes(IEnumerable<Type> attributes)
        {
            var attributeList = attributes.ToList();

            IEnumerable<T> Filter(IEnumerable<T> objects, Architecture architecture)
            {
                var archAttributeList = attributeList.Select(architecture.GetAttributeOfType);
                return objects.Where(obj => !obj.Attributes.Intersect(archAttributeList).Any());
            }

            string description;
            if (attributeList.IsNullOrEmpty())
            {
                description = "do not have one of no attributes (always true)";
            }
            else
            {
                var firstType = attributeList.First();
                description = attributeList.Where(obj => obj != firstType).Distinct().Aggregate(
                    "do not have attribute \"" + firstType.FullName + "\"",
                    (current, obj) => current + " or \"" + obj.FullName + "\"");
            }

            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> DoNotHaveAnyAttributesWithArguments(object firstArgumentValue,
            params object[] moreArgumentValues)
        {
            var argumentValues = new List<object> {firstArgumentValue};
            argumentValues.AddRange(moreArgumentValues);
            return DoNotHaveAnyAttributesWithArguments(argumentValues);
        }

        public static IPredicate<T> DoNotHaveAttributeWithArguments(string attribute, object firstArgumentValue,
            params object[] moreArgumentValues)
        {
            var argumentValues = new List<object> {firstArgumentValue};
            argumentValues.AddRange(moreArgumentValues);
            return DoNotHaveAttributeWithArguments(attribute, argumentValues);
        }

        public static IPredicate<T> DoNotHaveAttributeWithArguments(Attribute attribute,
            object firstArgumentValue,
            params object[] moreArgumentValues)
        {
            var argumentValues = new List<object> {firstArgumentValue};
            argumentValues.AddRange(moreArgumentValues);
            return DoNotHaveAttributeWithArguments(attribute, argumentValues);
        }

        public static IPredicate<T> DoNotHaveAttributeWithArguments(Type attribute, object firstArgumentValue,
            params object[] moreArgumentValues)
        {
            var argumentValues = new List<object> {firstArgumentValue};
            argumentValues.AddRange(moreArgumentValues);
            return DoNotHaveAttributeWithArguments(attribute, argumentValues);
        }

        public static IPredicate<T> DoNotHaveAnyAttributesWithNamedArguments(
            (string, object) firstAttributeArgument,
            params (string, object)[] moreAttributeArguments)
        {
            var attributeArguments = new List<(string, object)> {firstAttributeArgument};
            attributeArguments.AddRange(moreAttributeArguments);
            return DoNotHaveAnyAttributesWithNamedArguments(attributeArguments);
        }

        public static IPredicate<T> DoNotHaveAttributeWithNamedArguments(string attribute,
            (string, object) firstAttributeArgument, params (string, object)[] moreAttributeArguments)
        {
            var attributeArguments = new List<(string, object)> {firstAttributeArgument};
            attributeArguments.AddRange(moreAttributeArguments);
            return DoNotHaveAttributeWithNamedArguments(attribute, attributeArguments);
        }

        public static IPredicate<T> DoNotHaveAttributeWithNamedArguments(Attribute attribute,
            (string, object) firstAttributeArgument, params (string, object)[] moreAttributeArguments)
        {
            var attributeArguments = new List<(string, object)> {firstAttributeArgument};
            attributeArguments.AddRange(moreAttributeArguments);
            return DoNotHaveAttributeWithNamedArguments(attribute, attributeArguments);
        }

        public static IPredicate<T> DoNotHaveAttributeWithNamedArguments(Type attribute,
            (string, object) firstAttributeArgument, params (string, object)[] moreAttributeArguments)
        {
            var attributeArguments = new List<(string, object)> {firstAttributeArgument};
            attributeArguments.AddRange(moreAttributeArguments);
            return DoNotHaveAttributeWithNamedArguments(attribute, attributeArguments);
        }

        public static IPredicate<T> DoNotHaveAnyAttributesWithArguments(IEnumerable<object> argumentValues)
        {
            var argumentValueList = argumentValues.ToList();
            string description;
            if (argumentValueList.IsNullOrEmpty())
            {
                description = "do not have no or any attributes with arguments (impossible)";
            }
            else
            {
                var firstArgument = argumentValueList.First();
                description = argumentValueList.Where(attribute => attribute != firstArgument).Aggregate(
                    "do not have any attributes with arguments \"" + firstArgument + "\"",
                    (current, argumentValue) => current + " and \"" + argumentValue + "\"");
            }

            bool Predicate(T obj, Architecture architecture)
            {
                var attributeArguments = obj.AttributeInstances
                    .SelectMany(instance => instance.AttributeArguments.Select(arg => arg.Value)).ToList();
                var typeAttributeArguments = attributeArguments.OfType<ITypeInstance<IType>>().Select(t => t.Type)
                    .Union(attributeArguments.OfType<IType>()).ToList();
                foreach (var arg in argumentValueList)
                {
                    if (arg is Type argType)
                    {
                        if (typeAttributeArguments.Any(t => t.FullName == argType.FullName))
                        {
                            return false;
                        }
                    }
                    else if (attributeArguments.Contains(arg))
                    {
                        return false;
                    }
                }

                return true;
            }

            return new ArchitecturePredicate<T>(Predicate, description);
        }

        public static IPredicate<T> DoNotHaveAttributeWithArguments([NotNull] string attribute,
            IEnumerable<object> argumentValues)
        {
            string description;
            var argumentValueList = argumentValues.ToList();
            if (argumentValueList.IsNullOrEmpty())
            {
                description = "do not have attribute \"" + attribute + "\"";
            }
            else
            {
                var firstArgument = argumentValueList.First();
                description = argumentValueList.Where(att => att != firstArgument).Aggregate(
                    "do not have attribute \"" + attribute + "\" with arguments \"" + firstArgument + "\"",
                    (current, argumentValue) => current + " and \"" + argumentValue + "\"");
            }

            bool Predicate(T obj, Architecture architecture)
            {
                foreach (var attributeInstance in obj.AttributeInstances)
                {
                    if (!attributeInstance.Type.FullNameMatches(attribute))
                    {
                        goto NextAttribute;
                    }

                    var attributeArguments = attributeInstance.AttributeArguments.Select(arg => arg.Value).ToList();
                    var typeAttributeArguments = attributeArguments.OfType<ITypeInstance<IType>>().Select(t => t.Type)
                        .Union(attributeArguments.OfType<IType>()).ToList();
                    foreach (var arg in argumentValueList)
                    {
                        if (arg is Type argType)
                        {
                            if (typeAttributeArguments.All(t => t.FullName != argType.FullName))
                            {
                                goto NextAttribute;
                            }
                        }
                        else if (!attributeArguments.Contains(arg))
                        {
                            goto NextAttribute;
                        }
                    }

                    return false;
                    NextAttribute: ;
                }

                return true;
            }

            return new ArchitecturePredicate<T>(Predicate, description);
        }

        public static IPredicate<T> DoNotHaveAttributeWithArguments([NotNull] Attribute attribute,
            IEnumerable<object> argumentValues)
        {
            string description;
            var argumentValueList = argumentValues.ToList();
            if (argumentValueList.IsNullOrEmpty())
            {
                description = "do not have attribute \"" + attribute.FullName + "\"";
            }
            else
            {
                var firstArgument = argumentValueList.First();
                description = argumentValueList.Where(att => att != firstArgument).Aggregate(
                    "do not have attribute \"" + attribute.FullName + "\" with arguments \"" + firstArgument + "\"",
                    (current, argumentValue) => current + " and \"" + argumentValue + "\"");
            }

            bool Predicate(T obj, Architecture architecture)
            {
                foreach (var attributeInstance in obj.AttributeInstances)
                {
                    if (!attributeInstance.Type.Equals(attribute))
                    {
                        goto NextAttribute;
                    }

                    var attributeArguments = attributeInstance.AttributeArguments.Select(arg => arg.Value).ToList();
                    var typeAttributeArguments = attributeArguments.OfType<ITypeInstance<IType>>().Select(t => t.Type)
                        .Union(attributeArguments.OfType<IType>()).ToList();
                    foreach (var arg in argumentValueList)
                    {
                        if (arg is Type argType)
                        {
                            if (typeAttributeArguments.All(t => t.FullName != argType.FullName))
                            {
                                goto NextAttribute;
                            }
                        }
                        else if (!attributeArguments.Contains(arg))
                        {
                            goto NextAttribute;
                        }
                    }

                    return false;
                    NextAttribute: ;
                }

                return true;
            }

            return new ArchitecturePredicate<T>(Predicate, description);
        }

        public static IPredicate<T> DoNotHaveAttributeWithArguments([NotNull] Type attribute,
            IEnumerable<object> argumentValues)
        {
            string description;
            var argumentValueList = argumentValues.ToList();
            if (argumentValueList.IsNullOrEmpty())
            {
                description = "do not have attribute \"" + attribute.FullName + "\"";
            }
            else
            {
                var firstArgument = argumentValueList.First();
                description = argumentValueList.Where(att => att != firstArgument).Aggregate(
                    "do not have attribute \"" + attribute.FullName + "\" with arguments \"" + firstArgument + "\"",
                    (current, argumentValue) => current + " and \"" + argumentValue + "\"");
            }

            bool Predicate(T obj, Architecture architecture)
            {
                foreach (var attributeInstance in obj.AttributeInstances)
                {
                    if (!attributeInstance.Type.Equals(architecture.GetAttributeOfType(attribute)))
                    {
                        goto NextAttribute;
                    }

                    var attributeArguments = attributeInstance.AttributeArguments.Select(arg => arg.Value).ToList();
                    var typeAttributeArguments = attributeArguments.OfType<ITypeInstance<IType>>().Select(t => t.Type)
                        .Union(attributeArguments.OfType<IType>()).ToList();
                    foreach (var arg in argumentValueList)
                    {
                        if (arg is Type argType)
                        {
                            if (typeAttributeArguments.All(t => t.FullName != argType.FullName))
                            {
                                goto NextAttribute;
                            }
                        }
                        else if (!attributeArguments.Contains(arg))
                        {
                            goto NextAttribute;
                        }
                    }

                    return false;
                    NextAttribute: ;
                }

                return true;
            }

            return new ArchitecturePredicate<T>(Predicate, description);
        }

        public static IPredicate<T> DoNotHaveAnyAttributesWithNamedArguments(
            IEnumerable<(string, object)> attributeArguments)
        {
            var argumentList = attributeArguments.ToList();
            string description;
            if (argumentList.IsNullOrEmpty())
            {
                description = "do not have no or any attributes with named arguments (impossible)";
            }
            else
            {
                var firstArgument = argumentList.First();
                description = argumentList.Where(attribute => attribute != firstArgument).Aggregate(
                    "do not have any attributes with named arguments \"" + firstArgument.Item1 + "=" +
                    firstArgument.Item2 +
                    "\"",
                    (current, arg) =>
                        current + " and \"" + arg.Item1 + "=" + arg.Item2 + "\"");
            }

            bool Condition(T obj, Architecture architecture)
            {
                var attArguments = obj.AttributeInstances.SelectMany(instance =>
                        instance.AttributeArguments.OfType<AttributeNamedArgument>()
                            .Select(arg => (arg.Name, arg.Value)))
                    .ToList();
                var typeAttributeArguments = attArguments
                    .Where(arg => arg.Value is ITypeInstance<IType> || arg.Value is IType).ToList();
                foreach (var arg in argumentList)
                {
                    if (arg.Item2 is Type argType)
                    {
                        if (typeAttributeArguments.Any(t =>
                            t.Name == arg.Item1 &&
                            (t.Value is ITypeInstance<IType> typeInstance &&
                             typeInstance.Type.FullName == argType.FullName ||
                             t.Value is IType type && type.FullName == argType.FullName)))
                        {
                            return false;
                        }
                    }
                    else if (attArguments.Contains(arg))
                    {
                        return false;
                    }
                }

                return true;
            }

            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> DoNotHaveAttributeWithNamedArguments([NotNull] string attribute,
            IEnumerable<(string, object)> attributeArguments)
        {
            string description;
            var argumentList = attributeArguments.ToList();
            if (argumentList.IsNullOrEmpty())
            {
                description = "do not have attribute \"" + attribute + "\"";
            }
            else
            {
                var firstArgument = argumentList.First();
                description = argumentList.Where(att => att != firstArgument).Aggregate(
                    "do not have attribute \"" + attribute + "\" with named arguments \"" + firstArgument.Item1 +
                    "=" + firstArgument.Item2 + "\"",
                    (current, arg) => current + " and \"" + arg.Item1 + "=" + arg.Item2 + "\"");
            }

            bool Predicate(T obj, Architecture architecture)
            {
                foreach (var attributeInstance in obj.AttributeInstances)
                {
                    if (!attributeInstance.Type.FullNameMatches(attribute))
                    {
                        goto NextAttribute;
                    }

                    var attributeArgs = attributeInstance.AttributeArguments.OfType<AttributeNamedArgument>()
                        .Select(arg => (arg.Name, arg.Value)).ToList();
                    var typeAttributeArguments = attributeArgs
                        .Where(arg => arg.Value is ITypeInstance<IType> || arg.Value is IType).ToList();
                    foreach (var arg in argumentList)
                    {
                        if (arg.Item2 is Type argType)
                        {
                            if (typeAttributeArguments.All(t => t.Name != arg.Item1 ||
                                                                t.Value is ITypeInstance<IType> typeInstance &&
                                                                typeInstance.Type.FullName != argType.FullName ||
                                                                t.Value is IType type &&
                                                                type.FullName != argType.FullName))
                            {
                                goto NextAttribute;
                            }
                        }
                        else if (!argumentList.Contains(arg))
                        {
                            goto NextAttribute;
                        }
                    }

                    return false;
                    NextAttribute: ;
                }

                return true;
            }

            return new ArchitecturePredicate<T>(Predicate, description);
        }

        public static IPredicate<T> DoNotHaveAttributeWithNamedArguments([NotNull] Attribute attribute,
            IEnumerable<(string, object)> attributeArguments)
        {
            string description;
            var argumentList = attributeArguments.ToList();
            if (argumentList.IsNullOrEmpty())
            {
                description = "do not have attribute \"" + attribute.FullName + "\"";
            }
            else
            {
                var firstArgument = argumentList.First();
                description = argumentList.Where(att => att != firstArgument).Aggregate(
                    "do not have attribute \"" + attribute.FullName + "\" with named arguments \"" +
                    firstArgument.Item1 +
                    "=" + firstArgument.Item2 + "\"",
                    (current, arg) => current + " and \"" + arg.Item1 + "=" + arg.Item2 + "\"");
            }

            bool Predicate(T obj, Architecture architecture)
            {
                foreach (var attributeInstance in obj.AttributeInstances)
                {
                    if (!attributeInstance.Type.Equals(attribute))
                    {
                        goto NextAttribute;
                    }

                    var attributeArgs = attributeInstance.AttributeArguments.OfType<AttributeNamedArgument>()
                        .Select(arg => (arg.Name, arg.Value)).ToList();
                    var typeAttributeArguments = attributeArgs
                        .Where(arg => arg.Value is ITypeInstance<IType> || arg.Value is IType).ToList();
                    foreach (var arg in argumentList)
                    {
                        if (arg.Item2 is Type argType)
                        {
                            if (typeAttributeArguments.All(t => t.Name != arg.Item1 ||
                                                                t.Value is ITypeInstance<IType> typeInstance &&
                                                                typeInstance.Type.FullName != argType.FullName ||
                                                                t.Value is IType type &&
                                                                type.FullName != argType.FullName))
                            {
                                goto NextAttribute;
                            }
                        }
                        else if (!argumentList.Contains(arg))
                        {
                            goto NextAttribute;
                        }
                    }

                    return false;
                    NextAttribute: ;
                }

                return true;
            }

            return new ArchitecturePredicate<T>(Predicate, description);
        }

        public static IPredicate<T> DoNotHaveAttributeWithNamedArguments([NotNull] Type attribute,
            IEnumerable<(string, object)> attributeArguments)
        {
            string description;
            var argumentList = attributeArguments.ToList();
            if (argumentList.IsNullOrEmpty())
            {
                description = "do not have attribute \"" + attribute.FullName + "\"";
            }
            else
            {
                var firstArgument = argumentList.First();
                description = argumentList.Where(att => att != firstArgument).Aggregate(
                    "do not have attribute \"" + attribute.FullName + "\" with named arguments \"" +
                    firstArgument.Item1 +
                    "=" + firstArgument.Item2 + "\"",
                    (current, arg) => current + " and \"" + arg.Item1 + "=" + arg.Item2 + "\"");
            }

            bool Predicate(T obj, Architecture architecture)
            {
                foreach (var attributeInstance in obj.AttributeInstances)
                {
                    if (!attributeInstance.Type.Equals(architecture.GetAttributeOfType(attribute)))
                    {
                        goto NextAttribute;
                    }

                    var attributeArgs = attributeInstance.AttributeArguments.OfType<AttributeNamedArgument>()
                        .Select(arg => (arg.Name, arg.Value)).ToList();
                    var typeAttributeArguments = attributeArgs
                        .Where(arg => arg.Value is ITypeInstance<IType> || arg.Value is IType).ToList();
                    foreach (var arg in argumentList)
                    {
                        if (arg.Item2 is Type argType)
                        {
                            if (typeAttributeArguments.All(t => t.Name != arg.Item1 ||
                                                                t.Value is ITypeInstance<IType> typeInstance &&
                                                                typeInstance.Type.FullName != argType.FullName ||
                                                                t.Value is IType type &&
                                                                type.FullName != argType.FullName))
                            {
                                goto NextAttribute;
                            }
                        }
                        else if (!argumentList.Contains(arg))
                        {
                            goto NextAttribute;
                        }
                    }

                    return false;
                    NextAttribute: ;
                }

                return true;
            }

            return new ArchitecturePredicate<T>(Predicate, description);
        }

        public static IPredicate<T> DoNotHaveName(string name)
        {
            return new SimplePredicate<T>(obj => !obj.Name.Equals(name), "do not have name \"" + name + "\"");
        }

        public static IPredicate<T> DoNotHaveNameMatching(string pattern)
        {
            return new SimplePredicate<T>(obj => !obj.NameMatches(pattern),
                "do not have name matching \"" + pattern + "\"");
        }

        public static IPredicate<T> DoNotHaveFullName(string fullname)
        {
            return new SimplePredicate<T>(obj => !obj.FullName.Equals(fullname),
                "do not have full name \"" + fullname + "\"");
        }

        public static IPredicate<T> DoNotHaveFullNameMatching(string pattern)
        {
            return new SimplePredicate<T>(obj => !obj.FullNameMatches(pattern),
                "do not have full name matching \"" + pattern + "\"");
        }

        public static IPredicate<T> DoNotHaveNameStartingWith(string pattern)
        {
            return new SimplePredicate<T>(obj => !obj.NameStartsWith(pattern),
                "do not have name starting with \"" + pattern + "\"");
        }

        public static IPredicate<T> DoNotHaveNameEndingWith(string pattern)
        {
            return new SimplePredicate<T>(obj => !obj.NameEndsWith(pattern),
                "do not have name ending with \"" + pattern + "\"");
        }

        public static IPredicate<T> DoNotHaveNameContaining(string pattern)
        {
            return new SimplePredicate<T>(obj => !obj.NameContains(pattern),
                "do not have name containing \"" + pattern + "\"");
        }

        public static IPredicate<T> DoNotHaveFullNameContaining(string pattern)
        {
            return new SimplePredicate<T>(obj => !obj.FullNameContains(pattern),
                "do not have full name containing \"" + pattern + "\"");
        }

        public static IPredicate<T> AreNotPrivate()
        {
            return new SimplePredicate<T>(obj => obj.Visibility != Private, "are not private");
        }

        public static IPredicate<T> AreNotPublic()
        {
            return new SimplePredicate<T>(obj => obj.Visibility != Public, "are not public");
        }

        public static IPredicate<T> AreNotProtected()
        {
            return new SimplePredicate<T>(obj => obj.Visibility != Protected, "are not protected");
        }

        public static IPredicate<T> AreNotInternal()
        {
            return new SimplePredicate<T>(obj => obj.Visibility != Internal, "are not internal");
        }

        public static IPredicate<T> AreNotProtectedInternal()
        {
            return new SimplePredicate<T>(obj => obj.Visibility != ProtectedInternal, "are not protected internal");
        }

        public static IPredicate<T> AreNotPrivateProtected()
        {
            return new SimplePredicate<T>(obj => obj.Visibility != PrivateProtected, "are not private protected");
        }
    }
}