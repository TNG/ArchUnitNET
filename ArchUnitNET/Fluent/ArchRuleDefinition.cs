//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax.Elements.Members;
using ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers;
using ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers;
using ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers;
using ArchUnitNET.Fluent.Syntax.Elements.Types;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Classes;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces;

namespace ArchUnitNET.Fluent
{
    public static class ArchRuleDefinition
    {
        public static GivenTypes Types(bool includeReferenced = false)
        {
            var ruleCreator = includeReferenced
                ? new ArchRuleCreator<IType>(BasicObjectProviderDefinition.TypesIncludingReferenced)
                : new ArchRuleCreator<IType>(BasicObjectProviderDefinition.Types);
            return new GivenTypes(ruleCreator);
        }

        public static GivenAttributes Attributes(bool includeReferenced = false)
        {
            var ruleCreator = includeReferenced
                ? new ArchRuleCreator<Attribute>(
                    BasicObjectProviderDefinition.AttributesIncludingReferenced
                )
                : new ArchRuleCreator<Attribute>(BasicObjectProviderDefinition.Attributes);
            return new GivenAttributes(ruleCreator);
        }

        public static GivenClasses Classes(bool includeReferenced = false)
        {
            var ruleCreator = includeReferenced
                ? new ArchRuleCreator<Class>(
                    BasicObjectProviderDefinition.ClassesIncludingReferenced
                )
                : new ArchRuleCreator<Class>(BasicObjectProviderDefinition.Classes);
            return new GivenClasses(ruleCreator);
        }

        public static GivenInterfaces Interfaces(bool includeReferenced = false)
        {
            var ruleCreator = includeReferenced
                ? new ArchRuleCreator<Interface>(
                    BasicObjectProviderDefinition.InterfacesIncludingReferenced
                )
                : new ArchRuleCreator<Interface>(BasicObjectProviderDefinition.Interfaces);
            return new GivenInterfaces(ruleCreator);
        }

        public static GivenMembers Members()
        {
            var ruleCreator = new ArchRuleCreator<IMember>(BasicObjectProviderDefinition.Members);
            return new GivenMembers(ruleCreator);
        }

        public static GivenFieldMembers FieldMembers()
        {
            var ruleCreator = new ArchRuleCreator<FieldMember>(
                BasicObjectProviderDefinition.FieldMembers
            );
            return new GivenFieldMembers(ruleCreator);
        }

        public static GivenMethodMembers MethodMembers()
        {
            var ruleCreator = new ArchRuleCreator<MethodMember>(
                BasicObjectProviderDefinition.MethodMembers
            );
            return new GivenMethodMembers(ruleCreator);
        }

        public static GivenPropertyMembers PropertyMembers()
        {
            var ruleCreator = new ArchRuleCreator<PropertyMember>(
                BasicObjectProviderDefinition.PropertyMembers
            );
            return new GivenPropertyMembers(ruleCreator);
        }
    }
}
