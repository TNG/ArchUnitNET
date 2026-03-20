using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using JetBrains.Annotations;
using Mono.Cecil;

namespace ArchUnitNET.Loader.LoadTasks
{
    /// <summary>
    /// Creates field, property, and method members from the Cecil
    /// <see cref="TypeDefinition"/> and adds them to the domain type.
    /// Returns a <see cref="MemberData"/> containing method pairs and
    /// a mapping from getter/setter <see cref="MethodDefinition"/>s to
    /// their <see cref="PropertyMember"/>s.
    /// </summary>
    internal static class AddMembers
    {
        internal static MemberData Execute(
            ITypeInstance<IType> typeInstance,
            TypeDefinition typeDefinition,
            DomainResolver domainResolver
        )
        {
            var methodPairs = new List<(MethodMember Member, MethodDefinition Definition)>();
            var propertyByAccessor = new Dictionary<MethodDefinition, PropertyMember>();
            var members = CreateMembers(
                typeInstance,
                typeDefinition,
                domainResolver,
                methodPairs,
                propertyByAccessor
            );
            typeInstance.Type.Members.AddRange(members);
            return new MemberData(methodPairs, propertyByAccessor);
        }

        [NotNull]
        private static IEnumerable<IMember> CreateMembers(
            ITypeInstance<IType> typeInstance,
            [NotNull] TypeDefinition typeDefinition,
            DomainResolver domainResolver,
            List<(MethodMember Member, MethodDefinition Definition)> methodPairs,
            Dictionary<MethodDefinition, PropertyMember> propertyByAccessor
        )
        {
            var fieldMembers = typeDefinition
                .Fields.Where(fieldDefinition => !fieldDefinition.IsBackingField())
                .Select(fieldDef =>
                    (IMember)domainResolver.GetOrCreateFieldMember(typeInstance.Type, fieldDef)
                );

            var propertyMembers = typeDefinition.Properties.Select(propDef =>
            {
                var propertyMember = CreatePropertyMember(typeInstance, propDef, domainResolver);
                if (propDef.GetMethod != null)
                {
                    propertyByAccessor[propDef.GetMethod] = propertyMember;
                }

                if (propDef.SetMethod != null)
                {
                    propertyByAccessor[propDef.SetMethod] = propertyMember;
                }

                return (IMember)propertyMember;
            });

            var methodMembers = typeDefinition.Methods.Select(method =>
            {
                var member = domainResolver
                    .GetOrCreateMethodMemberFromMethodReference(typeInstance, method)
                    .Member;
                methodPairs.Add((member, method));
                return (IMember)member;
            });

            return fieldMembers
                .Concat(propertyMembers)
                .Concat(methodMembers)
                .Where(member => !member.IsCompilerGenerated);
        }

        [NotNull]
        private static PropertyMember CreatePropertyMember(
            ITypeInstance<IType> typeInstance,
            PropertyDefinition propertyDefinition,
            DomainResolver domainResolver
        )
        {
            var typeReference = propertyDefinition.PropertyType;
            var propertyType = domainResolver.GetOrCreateStubTypeInstanceFromTypeReference(
                typeReference
            );
            var isCompilerGenerated = propertyDefinition.IsCompilerGenerated();
            var isStatic =
                (propertyDefinition.SetMethod != null && propertyDefinition.SetMethod.IsStatic)
                || (propertyDefinition.GetMethod != null && propertyDefinition.GetMethod.IsStatic);
            var writeAccessor = GetPropertyWriteAccessor(propertyDefinition);
            return new PropertyMember(
                typeInstance.Type,
                propertyDefinition.Name,
                propertyDefinition.FullName,
                propertyType,
                isCompilerGenerated,
                isStatic,
                writeAccessor
            );
        }

        private static Writability GetPropertyWriteAccessor(
            [NotNull] PropertyDefinition propertyDefinition
        )
        {
            if (propertyDefinition.SetMethod == null)
            {
                return Writability.ReadOnly;
            }

            if (CheckPropertyHasInitSetterInNetStandardCompatibleWay(propertyDefinition))
            {
                return Writability.InitOnly;
            }

            return Writability.Writable;
        }

        private static bool CheckPropertyHasInitSetterInNetStandardCompatibleWay(
            PropertyDefinition propertyDefinition
        )
        {
            return propertyDefinition.SetMethod?.ReturnType.IsRequiredModifier == true
                && ((RequiredModifierType)propertyDefinition.SetMethod.ReturnType)
                    .ModifierType
                    .FullName == "System.Runtime.CompilerServices.IsExternalInit";
        }
    }

    /// <summary>
    /// Bundles the results of member creation: method (MethodMember, MethodDefinition) pairs
    /// and a mapping from getter/setter MethodDefinitions to their PropertyMembers.
    /// </summary>
    internal sealed class MemberData
    {
        public MemberData(
            IReadOnlyList<(MethodMember Member, MethodDefinition Definition)> methodPairs,
            IReadOnlyDictionary<MethodDefinition, PropertyMember> propertyByAccessor
        )
        {
            MethodPairs = methodPairs;
            PropertyByAccessor = propertyByAccessor;
        }

        public IReadOnlyList<(
            MethodMember Member,
            MethodDefinition Definition
        )> MethodPairs { get; }
        public IReadOnlyDictionary<MethodDefinition, PropertyMember> PropertyByAccessor { get; }
    }
}
