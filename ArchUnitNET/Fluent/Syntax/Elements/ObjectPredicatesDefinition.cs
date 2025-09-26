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
    internal static class ObjectPredicatesDefinition<T>
        where T : ICanBeAnalyzed
    {
        public static IPredicate<T> Are(IObjectProvider<ICanBeAnalyzed> objectProvider)
        {
            IEnumerable<T> Filter(IEnumerable<T> objects, Architecture architecture)
            {
                return objects.Intersect(objectProvider.GetObjects(architecture).OfType<T>());
            }

            var description = objectProvider.FormatDescription(
                "are any of no objects (always empty)",
                "are",
                "are"
            );
            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> CallAny(IObjectProvider<MethodMember> objectProvider)
        {
            IEnumerable<T> Filter(IEnumerable<T> objects, Architecture architecture)
            {
                var methods = objectProvider.GetObjects(architecture);
                return objects.Where(obj => obj.GetCalledMethods().Intersect(methods).Any());
            }

            var description = objectProvider.FormatDescription(
                "do not call any method",
                "call",
                "call any"
            );
            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> DependOnAny(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<T> Filter(IEnumerable<T> objects, Architecture architecture)
            {
                var types = objectProvider.GetObjects(architecture);
                return objects.Where(obj => obj.GetTypeDependencies().Intersect(types).Any());
            }

            var description = objectProvider.FormatDescription(
                "depend on any of no types (impossible)",
                "depend on",
                "depend on any"
            );
            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> FollowCustomPredicate(
            Func<T, bool> predicate,
            string description
        )
        {
            return new SimplePredicate<T>(predicate, description);
        }

        public static IPredicate<T> OnlyDependOn(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<T> Filter(IEnumerable<T> objects, Architecture architecture)
            {
                var types = objectProvider.GetObjects(architecture);
                return objects.Where(obj =>
                    obj.GetTypeDependencies(architecture).Except(types).IsNullOrEmpty()
                );
            }

            var description = objectProvider.FormatDescription(
                "depend on no types",
                "only depend on",
                "only depend on"
            );
            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> HaveAnyAttributes(IObjectProvider<Attribute> objectProvider)
        {
            IEnumerable<T> Filter(IEnumerable<T> objects, Architecture architecture)
            {
                var attributes = objectProvider.GetObjects(architecture);
                return objects.Where(obj => obj.Attributes.Intersect(attributes).Any());
            }

            var description = objectProvider.FormatDescription(
                "have any of no attributes (always empty)",
                "have",
                "have any"
            );
            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> OnlyHaveAttributes(IObjectProvider<Attribute> objectProvider)
        {
            IEnumerable<T> Filter(IEnumerable<T> objects, Architecture architecture)
            {
                var attributes = objectProvider.GetObjects(architecture);
                return objects.Where(obj => !obj.Attributes.Except(attributes).Any());
            }

            var description = objectProvider.FormatDescription(
                "have no attributes",
                "only have",
                "only have"
            );
            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> HaveAnyAttributesWithArguments(
            IEnumerable<object> argumentValues
        )
        {
            var argumentValueList = argumentValues as IList<object> ?? argumentValues.ToList();
            IEnumerable<T> Predicate(IEnumerable<T> ruleTypes, Architecture architecture)
            {
                var resolvedArgumentValueList = argumentValueList
                    .ResolveAttributeArguments(architecture)
                    .ToList();
                return ruleTypes.Where(ruleType =>
                    ruleType.AttributeInstances.Any(instance =>
                    {
                        var instanceArgumentValues = instance
                            .GetAllAttributeArgumentValues()
                            .ToList();
                        return resolvedArgumentValueList.All(instanceArgumentValues.Contains);
                    })
                );
            }
            var description = argumentValueList.FormatDescription(
                "have any attributes",
                "have any attributes with argument",
                "have any attributes with arguments"
            );
            return new ArchitecturePredicate<T>(Predicate, description);
        }

        public static IPredicate<T> HaveAttributeWithArguments(
            string attributeFullName,
            [NotNull] Func<Architecture, Attribute> getAttribute,
            IEnumerable<object> argumentValues
        )
        {
            var argumentValueList = argumentValues as IList<object> ?? argumentValues.ToList();
            IEnumerable<T> Predicate(IEnumerable<T> ruleTypes, Architecture architecture)
            {
                var resolvedAttribute = getAttribute(architecture);
                var resolvedArgumentValueList = argumentValueList
                    .ResolveAttributeArguments(architecture)
                    .ToList();
                return ruleTypes.Where(ruleType =>
                    ruleType
                        .AttributeInstances.Where(instance =>
                            instance.Type.Equals(resolvedAttribute)
                        )
                        .Any(instance =>
                        {
                            var instanceArgumentValues = instance
                                .GetAllAttributeArgumentValues()
                                .ToList();
                            return resolvedArgumentValueList.All(instanceArgumentValues.Contains);
                        })
                );
            }
            var description = argumentValueList.FormatDescription(
                $"have attribute \"{attributeFullName}\"",
                $"have attribute \"{attributeFullName}\" with argument",
                $"have attribute \"{attributeFullName}\" with arguments"
            );
            return new ArchitecturePredicate<T>(Predicate, description);
        }

        public static IPredicate<T> HaveAnyAttributesWithNamedArguments(
            IEnumerable<(string, object)> namedArguments
        )
        {
            var namedArgumentList =
                namedArguments as IList<(string, object)> ?? namedArguments.ToList();
            IEnumerable<T> Predicate(IEnumerable<T> ruleTypes, Architecture architecture)
            {
                var resolvedNamedArgumentList = namedArgumentList
                    .ResolveNamedAttributeArgumentTuples(architecture)
                    .ToList();
                return ruleTypes.Where(ruleType =>
                    ruleType.AttributeInstances.Any(instance =>
                    {
                        var attArguments = instance.GetAllNamedAttributeArgumentTuples().ToList();
                        return resolvedNamedArgumentList.All(attArguments.Contains);
                    })
                );
            }
            var description = namedArgumentList.FormatDescription(
                "have any attributes",
                "have any attributes with named argument",
                "have any attributes with named arguments",
                elementDescription: arg => $"\"{arg.Item1}={arg.Item2}\""
            );
            return new ArchitecturePredicate<T>(Predicate, description);
        }

        public static IPredicate<T> HaveAttributeWithNamedArguments(
            string attributeFullName,
            [NotNull] Func<Architecture, Attribute> getAttribute,
            IEnumerable<(string, object)> namedArguments
        )
        {
            var namedArgumentList =
                namedArguments as IList<(string, object)> ?? namedArguments.ToList();
            IEnumerable<T> Predicate(IEnumerable<T> ruleTypes, Architecture architecture)
            {
                var resolvedAttribute = getAttribute(architecture);
                var resolvedNamedArgumentList = namedArgumentList
                    .ResolveNamedAttributeArgumentTuples(architecture)
                    .ToList();
                return ruleTypes.Where(ruleType =>
                    ruleType
                        .AttributeInstances.Where(instance =>
                            instance.Type.Equals(resolvedAttribute)
                        )
                        .Any(instance =>
                        {
                            var attArguments = instance
                                .GetAllNamedAttributeArgumentTuples()
                                .ToList();
                            return resolvedNamedArgumentList.All(attArguments.Contains);
                        })
                );
            }
            var description = namedArgumentList.FormatDescription(
                $"have attribute \"{attributeFullName}\"",
                $"have attribute \"{attributeFullName}\" with named argument",
                $"have attribute \"{attributeFullName}\" with named arguments",
                elementDescription: arg => $"\"{arg.Item1}={arg.Item2}\""
            );
            return new ArchitecturePredicate<T>(Predicate, description);
        }

        public static IPredicate<T> HaveName(string name)
        {
            return new SimplePredicate<T>(
                obj => obj.NameEquals(name),
                "have name \"" + name + "\""
            );
        }

        public static IPredicate<T> HaveNameMatching(string pattern)
        {
            return new SimplePredicate<T>(
                obj => obj.NameMatches(pattern),
                "have name matching \"" + pattern + "\""
            );
        }

        public static IPredicate<T> HaveNameStartingWith(string pattern)
        {
            return new SimplePredicate<T>(
                obj => obj.NameStartsWith(pattern),
                "have name starting with \"" + pattern + "\""
            );
        }

        public static IPredicate<T> HaveNameEndingWith(string pattern)
        {
            return new SimplePredicate<T>(
                obj => obj.NameEndsWith(pattern),
                "have name ending with \"" + pattern + "\""
            );
        }

        public static IPredicate<T> HaveNameContaining(string pattern)
        {
            return new SimplePredicate<T>(
                obj => obj.NameContains(pattern),
                "have name containing \"" + pattern + "\""
            );
        }

        public static IPredicate<T> HaveFullName(string fullName)
        {
            return new SimplePredicate<T>(
                obj => obj.FullNameEquals(fullName),
                "have full name \"" + fullName + "\""
            );
        }

        public static IPredicate<T> HaveFullNameMatching(string pattern)
        {
            return new SimplePredicate<T>(
                obj => obj.FullNameMatches(pattern),
                "have full name matching \"" + pattern + "\""
            );
        }

        public static IPredicate<T> HaveFullNameStartingWith(string pattern)
        {
            return new SimplePredicate<T>(
                obj => obj.FullNameStartsWith(pattern),
                "have full name starting with \"" + pattern + "\""
            );
        }

        public static IPredicate<T> HaveFullNameEndingWith(string pattern)
        {
            return new SimplePredicate<T>(
                obj => obj.FullNameEndsWith(pattern),
                "have full name ending with \"" + pattern + "\""
            );
        }

        public static IPredicate<T> HaveFullNameContaining(string pattern)
        {
            return new SimplePredicate<T>(
                obj => obj.FullNameContains(pattern),
                "have full name containing \"" + pattern + "\""
            );
        }

        public static IPredicate<T> HaveAssemblyQualifiedName(string assemblyQualifiedName)
        {
            return new SimplePredicate<T>(
                obj => obj.AssemblyQualifiedNameEquals(assemblyQualifiedName),
                "have assembly qualified name \"" + assemblyQualifiedName + "\""
            );
        }

        public static IPredicate<T> HaveAssemblyQualifiedNameMatching(string pattern)
        {
            return new SimplePredicate<T>(
                obj => obj.AssemblyQualifiedNameMatches(pattern),
                "have assembly qualified name matching \"" + pattern + "\""
            );
        }

        public static IPredicate<T> HaveAssemblyQualifiedNameStartingWith(string pattern)
        {
            return new SimplePredicate<T>(
                obj => obj.AssemblyQualifiedNameStartsWith(pattern),
                "have assembly qualified name starting with \"" + pattern + "\""
            );
        }

        public static IPredicate<T> HaveAssemblyQualifiedNameEndingWith(string pattern)
        {
            return new SimplePredicate<T>(
                obj => obj.AssemblyQualifiedNameEndsWith(pattern),
                "have assembly qualified name ending with \"" + pattern + "\""
            );
        }

        public static IPredicate<T> HaveAssemblyQualifiedNameContaining(string pattern)
        {
            return new SimplePredicate<T>(
                obj => obj.AssemblyQualifiedNameContains(pattern),
                "have assembly qualified name containing \"" + pattern + "\""
            );
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
            return new SimplePredicate<T>(
                obj => obj.Visibility == ProtectedInternal,
                "are protected internal"
            );
        }

        public static IPredicate<T> ArePrivateProtected()
        {
            return new SimplePredicate<T>(
                obj => obj.Visibility == PrivateProtected,
                "are private protected"
            );
        }

        //Negations

        public static IPredicate<T> AreNot(IObjectProvider<ICanBeAnalyzed> objectProvider)
        {
            IEnumerable<T> Filter(IEnumerable<T> objects, Architecture architecture)
            {
                return objects.Except(objectProvider.GetObjects(architecture).OfType<T>());
            }

            var description = objectProvider.FormatDescription(
                "are not any of no objects (always true)",
                "are not",
                "are not"
            );
            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> DoNotCallAny(IObjectProvider<MethodMember> objectProvider)
        {
            IEnumerable<T> Filter(IEnumerable<T> objects, Architecture architecture)
            {
                var methods = objectProvider.GetObjects(architecture);
                return objects.Where(obj => !obj.GetCalledMethods().Intersect(methods).Any());
            }

            var description = objectProvider.FormatDescription(
                "do not call any of no methods (always true)",
                "do not call",
                "do not call any"
            );
            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> DoNotDependOnAny(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<T> Filter(IEnumerable<T> objects, Architecture architecture)
            {
                var types = objectProvider.GetObjects(architecture);
                return objects.Where(obj =>
                    !obj.GetTypeDependencies(architecture).Intersect(types).Any()
                );
            }

            var description = objectProvider.FormatDescription(
                "do not depend on any of no types (always true)",
                "do not depend on",
                "do not depend on any"
            );
            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> DoNotHaveAnyAttributes(
            IObjectProvider<Attribute> objectProvider
        )
        {
            IEnumerable<T> Filter(IEnumerable<T> objects, Architecture architecture)
            {
                var types = objectProvider.GetObjects(architecture);
                return objects.Where(obj => !obj.Attributes.Intersect(types).Any());
            }

            var description = objectProvider.FormatDescription(
                "do not have any of no attributes (always true)",
                "do not have",
                "do not have any"
            );
            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> DoNotHaveAnyAttributesWithArguments(
            IEnumerable<object> argumentValues
        )
        {
            var argumentValueList = argumentValues as IList<object> ?? argumentValues.ToList();
            IEnumerable<T> Predicate(IEnumerable<T> ruleTypes, Architecture architecture)
            {
                var resolvedArgumentValueList = argumentValueList
                    .ResolveAttributeArguments(architecture)
                    .ToList();
                foreach (var ruleType in ruleTypes)
                {
                    var attributeArguments = ruleType.GetAllAttributeArgumentValues().ToList();
                    if (!resolvedArgumentValueList.Any(attributeArguments.Contains))
                    {
                        yield return ruleType;
                    }
                }
            }
            var description = argumentValueList.FormatDescription(
                "do not have any attributes with any of no arguments (always true)",
                "do not have any attributes with argument",
                "do not have any attributes with arguments"
            );
            return new ArchitecturePredicate<T>(Predicate, description);
        }

        public static IPredicate<T> DoNotHaveAttributeWithArguments(
            string attributeFullName,
            [NotNull] Func<Architecture, Attribute> getAttribute,
            IEnumerable<object> argumentValues
        )
        {
            var argumentValueList = argumentValues as IList<object> ?? argumentValues.ToList();

            IEnumerable<T> Predicate(IEnumerable<T> ruleTypes, Architecture architecture)
            {
                var resolvedAttribute = getAttribute(architecture);
                var resolvedArgumentValueList = argumentValueList
                    .ResolveAttributeArguments(architecture)
                    .ToList();
                foreach (var ruleType in ruleTypes)
                {
                    var attributeArguments = ruleType
                        .GetAllAttributeArgumentValues(resolvedAttribute)
                        .ToList();
                    if (!resolvedArgumentValueList.Any(attributeArguments.Contains))
                    {
                        yield return ruleType;
                    }
                }
            }

            var description = argumentValueList.FormatDescription(
                $"do not have attribute \"{attributeFullName}\" with any of no arguments (always true)",
                $"do not have attribute \"{attributeFullName}\" with argument",
                $"do not have attribute \"{attributeFullName}\" with arguments"
            );
            return new ArchitecturePredicate<T>(Predicate, description);
        }

        public static IPredicate<T> DoNotHaveAnyAttributesWithNamedArguments(
            IEnumerable<(string, object)> namedArguments
        )
        {
            var namedArgumentList =
                namedArguments as IList<(string, object)> ?? namedArguments.ToList();
            IEnumerable<T> Condition(IEnumerable<T> ruleTypes, Architecture architecture)
            {
                var resolvedNamedArgumentList = namedArgumentList
                    .ResolveNamedAttributeArgumentTuples(architecture)
                    .ToList();
                foreach (var ruleType in ruleTypes)
                {
                    var attArguments = ruleType.GetAllNamedAttributeArgumentTuples().ToList();
                    if (!resolvedNamedArgumentList.Any(attArguments.Contains))
                    {
                        yield return ruleType;
                    }
                }
            }

            var description = namedArgumentList.FormatDescription(
                "do not have any attributes with any of no named arguments (always true)",
                "do not have any attributes with named argument",
                "do not have any attributes with named arguments",
                elementDescription: arg => $"\"{arg.Item1}={arg.Item2}\""
            );
            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> DoNotHaveAttributeWithNamedArguments(
            string attributeFullName,
            [NotNull] Func<Architecture, Attribute> getAttribute,
            IEnumerable<(string, object)> namedArguments
        )
        {
            var namedArgumentList =
                namedArguments as IList<(string, object)> ?? namedArguments.ToList();
            IEnumerable<T> Condition(IEnumerable<T> ruleTypes, Architecture architecture)
            {
                var resolvedAttribute = getAttribute(architecture);
                var resolvedNamedArgumentList = namedArgumentList
                    .ResolveNamedAttributeArgumentTuples(architecture)
                    .ToList();
                foreach (var ruleType in ruleTypes)
                {
                    var attArguments = ruleType
                        .GetAllNamedAttributeArgumentTuples(resolvedAttribute)
                        .ToList();
                    if (!resolvedNamedArgumentList.Any(attArguments.Contains))
                    {
                        yield return ruleType;
                    }
                }
            }

            var description = namedArgumentList.FormatDescription(
                $"do not have attribute \"{attributeFullName}\" with any of no named arguments (always true)",
                $"do not have attribute \"{attributeFullName}\" with named argument",
                $"do not have attribute \"{attributeFullName}\" with named arguments",
                elementDescription: arg => $"\"{arg.Item1}={arg.Item2}\""
            );
            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> DoNotHaveName(string name)
        {
            return new SimplePredicate<T>(
                obj => !obj.NameEquals(name),
                $"do not have name \"{name}\""
            );
        }

        public static IPredicate<T> DoNotHaveNameMatching(string pattern)
        {
            return new SimplePredicate<T>(
                obj => !obj.NameMatches(pattern),
                $"do not have name matching \"{pattern}\""
            );
        }

        public static IPredicate<T> DoNotHaveNameStartingWith(string pattern)
        {
            return new SimplePredicate<T>(
                obj => !obj.NameStartsWith(pattern),
                "do not have name starting with \"" + pattern + "\""
            );
        }

        public static IPredicate<T> DoNotHaveNameEndingWith(string pattern)
        {
            return new SimplePredicate<T>(
                obj => !obj.NameEndsWith(pattern),
                "do not have name ending with \"" + pattern + "\""
            );
        }

        public static IPredicate<T> DoNotHaveNameContaining(string pattern)
        {
            return new SimplePredicate<T>(
                obj => !obj.NameContains(pattern),
                "do not have name containing \"" + pattern + "\""
            );
        }

        public static IPredicate<T> DoNotHaveFullName(string fullName)
        {
            return new SimplePredicate<T>(
                obj => !obj.FullNameEquals(fullName),
                "do not have full name \"" + fullName + "\""
            );
        }

        public static IPredicate<T> DoNotHaveFullNameMatching(string pattern)
        {
            return new SimplePredicate<T>(
                obj => !obj.FullNameMatches(pattern),
                "do not have full name matching \"" + pattern + "\""
            );
        }

        public static IPredicate<T> DoNotHaveFullNameStartingWith(string pattern)
        {
            return new SimplePredicate<T>(
                obj => !obj.FullNameStartsWith(pattern),
                "do not have full name starting with \"" + pattern + "\""
            );
        }

        public static IPredicate<T> DoNotHaveFullNameEndingWith(string pattern)
        {
            return new SimplePredicate<T>(
                obj => !obj.FullNameEndsWith(pattern),
                "do not have full name ending with \"" + pattern + "\""
            );
        }

        public static IPredicate<T> DoNotHaveFullNameContaining(string pattern)
        {
            return new SimplePredicate<T>(
                obj => !obj.FullNameContains(pattern),
                "do not have full name containing \"" + pattern + "\""
            );
        }

        public static IPredicate<T> DoNotHaveAssemblyQualifiedName(string assemblyQualifiedName)
        {
            return new SimplePredicate<T>(
                obj => !obj.AssemblyQualifiedNameEquals(assemblyQualifiedName),
                "do not have assembly qualified name \"" + assemblyQualifiedName + "\""
            );
        }

        public static IPredicate<T> DoNotHaveAssemblyQualifiedNameMatching(string pattern)
        {
            return new SimplePredicate<T>(
                obj => !obj.AssemblyQualifiedNameMatches(pattern),
                "do not have assembly qualified name matching \"" + pattern + "\""
            );
        }

        public static IPredicate<T> DoNotHaveAssemblyQualifiedNameStartingWith(string pattern)
        {
            return new SimplePredicate<T>(
                obj => !obj.AssemblyQualifiedNameStartsWith(pattern),
                "do not have assembly qualified name starting with \"" + pattern + "\""
            );
        }

        public static IPredicate<T> DoNotHaveAssemblyQualifiedNameEndingWith(string pattern)
        {
            return new SimplePredicate<T>(
                obj => !obj.AssemblyQualifiedNameEndsWith(pattern),
                "do not have assembly qualified name ending with \"" + pattern + "\""
            );
        }

        public static IPredicate<T> DoNotHaveAssemblyQualifiedNameContaining(string pattern)
        {
            return new SimplePredicate<T>(
                obj => !obj.AssemblyQualifiedNameContains(pattern),
                "do not have assembly qualified name containing \"" + pattern + "\""
            );
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
            return new SimplePredicate<T>(
                obj => obj.Visibility != ProtectedInternal,
                "are not protected internal"
            );
        }

        public static IPredicate<T> AreNotPrivateProtected()
        {
            return new SimplePredicate<T>(
                obj => obj.Visibility != PrivateProtected,
                "are not private protected"
            );
        }
    }
}
