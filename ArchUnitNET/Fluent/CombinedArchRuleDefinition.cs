﻿using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax;
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
    public class CombinedArchRuleDefinition
    {
        private readonly IArchRuleCreator _currentArchRuleCreator;
        private readonly LogicalConjunction _logicalConjunction;

        public CombinedArchRuleDefinition(IArchRuleCreator currentArchRuleCreator,
            LogicalConjunction logicalConjunction)
        {
            _currentArchRuleCreator = currentArchRuleCreator;
            _logicalConjunction = logicalConjunction;
        }

        public GivenTypes Types()
        {
            var combinedRuleCreator = new CombinedArchRuleCreator<IType>(_currentArchRuleCreator, _logicalConjunction,
                ObjectProviderDefinition.Types);
            return new GivenTypes(combinedRuleCreator);
        }

        public GivenAttributes Attributes()
        {
            var combinedRuleCreator = new CombinedArchRuleCreator<Attribute>(_currentArchRuleCreator,
                _logicalConjunction, ObjectProviderDefinition.Attributes);
            return new GivenAttributes(combinedRuleCreator);
        }

        public GivenClasses Classes()
        {
            var combinedRuleCreator = new CombinedArchRuleCreator<Class>(_currentArchRuleCreator, _logicalConjunction,
                ObjectProviderDefinition.Classes);
            return new GivenClasses(combinedRuleCreator);
        }

        public GivenInterfaces Interfaces()
        {
            var combinedRuleCreator = new CombinedArchRuleCreator<Interface>(_currentArchRuleCreator,
                _logicalConjunction, ObjectProviderDefinition.Interfaces);
            return new GivenInterfaces(combinedRuleCreator);
        }

        public GivenMembers Members()
        {
            var combinedRuleCreator = new CombinedArchRuleCreator<IMember>(_currentArchRuleCreator, _logicalConjunction,
                ObjectProviderDefinition.Members);
            return new GivenMembers(combinedRuleCreator);
        }

        public GivenFieldMembers FieldMembers()
        {
            var combinedRuleCreator = new CombinedArchRuleCreator<FieldMember>(_currentArchRuleCreator,
                _logicalConjunction, ObjectProviderDefinition.FieldMembers);
            return new GivenFieldMembers(combinedRuleCreator);
        }

        public GivenMethodMembers MethodMembers()
        {
            var combinedRuleCreator = new CombinedArchRuleCreator<MethodMember>(_currentArchRuleCreator,
                _logicalConjunction, ObjectProviderDefinition.MethodMembers);
            return new GivenMethodMembers(combinedRuleCreator);
        }

        public GivenPropertyMembers PropertyMembers()
        {
            var combinedRuleCreator = new CombinedArchRuleCreator<PropertyMember>(_currentArchRuleCreator,
                _logicalConjunction, ObjectProviderDefinition.PropertyMembers);
            return new GivenPropertyMembers(combinedRuleCreator);
        }
    }
}