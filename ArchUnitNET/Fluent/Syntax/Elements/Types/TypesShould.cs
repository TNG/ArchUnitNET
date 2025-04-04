//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using System.IO;
using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;
using Assembly = System.Reflection.Assembly;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public class TypesShould<TRuleTypeShouldConjunction, TRuleType>
        : ObjectsShould<TRuleTypeShouldConjunction, TRuleType>,
            IComplexTypeConditions<TRuleTypeShouldConjunction, TRuleType>
        where TRuleType : IType
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
    {
        // ReSharper disable once MemberCanBeProtected.Global
        public TypesShould(IArchRuleCreator<TRuleType> ruleCreator)
            : base(ruleCreator) { }

        public TRuleTypeShouldConjunction Be(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddCondition(TypeConditionsDefinition<TRuleType>.Be(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction Be(IEnumerable<Type> types)
        {
            _ruleCreator.AddCondition(TypeConditionsDefinition<TRuleType>.Be(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use BeAssignableTo(Types().That().HaveFullName()) instead"
        )]
        public TRuleTypeShouldConjunction BeAssignableTo(
            string pattern,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.BeAssignableTo(pattern, useRegularExpressions)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use BeAssignableTo(Types().That().HaveFullName()) instead"
        )]
        public TRuleTypeShouldConjunction BeAssignableTo(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.BeAssignableTo(patterns, useRegularExpressions)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeAssignableTo(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.BeAssignableTo(firstType, moreTypes)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeAssignableTo(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.BeAssignableTo(firstType, moreTypes)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeAssignableTo(IObjectProvider<IType> types)
        {
            _ruleCreator.AddCondition(TypeConditionsDefinition<TRuleType>.BeAssignableTo(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeAssignableTo(IEnumerable<IType> types)
        {
            _ruleCreator.AddCondition(TypeConditionsDefinition<TRuleType>.BeAssignableTo(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeAssignableTo(IEnumerable<Type> types)
        {
            _ruleCreator.AddCondition(TypeConditionsDefinition<TRuleType>.BeAssignableTo(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeNestedIn(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.BeNestedIn(firstType, moreTypes)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeNestedIn(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.BeNestedIn(firstType, moreTypes)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeNestedIn(IObjectProvider<IType> types)
        {
            _ruleCreator.AddCondition(TypeConditionsDefinition<TRuleType>.BeNestedIn(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeNestedIn(IEnumerable<IType> types)
        {
            _ruleCreator.AddCondition(TypeConditionsDefinition<TRuleType>.BeNestedIn(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeNestedIn(IEnumerable<Type> types)
        {
            _ruleCreator.AddCondition(TypeConditionsDefinition<TRuleType>.BeNestedIn(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeValueTypes()
        {
            _ruleCreator.AddCondition(TypeConditionsDefinition<TRuleType>.BeValueTypes());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeEnums()
        {
            _ruleCreator.AddCondition(TypeConditionsDefinition<TRuleType>.BeEnums());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeStructs()
        {
            _ruleCreator.AddCondition(TypeConditionsDefinition<TRuleType>.BeStructs());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update."
        )]
        public TRuleTypeShouldConjunction ImplementInterface(
            string pattern,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.ImplementInterface(
                    pattern,
                    useRegularExpressions
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ImplementInterface(Interface intf)
        {
            _ruleCreator.AddCondition(TypeConditionsDefinition<TRuleType>.ImplementInterface(intf));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ImplementInterface(Type intf)
        {
            _ruleCreator.AddCondition(TypeConditionsDefinition<TRuleType>.ImplementInterface(intf));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Either ResideInNamespace() without the useRegularExpressions parameter or ResideInNamespaceMatching() should be used"
        )]
        public TRuleTypeShouldConjunction ResideInNamespace(
            string pattern,
            bool useRegularExpressions
        )
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.ResideInNamespace(
                    pattern,
                    useRegularExpressions
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ResideInNamespace(string fullName)
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.ResideInNamespace(fullName)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ResideInNamespaceMatching(string pattern)
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.ResideInNamespaceMatching(pattern)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Either ResideInAssembly() without the useRegularExpressions parameter or ResideInAssemblyMatching() should be used"
        )]
        public TRuleTypeShouldConjunction ResideInAssembly(
            string pattern,
            bool useRegularExpressions
        )
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.ResideInAssembly(pattern, useRegularExpressions)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ResideInAssembly(string fullName)
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.ResideInAssembly(fullName)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ResideInAssemblyMatching(string pattern)
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.ResideInAssemblyMatching(pattern)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ResideInAssembly(
            Assembly assembly,
            params Assembly[] moreAssemblies
        )
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.ResideInAssembly(assembly, moreAssemblies)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ResideInAssembly(
            Domain.Assembly assembly,
            params Domain.Assembly[] moreAssemblies
        )
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.ResideInAssembly(assembly, moreAssemblies)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HavePropertyMemberWithName(string name)
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.HavePropertyMemberWithName(name)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFieldMemberWithName(string name)
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.HaveFieldMemberWithName(name)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveMethodMemberWithName(string name)
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.HaveMethodMemberWithName(name)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveMemberWithName(string name)
        {
            _ruleCreator.AddCondition(TypeConditionsDefinition<TRuleType>.HaveMemberWithName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeNested()
        {
            _ruleCreator.AddCondition(TypeConditionsDefinition<TRuleType>.BeNested());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        //Relation Conditions

        public ShouldRelateToTypesThat<
            TRuleTypeShouldConjunction,
            IType,
            TRuleType
        > BeAssignableToTypesThat()
        {
            _ruleCreator.BeginComplexCondition(
                ArchRuleDefinition.Types(true),
                TypeConditionsDefinition<TRuleType>.BeAssignableToTypesThat()
            );
            return new ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType>(
                _ruleCreator
            );
        }

        //Negations


        public TRuleTypeShouldConjunction NotBe(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.NotBe(firstType, moreTypes)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBe(IEnumerable<Type> types)
        {
            _ruleCreator.AddCondition(TypeConditionsDefinition<TRuleType>.NotBe(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use NotBeAssignableTo(Types().That().HaveFullName()) instead"
        )]
        public TRuleTypeShouldConjunction NotBeAssignableTo(
            string pattern,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.NotBeAssignableTo(
                    pattern,
                    useRegularExpressions
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use NotBeAssignableTo(Types().That().HaveFullName()) instead"
        )]
        public TRuleTypeShouldConjunction NotBeAssignableTo(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.NotBeAssignableTo(
                    patterns,
                    useRegularExpressions
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeAssignableTo(
            IType firstType,
            params IType[] moreTypes
        )
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.NotBeAssignableTo(firstType, moreTypes)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeAssignableTo(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.NotBeAssignableTo(firstType, moreTypes)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeAssignableTo(IObjectProvider<IType> types)
        {
            _ruleCreator.AddCondition(TypeConditionsDefinition<TRuleType>.NotBeAssignableTo(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeAssignableTo(IEnumerable<IType> types)
        {
            _ruleCreator.AddCondition(TypeConditionsDefinition<TRuleType>.NotBeAssignableTo(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeAssignableTo(IEnumerable<Type> types)
        {
            _ruleCreator.AddCondition(TypeConditionsDefinition<TRuleType>.NotBeAssignableTo(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeValueTypes()
        {
            _ruleCreator.AddCondition(TypeConditionsDefinition<TRuleType>.NotBeValueTypes());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeEnums()
        {
            _ruleCreator.AddCondition(TypeConditionsDefinition<TRuleType>.NotBeEnums());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeStructs()
        {
            _ruleCreator.AddCondition(TypeConditionsDefinition<TRuleType>.NotBeStructs());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update."
        )]
        public TRuleTypeShouldConjunction NotImplementInterface(
            string pattern,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.NotImplementInterface(
                    pattern,
                    useRegularExpressions
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotImplementInterface(Interface intf)
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.NotImplementInterface(intf)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotImplementInterface(Type intf)
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.NotImplementInterface(intf)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Either NotResideInNamespace() without the useRegularExpressions parameter or NotResideInNamespaceMatching() should be used"
        )]
        public TRuleTypeShouldConjunction NotResideInNamespace(
            string pattern,
            bool useRegularExpressions
        )
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.NotResideInNamespace(
                    pattern,
                    useRegularExpressions
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotResideInNamespace(string fullName)
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.NotResideInNamespace(fullName)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotResideInNamespaceMatching(string pattern)
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.NotResideInNamespaceMatching(pattern)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Either NotResideInAssembly() without the useRegularExpressions parameter or NotResideInAssemblyMatching() should be used"
        )]
        public TRuleTypeShouldConjunction NotResideInAssembly(
            string pattern,
            bool useRegularExpressions
        )
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.NotResideInAssembly(
                    pattern,
                    useRegularExpressions
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotResideInAssembly(string fullName)
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.NotResideInAssembly(fullName)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotResideInAssemblyMatching(string pattern)
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.NotResideInAssemblyMatching(pattern)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotResideInAssembly(
            Assembly assembly,
            params Assembly[] moreAssemblies
        )
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.NotResideInAssembly(assembly, moreAssemblies)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotResideInAssembly(
            Domain.Assembly assembly,
            params Domain.Assembly[] moreAssemblies
        )
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.NotResideInAssembly(assembly, moreAssemblies)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHavePropertyMemberWithName(string name)
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.NotHavePropertyMemberWithName(name)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveFieldMemberWithName(string name)
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.NotHaveFieldMemberWithName(name)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveMethodMemberWithName(string name)
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.NotHaveMethodMemberWithName(name)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveMemberWithName(string name)
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.NotHaveMemberWithName(name)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeNested()
        {
            _ruleCreator.AddCondition(TypeConditionsDefinition<TRuleType>.NotBeNested());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AdhereToPlantUmlDiagram(string file)
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.AdhereToPlantUmlDiagram(file)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AdhereToPlantUmlDiagram(Stream stream)
        {
            _ruleCreator.AddCondition(
                TypeConditionsDefinition<TRuleType>.AdhereToPlantUmlDiagram(stream)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        //Relation Condition Negations

        public ShouldRelateToTypesThat<
            TRuleTypeShouldConjunction,
            IType,
            TRuleType
        > NotBeAssignableToTypesThat()
        {
            _ruleCreator.BeginComplexCondition(
                ArchRuleDefinition.Types(true),
                TypeConditionsDefinition<TRuleType>.NotBeAssignableToTypesThat()
            );
            return new ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType>(
                _ruleCreator
            );
        }
    }
}
