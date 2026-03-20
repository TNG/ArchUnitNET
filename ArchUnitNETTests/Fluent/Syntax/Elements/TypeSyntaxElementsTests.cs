using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNETTests.AssemblyTestHelper;
using ArchUnitNETTests.Domain.PlantUml;
using TestAssembly.Diagram.NoDependencies.Independent;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements
{
    // csharpier-ignore
    public class TypeSyntaxElementsTests
    {
        [Fact]
        public async Task BeSystemTypeTest()
        {
            var helper = new TypeAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = Types().That().Are(helper.RegularClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.Be(helper.RegularClassSystemType).AssertNoViolations(helper);
            should.Be(new List<System.Type> { helper.RegularClassSystemType }).AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Complex conditions");
            should.BeTypesThat().Are(helper.RegularClass).AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().Are(helper.RegularClassSystemType)).AssertNoViolations(helper);
            should.Be(Types().That().Are(new List<System.Type> { helper.RegularClassSystemType })).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = Types().That().Are(helper.OtherRegularClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.Be(helper.RegularClassSystemType).AssertOnlyViolations(helper);
            should.Be(new List<System.Type> { helper.RegularClassSystemType }).AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().Are(helper.RegularClassSystemType)).AssertOnlyViolations(helper);
            should.Be(Types().That().Are(new List<System.Type> { helper.RegularClassSystemType })).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Empty Arguments (Only Violations)");
            should = Types().That().Are(helper.RegularClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.Be(new List<System.Type>()).AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().Are(new List<System.Type>())).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Type not in architecture");
            should = Types().That().Are(helper.RegularClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.Be(typeof(AttributeNamespace.ClassWithoutAttributes)).AssertOnlyViolations(helper);
            should.Be(new List<System.Type> { typeof(AttributeNamespace.ClassWithoutAttributes) }).AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().Are(typeof(AttributeNamespace.ClassWithoutAttributes))).AssertOnlyViolations(helper);
            should.Be(Types().That().Are(new List<System.Type> { typeof(AttributeNamespace.ClassWithoutAttributes) })).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple arguments");
            should = Types().That().Are(helper.RegularClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.Be(helper.RegularClassSystemType, helper.OtherRegularClassSystemType).AssertNoViolations(helper);
            should.Be(new List<System.Type> { helper.RegularClassSystemType, helper.OtherRegularClassSystemType }).AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().Are(helper.RegularClassSystemType, helper.OtherRegularClassSystemType)).AssertNoViolations(helper);
            should.Be(Types().That().Are(new List<System.Type> { helper.RegularClassSystemType, helper.OtherRegularClassSystemType })).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Types().That().Are(helper.RegularClass, helper.OtherRegularClass).Should().Be(helper.RegularClassSystemType).AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotBeSystemTypeTest()
        {
            var helper = new TypeAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = Types().That().Are(helper.OtherRegularClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotBe(helper.RegularClassSystemType).AssertNoViolations(helper);
            should.NotBe(new List<System.Type> { helper.RegularClassSystemType }).AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().AreNot(helper.RegularClassSystemType)).AssertNoViolations(helper);
            should.Be(Types().That().AreNot(new List<System.Type> { helper.RegularClassSystemType })).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = Types().That().Are(helper.RegularClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotBe(helper.RegularClassSystemType).AssertOnlyViolations(helper);
            should.NotBe(new List<System.Type> { helper.RegularClassSystemType }).AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().AreNot(helper.RegularClassSystemType)).AssertOnlyViolations(helper);
            should.Be(Types().That().AreNot(new List<System.Type> { helper.RegularClassSystemType })).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Empty Arguments (No Violations)");
            should = Types().That().Are(helper.RegularClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotBe(new List<System.Type>()).AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().AreNot(new List<System.Type>())).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Type not in architecture");
            should = Types().That().Are(helper.RegularClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotBe(typeof(AttributeNamespace.ClassWithoutAttributes)).AssertNoViolations(helper);
            should.NotBe(new List<System.Type> { typeof(AttributeNamespace.ClassWithoutAttributes) }).AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().AreNot(typeof(AttributeNamespace.ClassWithoutAttributes))).AssertNoViolations(helper);
            should.Be(Types().That().AreNot(new List<System.Type> { typeof(AttributeNamespace.ClassWithoutAttributes) })).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Multiple arguments");
            should = Types().That().Are(helper.OtherRegularClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotBe(helper.RegularClassSystemType, helper.ClassWithPropertySystemType).AssertNoViolations(helper);
            should.NotBe(new List<System.Type> { helper.RegularClassSystemType, helper.ClassWithPropertySystemType }).AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().AreNot(helper.RegularClassSystemType, helper.ClassWithPropertySystemType)).AssertNoViolations(helper);
            should.Be(Types().That().AreNot(new List<System.Type> { helper.RegularClassSystemType, helper.ClassWithPropertySystemType })).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Types().That().Are(helper.RegularClass, helper.OtherRegularClass).Should().NotBe(helper.RegularClassSystemType).AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task BeAssignableToTest()
        {
            var helper = new TypeAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = Types().That().Are(helper.DerivedClassForAssign).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.BeAssignableTo(helper.BaseClassForAssign).AssertNoViolations(helper);
            should.BeAssignableTo(helper.BaseClassForAssignSystemType).AssertNoViolations(helper);
            should.BeAssignableTo(Types().That().Are(helper.BaseClassForAssign)).AssertNoViolations(helper);
            should.BeAssignableTo(new List<IType> { helper.BaseClassForAssign }).AssertNoViolations(helper);
            should.BeAssignableTo(new List<System.Type> { helper.BaseClassForAssignSystemType }).AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().AreAssignableTo(helper.BaseClassForAssign)).AssertNoViolations(helper);
            should.Be(Types().That().AreAssignableTo(helper.BaseClassForAssignSystemType)).AssertNoViolations(helper);
            should.Be(Types().That().AreAssignableTo(Types().That().Are(helper.BaseClassForAssign))).AssertNoViolations(helper);
            should.Be(Types().That().AreAssignableTo(new List<IType> { helper.BaseClassForAssign })).AssertNoViolations(helper);
            should.Be(Types().That().AreAssignableTo(new List<System.Type> { helper.BaseClassForAssignSystemType })).AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Complex conditions");
            should.BeAssignableToTypesThat().Are(helper.BaseClassForAssign).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = Types().That().Are(helper.UnrelatedClassForAssign).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.BeAssignableTo(helper.BaseClassForAssign).AssertOnlyViolations(helper);
            should.BeAssignableTo(helper.BaseClassForAssignSystemType).AssertOnlyViolations(helper);
            should.BeAssignableTo(Types().That().Are(helper.BaseClassForAssign)).AssertOnlyViolations(helper);
            should.BeAssignableTo(new List<IType> { helper.BaseClassForAssign }).AssertOnlyViolations(helper);
            should.BeAssignableTo(new List<System.Type> { helper.BaseClassForAssignSystemType }).AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().AreAssignableTo(helper.BaseClassForAssign)).AssertOnlyViolations(helper);
            should.Be(Types().That().AreAssignableTo(helper.BaseClassForAssignSystemType)).AssertOnlyViolations(helper);
            should.Be(Types().That().AreAssignableTo(Types().That().Are(helper.BaseClassForAssign))).AssertOnlyViolations(helper);
            should.Be(Types().That().AreAssignableTo(new List<IType> { helper.BaseClassForAssign })).AssertOnlyViolations(helper);
            should.Be(Types().That().AreAssignableTo(new List<System.Type> { helper.BaseClassForAssignSystemType })).AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Complex conditions");
            should.BeAssignableToTypesThat().Are(helper.BaseClassForAssign).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple arguments");
            should = Types().That().Are(helper.UnrelatedClassForAssign).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.BeAssignableTo(helper.BaseClassForAssign, helper.OtherBaseClassForAssign).AssertOnlyViolations(helper);
            should.BeAssignableTo(helper.BaseClassForAssignSystemType, helper.OtherBaseClassForAssignSystemType).AssertOnlyViolations(helper);
            should.BeAssignableTo(Types().That().Are(helper.BaseClassForAssign, helper.OtherBaseClassForAssign)).AssertOnlyViolations(helper);
            should.BeAssignableTo(new List<IType> { helper.BaseClassForAssign, helper.OtherBaseClassForAssign }).AssertOnlyViolations(helper);
            should.BeAssignableTo(new List<System.Type> { helper.BaseClassForAssignSystemType, helper.OtherBaseClassForAssignSystemType }).AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().AreAssignableTo(helper.BaseClassForAssign, helper.OtherBaseClassForAssign)).AssertOnlyViolations(helper);
            should.Be(Types().That().AreAssignableTo(helper.BaseClassForAssignSystemType, helper.OtherBaseClassForAssignSystemType)).AssertOnlyViolations(helper);
            should.Be(Types().That().AreAssignableTo(Types().That().Are(helper.BaseClassForAssign, helper.OtherBaseClassForAssign))).AssertOnlyViolations(helper);
            should.Be(Types().That().AreAssignableTo(new List<IType> { helper.BaseClassForAssign, helper.OtherBaseClassForAssign })).AssertOnlyViolations(helper);
            should.Be(Types().That().AreAssignableTo(new List<System.Type> { helper.BaseClassForAssignSystemType, helper.OtherBaseClassForAssignSystemType })).AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Complex conditions");
            should.BeAssignableToTypesThat().Are(helper.BaseClassForAssign, helper.OtherBaseClassForAssign).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Types().That().Are(helper.DerivedClassForAssign, helper.OtherDerivedClassForAssign).Should().BeAssignableTo(helper.BaseClassForAssign).AssertNoViolations(helper);
            Types().That().Are(helper.DerivedClassForAssign, helper.UnrelatedClassForAssign).Should().BeAssignableTo(helper.BaseClassForAssign).AssertAnyViolations(helper);

            helper.AddSnapshotHeader("Type not in architecture");
            should = Types().That().Are(helper.BaseClassForAssign).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.BeAssignableTo(new List<System.Type> { typeof(AttributeNamespace.ClassWithoutAttributes) }).AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().AreAssignableTo(new List<System.Type> { typeof(AttributeNamespace.ClassWithoutAttributes) })).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Empty Arguments (Predicates only)");
            should = Types().That().Are(helper.BaseClassForAssign).Should();
            should.Be(Types().That().AreAssignableTo(new List<IType>())).AssertOnlyViolations(helper);
            should.Be(Types().That().AreAssignableTo(new List<System.Type>())).AssertOnlyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotBeAssignableToTest()
        {
            var helper = new TypeAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = Types().That().Are(helper.UnrelatedClassForAssign).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotBeAssignableTo(helper.BaseClassForAssign).AssertNoViolations(helper);
            should.NotBeAssignableTo(helper.BaseClassForAssignSystemType).AssertNoViolations(helper);
            should.NotBeAssignableTo(Types().That().Are(helper.BaseClassForAssign)).AssertNoViolations(helper);
            should.NotBeAssignableTo(new List<IType> { helper.BaseClassForAssign }).AssertNoViolations(helper);
            should.NotBeAssignableTo(new List<System.Type> { helper.BaseClassForAssignSystemType }).AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().AreNotAssignableTo(helper.BaseClassForAssign)).AssertNoViolations(helper);
            should.Be(Types().That().AreNotAssignableTo(helper.BaseClassForAssignSystemType)).AssertNoViolations(helper);
            should.Be(Types().That().AreNotAssignableTo(Types().That().Are(helper.BaseClassForAssign))).AssertNoViolations(helper);
            should.Be(Types().That().AreNotAssignableTo(new List<IType> { helper.BaseClassForAssign })).AssertNoViolations(helper);
            should.Be(Types().That().AreNotAssignableTo(new List<System.Type> { helper.BaseClassForAssignSystemType })).AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Complex conditions");
            should.NotBeAssignableToTypesThat().Are(helper.BaseClassForAssign).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = Types().That().Are(helper.DerivedClassForAssign).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotBeAssignableTo(helper.BaseClassForAssign).AssertOnlyViolations(helper);
            should.NotBeAssignableTo(helper.BaseClassForAssignSystemType).AssertOnlyViolations(helper);
            should.NotBeAssignableTo(Types().That().Are(helper.BaseClassForAssign)).AssertOnlyViolations(helper);
            should.NotBeAssignableTo(new List<IType> { helper.BaseClassForAssign }).AssertOnlyViolations(helper);
            should.NotBeAssignableTo(new List<System.Type> { helper.BaseClassForAssignSystemType }).AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().AreNotAssignableTo(helper.BaseClassForAssign)).AssertOnlyViolations(helper);
            should.Be(Types().That().AreNotAssignableTo(helper.BaseClassForAssignSystemType)).AssertOnlyViolations(helper);
            should.Be(Types().That().AreNotAssignableTo(Types().That().Are(helper.BaseClassForAssign))).AssertOnlyViolations(helper);
            should.Be(Types().That().AreNotAssignableTo(new List<IType> { helper.BaseClassForAssign })).AssertOnlyViolations(helper);
            should.Be(Types().That().AreNotAssignableTo(new List<System.Type> { helper.BaseClassForAssignSystemType })).AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Complex conditions");
            should.NotBeAssignableToTypesThat().Are(helper.BaseClassForAssign).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple arguments");
            should = Types().That().Are(helper.DerivedClassForAssign).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotBeAssignableTo(helper.BaseClassForAssign, helper.OtherBaseClassForAssign).AssertOnlyViolations(helper);
            should.NotBeAssignableTo(helper.BaseClassForAssignSystemType, helper.OtherBaseClassForAssignSystemType).AssertOnlyViolations(helper);
            should.NotBeAssignableTo(Types().That().Are(helper.BaseClassForAssign, helper.OtherBaseClassForAssign)).AssertOnlyViolations(helper);
            should.NotBeAssignableTo(new List<IType> { helper.BaseClassForAssign, helper.OtherBaseClassForAssign }).AssertOnlyViolations(helper);
            should.NotBeAssignableTo(new List<System.Type> { helper.BaseClassForAssignSystemType, helper.OtherBaseClassForAssignSystemType }).AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().AreNotAssignableTo(helper.BaseClassForAssign, helper.OtherBaseClassForAssign)).AssertOnlyViolations(helper);
            should.Be(Types().That().AreNotAssignableTo(helper.BaseClassForAssignSystemType, helper.OtherBaseClassForAssignSystemType)).AssertOnlyViolations(helper);
            should.Be(Types().That().AreNotAssignableTo(Types().That().Are(helper.BaseClassForAssign, helper.OtherBaseClassForAssign))).AssertOnlyViolations(helper);
            should.Be(Types().That().AreNotAssignableTo(new List<IType> { helper.BaseClassForAssign, helper.OtherBaseClassForAssign })).AssertOnlyViolations(helper);
            should.Be(Types().That().AreNotAssignableTo(new List<System.Type> { helper.BaseClassForAssignSystemType, helper.OtherBaseClassForAssignSystemType })).AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Complex conditions");
            should.NotBeAssignableToTypesThat().Are(helper.BaseClassForAssign, helper.OtherBaseClassForAssign).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Types().That().Are(helper.UnrelatedClassForAssign, helper.OtherUnrelatedClassForAssign).Should().NotBeAssignableTo(helper.BaseClassForAssign).AssertNoViolations(helper);
            Types().That().Are(helper.DerivedClassForAssign, helper.UnrelatedClassForAssign).Should().NotBeAssignableTo(helper.BaseClassForAssign).AssertAnyViolations(helper);

            helper.AddSnapshotHeader("Type not in architecture");
            should = Types().That().Are(helper.BaseClassForAssign).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotBeAssignableTo(new List<System.Type> { typeof(AttributeNamespace.ClassWithoutAttributes) }).AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().AreNotAssignableTo(new List<System.Type> { typeof(AttributeNamespace.ClassWithoutAttributes) })).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Empty Arguments");
            should = Types().That().Are(helper.BaseClassForAssign).Should();
            
            helper.AddSnapshotSubHeader("Conditions");
            // TODO throws InvalidOperationException
            // should.NotBeAssignableTo(new List<IType>()).AssertNoViolations(helper);
            // should.NotBeAssignableTo(new List<System.Type>()).AssertNoViolations(helper);
            
            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().AreNotAssignableTo(new List<IType>())).AssertNoViolations(helper);
            should.Be(Types().That().AreNotAssignableTo(new List<System.Type>())).AssertNoViolations(helper);

            await helper.AssertSnapshotMatches();
        }
        
        [Fact]
        public async Task BeNestedInTest()
        {
            var helper = new TypeAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = Types().That().Are(helper.InnerClassA).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.BeNestedIn(helper.OuterClassA).AssertNoViolations(helper);
            should.BeNestedIn(helper.OuterClassASystemType).AssertNoViolations(helper);
            should.BeNestedIn(Types().That().Are(helper.OuterClassA)).AssertNoViolations(helper);
            should.BeNestedIn(new List<IType> { helper.OuterClassA }).AssertNoViolations(helper);
            should.BeNestedIn(new List<System.Type> { helper.OuterClassASystemType }).AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().AreNestedIn(helper.OuterClassA)).AssertNoViolations(helper);
            should.Be(Types().That().AreNestedIn(helper.OuterClassASystemType)).AssertNoViolations(helper);
            should.Be(Types().That().AreNestedIn(Types().That().Are(helper.OuterClassA))).AssertNoViolations(helper);
            should.Be(Types().That().AreNestedIn(new List<IType> { helper.OuterClassA })).AssertNoViolations(helper);
            should.Be(Types().That().AreNestedIn(new List<System.Type> { helper.OuterClassASystemType })).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = Types().That().Are(helper.NonNestedClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.BeNestedIn(helper.OuterClassA).AssertOnlyViolations(helper);
            should.BeNestedIn(helper.OuterClassASystemType).AssertOnlyViolations(helper);
            should.BeNestedIn(Types().That().Are(helper.OuterClassA)).AssertOnlyViolations(helper);
            should.BeNestedIn(new List<IType> { helper.OuterClassA }).AssertOnlyViolations(helper);
            should.BeNestedIn(new List<System.Type> { helper.OuterClassASystemType }).AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().AreNestedIn(helper.OuterClassA)).AssertOnlyViolations(helper);
            should.Be(Types().That().AreNestedIn(helper.OuterClassASystemType)).AssertOnlyViolations(helper);
            should.Be(Types().That().AreNestedIn(Types().That().Are(helper.OuterClassA))).AssertOnlyViolations(helper);
            should.Be(Types().That().AreNestedIn(new List<IType> { helper.OuterClassA })).AssertOnlyViolations(helper);
            should.Be(Types().That().AreNestedIn(new List<System.Type> { helper.OuterClassASystemType })).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Types().That().Are(helper.InnerClassA, helper.OtherInnerClassA).Should().BeNestedIn(helper.OuterClassA).AssertNoViolations(helper);
            Types().That().Are(helper.InnerClassA, helper.NonNestedClass).Should().BeNestedIn(helper.OuterClassA).AssertAnyViolations(helper);

            helper.AddSnapshotHeader("Multiple arguments");
            should = Types().That().Are(helper.InnerClassA).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.BeNestedIn(helper.OuterClassA, helper.NonNestedClass).AssertNoViolations(helper);
            should.BeNestedIn(new List<IType> { helper.OuterClassA, helper.NonNestedClass }).AssertNoViolations(helper);
            should.BeNestedIn(helper.OuterClassASystemType, helper.NonNestedClassSystemType).AssertNoViolations(helper);
            should.BeNestedIn(new List<System.Type> { helper.OuterClassASystemType, helper.NonNestedClassSystemType }).AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().AreNestedIn(helper.OuterClassA, helper.NonNestedClass)).AssertNoViolations(helper);
            should.Be(Types().That().AreNestedIn(new List<IType> { helper.OuterClassA, helper.NonNestedClass })).AssertNoViolations(helper);
            should.Be(Types().That().AreNestedIn(helper.OuterClassASystemType, helper.NonNestedClassSystemType)).AssertNoViolations(helper);
            should.Be(Types().That().AreNestedIn(new List<System.Type> { helper.OuterClassASystemType, helper.NonNestedClassSystemType })).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Empty Arguments");
            should = Types().That().Are(helper.InnerClassA).Should();
            
            helper.AddSnapshotSubHeader("Conditions");
            // TODO throws InvalidOperationException
            // should.BeNestedIn(new List<IType>()).AssertOnlyViolations(helper);
            // should.BeNestedIn(new List<System.Type>()).AssertOnlyViolations(helper);
            
            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().AreNestedIn(new List<IType>())).AssertOnlyViolations(helper);
            should.Be(Types().That().AreNestedIn(new List<System.Type>())).AssertOnlyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task BeValueTypesTest()
        {
            var helper = new TypeAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            helper.AddSnapshotSubHeader("Conditions");
            Types().That().Are(helper.SimpleEnum).Should().BeValueTypes().AssertNoViolations(helper);
            Types().That().Are(helper.SimpleStruct).Should().BeValueTypes().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            Types().That().Are(helper.SimpleEnum).Should().Be(Types().That().AreValueTypes()).AssertNoViolations(helper);
            Types().That().Are(helper.SimpleStruct).Should().Be(Types().That().AreValueTypes()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            helper.AddSnapshotSubHeader("Conditions");
            Types().That().Are(helper.RegularClass).Should().BeValueTypes().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            Types().That().Are(helper.RegularClass).Should().Be(Types().That().AreValueTypes()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Types().That().Are(helper.SimpleEnum, helper.SimpleStruct).Should().BeValueTypes().AssertNoViolations(helper);
            Types().That().Are(helper.SimpleEnum, helper.RegularClass).Should().BeValueTypes().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotBeValueTypesTest()
        {
            var helper = new TypeAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            helper.AddSnapshotSubHeader("Conditions");
            Types().That().Are(helper.RegularClass).Should().NotBeValueTypes().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            Types().That().Are(helper.RegularClass).Should().Be(Types().That().AreNotValueTypes()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            helper.AddSnapshotSubHeader("Conditions");
            Types().That().Are(helper.SimpleEnum).Should().NotBeValueTypes().AssertOnlyViolations(helper);
            Types().That().Are(helper.SimpleStruct).Should().NotBeValueTypes().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            Types().That().Are(helper.SimpleEnum).Should().Be(Types().That().AreNotValueTypes()).AssertOnlyViolations(helper);
            Types().That().Are(helper.SimpleStruct).Should().Be(Types().That().AreNotValueTypes()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Types().That().Are(helper.RegularClass, helper.OtherRegularClass).Should().NotBeValueTypes().AssertNoViolations(helper);
            Types().That().Are(helper.RegularClass, helper.SimpleEnum).Should().NotBeValueTypes().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task BeEnumsTest()
        {
            var helper = new TypeAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            helper.AddSnapshotSubHeader("Conditions");
            Types().That().Are(helper.SimpleEnum).Should().BeEnums().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            Types().That().Are(helper.SimpleEnum).Should().Be(Types().That().AreEnums()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            helper.AddSnapshotSubHeader("Conditions");
            Types().That().Are(helper.RegularClass).Should().BeEnums().AssertOnlyViolations(helper);
            Types().That().Are(helper.SimpleStruct).Should().BeEnums().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            Types().That().Are(helper.RegularClass).Should().Be(Types().That().AreEnums()).AssertOnlyViolations(helper);
            Types().That().Are(helper.SimpleStruct).Should().Be(Types().That().AreEnums()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Types().That().Are(helper.SimpleEnum, helper.OtherEnum).Should().BeEnums().AssertNoViolations(helper);
            Types().That().Are(helper.SimpleEnum, helper.RegularClass).Should().BeEnums().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotBeEnumsTest()
        {
            var helper = new TypeAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            helper.AddSnapshotSubHeader("Conditions");
            Types().That().Are(helper.RegularClass).Should().NotBeEnums().AssertNoViolations(helper);
            Types().That().Are(helper.SimpleStruct).Should().NotBeEnums().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            Types().That().Are(helper.RegularClass).Should().Be(Types().That().AreNotEnums()).AssertNoViolations(helper);
            Types().That().Are(helper.SimpleStruct).Should().Be(Types().That().AreNotEnums()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            helper.AddSnapshotSubHeader("Conditions");
            Types().That().Are(helper.SimpleEnum).Should().NotBeEnums().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            Types().That().Are(helper.SimpleEnum).Should().Be(Types().That().AreNotEnums()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Types().That().Are(helper.RegularClass, helper.SimpleStruct).Should().NotBeEnums().AssertNoViolations(helper);
            Types().That().Are(helper.RegularClass, helper.SimpleEnum).Should().NotBeEnums().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task BeStructsTest()
        {
            var helper = new TypeAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            helper.AddSnapshotSubHeader("Conditions");
            Types().That().Are(helper.SimpleStruct).Should().BeStructs().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            Types().That().Are(helper.SimpleStruct).Should().Be(Types().That().AreStructs()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            helper.AddSnapshotSubHeader("Conditions");
            Types().That().Are(helper.RegularClass).Should().BeStructs().AssertOnlyViolations(helper);
            Types().That().Are(helper.SimpleEnum).Should().BeStructs().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            Types().That().Are(helper.RegularClass).Should().Be(Types().That().AreStructs()).AssertOnlyViolations(helper);
            Types().That().Are(helper.SimpleEnum).Should().Be(Types().That().AreStructs()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Types().That().Are(helper.SimpleStruct, helper.OtherStruct).Should().BeStructs().AssertNoViolations(helper);
            Types().That().Are(helper.SimpleStruct, helper.RegularClass).Should().BeStructs().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotBeStructsTest()
        {
            var helper = new TypeAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            helper.AddSnapshotSubHeader("Conditions");
            Types().That().Are(helper.RegularClass).Should().NotBeStructs().AssertNoViolations(helper);
            Types().That().Are(helper.SimpleEnum).Should().NotBeStructs().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            Types().That().Are(helper.RegularClass).Should().Be(Types().That().AreNotStructs()).AssertNoViolations(helper);
            Types().That().Are(helper.SimpleEnum).Should().Be(Types().That().AreNotStructs()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            helper.AddSnapshotSubHeader("Conditions");
            Types().That().Are(helper.SimpleStruct).Should().NotBeStructs().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            Types().That().Are(helper.SimpleStruct).Should().Be(Types().That().AreNotStructs()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Types().That().Are(helper.RegularClass, helper.SimpleEnum).Should().NotBeStructs().AssertNoViolations(helper);
            Types().That().Are(helper.RegularClass, helper.SimpleStruct).Should().NotBeStructs().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task ImplementInterfaceTest()
        {
            var helper = new TypeAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = Types().That().Are(helper.ClassImplementingInterface).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.ImplementInterface(helper.TestInterface).AssertNoViolations(helper);
            should.ImplementInterface(helper.TestInterfaceSystemType).AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().ImplementInterface(helper.TestInterface)).AssertNoViolations(helper);
            should.Be(Types().That().ImplementInterface(helper.TestInterfaceSystemType)).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = Types().That().Are(helper.ClassNotImplementingInterface).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.ImplementInterface(helper.TestInterface).AssertOnlyViolations(helper);
            should.ImplementInterface(helper.TestInterfaceSystemType).AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().ImplementInterface(helper.TestInterface)).AssertOnlyViolations(helper);
            should.Be(Types().That().ImplementInterface(helper.TestInterfaceSystemType)).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Types().That().Are(helper.ClassImplementingInterface, helper.ClassNotImplementingInterface).Should().ImplementInterface(helper.TestInterface).AssertAnyViolations(helper);

            helper.AddSnapshotHeader("Type not in architecture");
            should = Types().That().Are(helper.ClassImplementingInterface).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.ImplementInterface(typeof(System.IDisposable)).AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().ImplementInterface(typeof(System.IDisposable))).AssertOnlyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotImplementInterfaceTest()
        {
            var helper = new TypeAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = Types().That().Are(helper.ClassNotImplementingInterface).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotImplementInterface(helper.TestInterface).AssertNoViolations(helper);
            should.NotImplementInterface(helper.TestInterfaceSystemType).AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().DoNotImplementInterface(helper.TestInterface)).AssertNoViolations(helper);
            should.Be(Types().That().DoNotImplementInterface(helper.TestInterfaceSystemType)).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = Types().That().Are(helper.ClassImplementingInterface).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotImplementInterface(helper.TestInterface).AssertOnlyViolations(helper);
            should.NotImplementInterface(helper.TestInterfaceSystemType).AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().DoNotImplementInterface(helper.TestInterface)).AssertOnlyViolations(helper);
            should.Be(Types().That().DoNotImplementInterface(helper.TestInterfaceSystemType)).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Types().That().Are(helper.ClassNotImplementingInterface, helper.ClassImplementingInterface).Should().NotImplementInterface(helper.TestInterface).AssertAnyViolations(helper);

            helper.AddSnapshotHeader("Type not in architecture");
            should = Types().That().Are(helper.ClassImplementingInterface).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotImplementInterface(typeof(System.IDisposable)).AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().DoNotImplementInterface(typeof(System.IDisposable))).AssertNoViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task ImplementAnyInterfacesTest()
        {
            var helper = new TypeAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = Interfaces().That().Are(helper.ChildTestInterface).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.ImplementAnyInterfaces(helper.TestInterface).AssertNoViolations(helper);
            should.ImplementAnyInterfaces([helper.TestInterface]).AssertNoViolations(helper);
            should.ImplementAnyInterfaces(helper.TestInterfaceSystemType).AssertNoViolations(helper);
            should.ImplementAnyInterfaces([helper.TestInterfaceSystemType]).AssertNoViolations(helper);
            should.ImplementAnyInterfaces(Interfaces().That().Are(helper.TestInterface)).AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Interfaces().That().ImplementAnyInterfaces(helper.TestInterface)).AssertNoViolations(helper);
            should.Be(Interfaces().That().ImplementAnyInterfaces([helper.TestInterface])).AssertNoViolations(helper);
            should.Be(Interfaces().That().ImplementAnyInterfaces(helper.TestInterfaceSystemType)).AssertNoViolations(helper);
            should.Be(Interfaces().That().ImplementAnyInterfaces([helper.TestInterfaceSystemType])).AssertNoViolations(helper);
            should.Be(Interfaces().That().ImplementAnyInterfaces(Interfaces().That().Are(helper.TestInterface))).AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Complex conditions");
            should.ImplementAnyInterfacesThat().Are(helper.TestInterface).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = Interfaces().That().Are(helper.ChildTestInterface).Should();
            
            helper.AddSnapshotSubHeader("Conditions");
            should.ImplementAnyInterfaces(helper.OtherTestInterface).AssertOnlyViolations(helper);
            should.ImplementAnyInterfaces([helper.OtherTestInterface]).AssertOnlyViolations(helper);
            should.ImplementAnyInterfaces(helper.OtherTestInterfaceSystemType).AssertOnlyViolations(helper);
            should.ImplementAnyInterfaces([helper.OtherTestInterfaceSystemType]).AssertOnlyViolations(helper);
            should.ImplementAnyInterfaces(Interfaces().That().Are(helper.OtherTestInterface)).AssertOnlyViolations(helper);
                
            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Interfaces().That().ImplementAnyInterfaces(helper.OtherTestInterface)).AssertOnlyViolations(helper);
            should.Be(Interfaces().That().ImplementAnyInterfaces([helper.OtherTestInterface])).AssertOnlyViolations(helper);
            should.Be(Interfaces().That().ImplementAnyInterfaces(helper.OtherTestInterfaceSystemType)).AssertOnlyViolations(helper);
            should.Be(Interfaces().That().ImplementAnyInterfaces([helper.OtherTestInterfaceSystemType])).AssertOnlyViolations(helper);
            should.Be(Interfaces().That().ImplementAnyInterfaces(Interfaces().That().Are(helper.OtherTestInterface))).AssertOnlyViolations(helper);
            
            helper.AddSnapshotSubHeader("Complex conditions");
            should.ImplementAnyInterfacesThat().Are(helper.OtherTestInterface).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Empty Arguments (Only Violations)");
            should = Interfaces().That().Are(helper.TestInterface, helper.ChildTestInterface).Should();
            
            helper.AddSnapshotSubHeader("Conditions");
            should.ImplementAnyInterfaces().AssertOnlyViolations(helper);
            should.ImplementAnyInterfaces(new List<Interface>()).AssertOnlyViolations(helper);
            should.ImplementAnyInterfaces(new List<System.Type>()).AssertOnlyViolations(helper);
            should.ImplementAnyInterfaces(Interfaces().That().HaveName(helper.NonExistentObjectName)).AssertOnlyViolations(helper);
           
            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Interfaces().That().ImplementAnyInterfaces()).AssertOnlyViolations(helper);
            should.Be(Interfaces().That().ImplementAnyInterfaces(new List<Interface>())).AssertOnlyViolations(helper);
            should.Be(Interfaces().That().ImplementAnyInterfaces(new List<System.Type>())).AssertOnlyViolations(helper);
            should.Be(Interfaces().That().ImplementAnyInterfaces(Interfaces().That().HaveName(helper.NonExistentObjectName))).AssertOnlyViolations(helper);
            
            helper.AddSnapshotSubHeader("Complex conditions");
            should.ImplementAnyInterfacesThat().Are(new List<Interface>()).AssertOnlyViolations(helper);
            
            helper.AddSnapshotHeader("Multiple arguments");
            should = Interfaces().That().Are(helper.ChildTestInterface).Should();
            
            helper.AddSnapshotSubHeader("Conditions");
            should.ImplementAnyInterfaces(helper.OtherTestInterface, helper.OtherChildTestInterface).AssertOnlyViolations(helper);
            should.ImplementAnyInterfaces([helper.OtherTestInterface, helper.OtherChildTestInterface]).AssertOnlyViolations(helper);
            should.ImplementAnyInterfaces(helper.OtherTestInterfaceSystemType, helper.OtherChildTestInterfaceSystemType).AssertOnlyViolations(helper);
            should.ImplementAnyInterfaces([helper.OtherTestInterfaceSystemType, helper.OtherChildTestInterfaceSystemType]).AssertOnlyViolations(helper);
            should.ImplementAnyInterfaces(Interfaces().That().Are(helper.OtherTestInterface, helper.OtherChildTestInterface)).AssertOnlyViolations(helper);
            
            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Interfaces().That().ImplementAnyInterfaces(helper.OtherTestInterface, helper.OtherChildTestInterface)).AssertOnlyViolations(helper);
            should.Be(Interfaces().That().ImplementAnyInterfaces([helper.OtherTestInterface, helper.OtherChildTestInterface])).AssertOnlyViolations(helper);
            should.Be(Interfaces().That().ImplementAnyInterfaces(helper.OtherTestInterfaceSystemType, helper.OtherChildTestInterfaceSystemType)).AssertOnlyViolations(helper);
            should.Be(Interfaces().That().ImplementAnyInterfaces([helper.OtherTestInterfaceSystemType, helper.OtherChildTestInterfaceSystemType])).AssertOnlyViolations(helper);
            should.Be(Interfaces().That().ImplementAnyInterfaces(Interfaces().That().Are(helper.OtherTestInterface, helper.OtherChildTestInterface))).AssertOnlyViolations(helper);
            
            helper.AddSnapshotSubHeader("Complex conditions");
            should.ImplementAnyInterfacesThat().Are(helper.OtherTestInterface, helper.OtherChildTestInterface).AssertOnlyViolations(helper);
            should.ImplementAnyInterfacesThat().Are([helper.OtherTestInterface, helper.OtherChildTestInterface]).AssertOnlyViolations(helper);
            should.ImplementAnyInterfacesThat().Are(helper.OtherTestInterfaceSystemType, helper.OtherChildTestInterfaceSystemType).AssertOnlyViolations(helper);
            should.ImplementAnyInterfacesThat().Are([helper.OtherTestInterfaceSystemType, helper.OtherChildTestInterfaceSystemType]).AssertOnlyViolations(helper);
            should.ImplementAnyInterfacesThat().Are(Interfaces().That().Are(helper.OtherTestInterface, helper.OtherChildTestInterface)).AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Complex conditions");
            should.ImplementAnyInterfacesThat().Are(helper.OtherTestInterface, helper.OtherChildTestInterface).AssertOnlyViolations(helper);
            
            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotImplementAnyInterfacesTest()
        {
            var helper = new TypeAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = Interfaces().That().Are(helper.ChildTestInterface).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotImplementAnyInterfaces(helper.OtherTestInterface).AssertNoViolations(helper);
            should.NotImplementAnyInterfaces([helper.OtherTestInterface]).AssertNoViolations(helper);
            should.NotImplementAnyInterfaces(helper.OtherTestInterfaceSystemType).AssertNoViolations(helper);
            should.NotImplementAnyInterfaces([helper.OtherTestInterfaceSystemType]).AssertNoViolations(helper);
            should.NotImplementAnyInterfaces(Interfaces().That().Are(helper.OtherTestInterface)).AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Interfaces().That().DoNotImplementAnyInterfaces(helper.OtherTestInterface)).AssertNoViolations(helper);
            should.Be(Interfaces().That().DoNotImplementAnyInterfaces([helper.OtherTestInterface])).AssertNoViolations(helper);
            should.Be(Interfaces().That().DoNotImplementAnyInterfaces(helper.OtherTestInterfaceSystemType)).AssertNoViolations(helper);
            should.Be(Interfaces().That().DoNotImplementAnyInterfaces([helper.OtherTestInterfaceSystemType])).AssertNoViolations(helper);
            should.Be(Interfaces().That().DoNotImplementAnyInterfaces(Interfaces().That().Are(helper.OtherTestInterface))).AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Complex conditions");
            should.NotImplementAnyInterfacesThat().Are(helper.OtherTestInterface).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = Interfaces().That().Are(helper.ChildTestInterface).Should();
            
            helper.AddSnapshotSubHeader("Conditions");
            should.NotImplementAnyInterfaces(helper.TestInterface).AssertOnlyViolations(helper);
            should.NotImplementAnyInterfaces([helper.TestInterface]).AssertOnlyViolations(helper);
            should.NotImplementAnyInterfaces(helper.TestInterfaceSystemType).AssertOnlyViolations(helper);
            should.NotImplementAnyInterfaces([helper.TestInterfaceSystemType]).AssertOnlyViolations(helper);
            should.NotImplementAnyInterfaces(Interfaces().That().Are(helper.TestInterface)).AssertOnlyViolations(helper);
            
            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Interfaces().That().DoNotImplementAnyInterfaces(helper.TestInterface)).AssertOnlyViolations(helper);
            should.Be(Interfaces().That().DoNotImplementAnyInterfaces([helper.TestInterface])).AssertOnlyViolations(helper);
            should.Be(Interfaces().That().DoNotImplementAnyInterfaces(helper.TestInterfaceSystemType)).AssertOnlyViolations(helper);
            should.Be(Interfaces().That().DoNotImplementAnyInterfaces([helper.TestInterfaceSystemType])).AssertOnlyViolations(helper);
            should.Be(Interfaces().That().DoNotImplementAnyInterfaces(Interfaces().That().Are(helper.TestInterface))).AssertOnlyViolations(helper);
            
            helper.AddSnapshotSubHeader("Complex conditions");
            should.NotImplementAnyInterfacesThat().Are(helper.TestInterface).AssertOnlyViolations(helper);
            
            helper.AddSnapshotHeader("Empty Arguments (No Violations)");
            should = Interfaces().That().Are(helper.TestInterface, helper.ChildTestInterface).Should();
            
            helper.AddSnapshotSubHeader("Conditions");
            should.NotImplementAnyInterfaces().AssertNoViolations(helper);
            should.NotImplementAnyInterfaces(new List<Interface>()).AssertNoViolations(helper);
            should.NotImplementAnyInterfaces(new List<System.Type>()).AssertNoViolations(helper);
            should.NotImplementAnyInterfaces(Interfaces().That().HaveName(helper.NonExistentObjectName)).AssertNoViolations(helper);
           
            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Interfaces().That().DoNotImplementAnyInterfaces()).AssertNoViolations(helper);
            should.Be(Interfaces().That().DoNotImplementAnyInterfaces(new List<Interface>())).AssertNoViolations(helper);
            should.Be(Interfaces().That().DoNotImplementAnyInterfaces(new List<System.Type>())).AssertNoViolations(helper);
            should.Be(Interfaces().That().DoNotImplementAnyInterfaces(Interfaces().That().HaveName(helper.NonExistentObjectName))).AssertNoViolations(helper);
            
            helper.AddSnapshotSubHeader("Complex conditions");
            should.NotImplementAnyInterfacesThat().Are(new List<Interface>()).AssertNoViolations(helper);
            
            helper.AddSnapshotHeader("Multiple arguments");
            should = Interfaces().That().Are(helper.ChildTestInterface).Should();
            
            helper.AddSnapshotSubHeader("Conditions");
            should.NotImplementAnyInterfaces(helper.TestInterface, helper.OtherTestInterface).AssertOnlyViolations(helper);
            should.NotImplementAnyInterfaces([helper.TestInterface, helper.OtherTestInterface]).AssertOnlyViolations(helper);
            should.NotImplementAnyInterfaces(helper.TestInterfaceSystemType, helper.OtherTestInterfaceSystemType).AssertOnlyViolations(helper);
            should.NotImplementAnyInterfaces([helper.TestInterfaceSystemType, helper.OtherTestInterfaceSystemType]).AssertOnlyViolations(helper);
            should.NotImplementAnyInterfaces(Interfaces().That().Are(helper.TestInterface, helper.OtherTestInterface)).AssertOnlyViolations(helper);
            
            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Interfaces().That().DoNotImplementAnyInterfaces(helper.TestInterface, helper.OtherTestInterface)).AssertOnlyViolations(helper);
            should.Be(Interfaces().That().DoNotImplementAnyInterfaces([helper.TestInterface, helper.OtherTestInterface])).AssertOnlyViolations(helper);
            should.Be(Interfaces().That().DoNotImplementAnyInterfaces(helper.TestInterfaceSystemType, helper.OtherTestInterfaceSystemType)).AssertOnlyViolations(helper);
            should.Be(Interfaces().That().DoNotImplementAnyInterfaces([helper.TestInterfaceSystemType, helper.OtherTestInterfaceSystemType])).AssertOnlyViolations(helper);
            should.Be(Interfaces().That().DoNotImplementAnyInterfaces(Interfaces().That().Are(helper.TestInterface, helper.OtherTestInterface))).AssertOnlyViolations(helper);
            
            helper.AddSnapshotSubHeader("Complex conditions");
            should.NotImplementAnyInterfacesThat().Are(helper.TestInterface, helper.OtherTestInterface).AssertOnlyViolations(helper);
            
            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task ResideInNamespaceTest()
        {
            var helper = new TypeAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = Types().That().Are(helper.RegularClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.ResideInNamespace("TypeNamespace").AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().ResideInNamespace("TypeNamespace")).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = Types().That().Are(helper.RegularClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.ResideInNamespace("NonExistentNamespace").AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().ResideInNamespace("NonExistentNamespace")).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Types().That().Are(helper.RegularClass, helper.OtherRegularClass).Should().ResideInNamespace("TypeNamespace").AssertNoViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotResideInNamespaceTest()
        {
            var helper = new TypeAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = Types().That().Are(helper.RegularClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotResideInNamespace("NonExistentNamespace").AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().DoNotResideInNamespace("NonExistentNamespace")).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = Types().That().Are(helper.RegularClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotResideInNamespace("TypeNamespace").AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().DoNotResideInNamespace("TypeNamespace")).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Types().That().Are(helper.RegularClass, helper.OtherRegularClass).Should().NotResideInNamespace("NonExistentNamespace").AssertNoViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task ResideInNamespaceMatchingTest()
        {
            var helper = new TypeAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = Types().That().Are(helper.RegularClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.ResideInNamespaceMatching("TypeName.*").AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().ResideInNamespaceMatching("TypeName.*")).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = Types().That().Are(helper.RegularClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.ResideInNamespaceMatching("NonExistent.*").AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().ResideInNamespaceMatching("NonExistent.*")).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Types().That().Are(helper.RegularClass, helper.OtherRegularClass).Should().ResideInNamespaceMatching("TypeName.*").AssertNoViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotResideInNamespaceMatchingTest()
        {
            var helper = new TypeAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = Types().That().Are(helper.RegularClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotResideInNamespaceMatching("NonExistent.*").AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().DoNotResideInNamespaceMatching("NonExistent.*")).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = Types().That().Are(helper.RegularClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotResideInNamespaceMatching("TypeName.*").AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().DoNotResideInNamespaceMatching("TypeName.*")).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Types().That().Are(helper.RegularClass, helper.OtherRegularClass).Should().NotResideInNamespaceMatching("NonExistent.*").AssertNoViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task ResideInAssemblyTest()
        {
            var helper = new TypeAssemblyTestHelper();
            var typeAssembly = typeof(TypeNamespace.RegularClass).Assembly;
            var archAssembly = helper.Architecture.Assemblies.First();
            var assemblyFullName = archAssembly.FullName;

            helper.AddSnapshotHeader("No Violations");
            var should = Types().That().Are(helper.RegularClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.ResideInAssembly(typeAssembly).AssertNoViolations(helper);
            should.ResideInAssembly(archAssembly).AssertNoViolations(helper);
            should.ResideInAssembly(assemblyFullName).AssertNoViolations(helper);
            should.ResideInAssemblyMatching("TypeAssembly.*").AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().ResideInAssembly(typeAssembly)).AssertNoViolations(helper);
            should.Be(Types().That().ResideInAssembly(archAssembly)).AssertNoViolations(helper);
            should.Be(Types().That().ResideInAssembly(assemblyFullName)).AssertNoViolations(helper);
            should.Be(Types().That().ResideInAssemblyMatching("TypeAssembly.*")).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = Types().That().Are(helper.RegularClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.ResideInAssembly("NonExistentAssembly").AssertOnlyViolations(helper);
            should.ResideInAssemblyMatching("NonExistent.*").AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().ResideInAssembly("NonExistentAssembly")).AssertOnlyViolations(helper);
            should.Be(Types().That().ResideInAssemblyMatching("NonExistent.*")).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Types().That().Are(helper.RegularClass, helper.OtherRegularClass).Should().ResideInAssembly(assemblyFullName).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Multiple assemblies");
            should = Types().That().Are(helper.RegularClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.ResideInAssembly(archAssembly, archAssembly).AssertNoViolations(helper);
            should.ResideInAssembly(typeAssembly, typeAssembly).AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().ResideInAssembly(archAssembly, archAssembly)).AssertNoViolations(helper);
            should.Be(Types().That().ResideInAssembly(typeAssembly, typeAssembly)).AssertNoViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotResideInAssemblyTest()
        {
            var helper = new TypeAssemblyTestHelper();
            var typeAssembly = typeof(TypeNamespace.RegularClass).Assembly;
            var archAssembly = helper.Architecture.Assemblies.First();
            var assemblyFullName = archAssembly.FullName;

            helper.AddSnapshotHeader("No Violations");
            var should = Types().That().Are(helper.RegularClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotResideInAssembly("NonExistentAssembly").AssertNoViolations(helper);
            should.NotResideInAssemblyMatching("NonExistent.*").AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().DoNotResideInAssembly("NonExistentAssembly")).AssertNoViolations(helper);
            should.Be(Types().That().DoNotResideInAssemblyMatching("NonExistent.*")).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = Types().That().Are(helper.RegularClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotResideInAssembly(typeAssembly).AssertOnlyViolations(helper);
            should.NotResideInAssembly(archAssembly).AssertOnlyViolations(helper);
            should.NotResideInAssembly(assemblyFullName).AssertOnlyViolations(helper);
            should.NotResideInAssemblyMatching("TypeAssembly.*").AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().DoNotResideInAssembly(typeAssembly)).AssertOnlyViolations(helper);
            should.Be(Types().That().DoNotResideInAssembly(archAssembly)).AssertOnlyViolations(helper);
            should.Be(Types().That().DoNotResideInAssembly(assemblyFullName)).AssertOnlyViolations(helper);
            should.Be(Types().That().DoNotResideInAssemblyMatching("TypeAssembly.*")).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Types().That().Are(helper.RegularClass, helper.OtherRegularClass).Should().NotResideInAssembly("NonExistentAssembly").AssertNoViolations(helper);

            helper.AddSnapshotHeader("Multiple assemblies");
            should = Types().That().Are(helper.RegularClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotResideInAssembly(archAssembly, archAssembly).AssertOnlyViolations(helper);
            should.NotResideInAssembly(typeAssembly, typeAssembly).AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().DoNotResideInAssembly(archAssembly, archAssembly)).AssertOnlyViolations(helper);
            should.Be(Types().That().DoNotResideInAssembly(typeAssembly, typeAssembly)).AssertOnlyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task HavePropertyMemberWithNameTest()
        {
            var helper = new TypeAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = Types().That().Are(helper.ClassWithProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HavePropertyMemberWithName("PropertyA").AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().HavePropertyMemberWithName("PropertyA")).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = Types().That().Are(helper.ClassWithoutMembers).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HavePropertyMemberWithName("PropertyA").AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().HavePropertyMemberWithName("PropertyA")).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Types().That().Are(helper.ClassWithProperty, helper.ClassWithoutMembers).Should().HavePropertyMemberWithName("PropertyA").AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotHavePropertyMemberWithNameTest()
        {
            var helper = new TypeAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = Types().That().Are(helper.ClassWithoutMembers).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHavePropertyMemberWithName("PropertyA").AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().DoNotHavePropertyMemberWithName("PropertyA")).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = Types().That().Are(helper.ClassWithProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHavePropertyMemberWithName("PropertyA").AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().DoNotHavePropertyMemberWithName("PropertyA")).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Types().That().Are(helper.ClassWithoutMembers, helper.ClassWithProperty).Should().NotHavePropertyMemberWithName("PropertyA").AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task HaveFieldMemberWithNameTest()
        {
            var helper = new TypeAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = Types().That().Are(helper.ClassWithField).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HaveFieldMemberWithName("FieldA").AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().HaveFieldMemberWithName("FieldA")).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = Types().That().Are(helper.ClassWithoutMembers).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HaveFieldMemberWithName("FieldA").AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().HaveFieldMemberWithName("FieldA")).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Types().That().Are(helper.ClassWithField, helper.ClassWithoutMembers).Should().HaveFieldMemberWithName("FieldA").AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotHaveFieldMemberWithNameTest()
        {
            var helper = new TypeAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = Types().That().Are(helper.ClassWithoutMembers).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHaveFieldMemberWithName("FieldA").AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().DoNotHaveFieldMemberWithName("FieldA")).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = Types().That().Are(helper.ClassWithField).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHaveFieldMemberWithName("FieldA").AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().DoNotHaveFieldMemberWithName("FieldA")).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Types().That().Are(helper.ClassWithoutMembers, helper.ClassWithField).Should().NotHaveFieldMemberWithName("FieldA").AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task HaveMethodMemberWithNameTest()
        {
            var helper = new TypeAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = Types().That().Are(helper.ClassWithMethod).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HaveMethodMemberWithName("MethodA()").AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().HaveMethodMemberWithName("MethodA()")).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = Types().That().Are(helper.ClassWithoutMembers).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HaveMethodMemberWithName("MethodA()").AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().HaveMethodMemberWithName("MethodA()")).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Types().That().Are(helper.ClassWithMethod, helper.ClassWithoutMembers).Should().HaveMethodMemberWithName("MethodA()").AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotHaveMethodMemberWithNameTest()
        {
            var helper = new TypeAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = Types().That().Are(helper.ClassWithoutMembers).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHaveMethodMemberWithName("MethodA()").AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().DoNotHaveMethodMemberWithName("MethodA()")).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = Types().That().Are(helper.ClassWithMethod).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHaveMethodMemberWithName("MethodA()").AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().DoNotHaveMethodMemberWithName("MethodA()")).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Types().That().Are(helper.ClassWithoutMembers, helper.ClassWithMethod).Should().NotHaveMethodMemberWithName("MethodA()").AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task HaveMemberWithNameTest()
        {
            var helper = new TypeAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = Types().That().Are(helper.ClassWithAllMembers).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HaveMemberWithName("PropertyB").AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().HaveMemberWithName("PropertyB")).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = Types().That().Are(helper.ClassWithoutMembers).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HaveMemberWithName("PropertyB").AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().HaveMemberWithName("PropertyB")).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Types().That().Are(helper.ClassWithAllMembers, helper.ClassWithoutMembers).Should().HaveMemberWithName("PropertyB").AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotHaveMemberWithNameTest()
        {
            var helper = new TypeAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = Types().That().Are(helper.ClassWithoutMembers).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHaveMemberWithName("PropertyB").AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().DoNotHaveMemberWithName("PropertyB")).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = Types().That().Are(helper.ClassWithAllMembers).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHaveMemberWithName("PropertyB").AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Types().That().DoNotHaveMemberWithName("PropertyB")).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Types().That().Are(helper.ClassWithoutMembers, helper.ClassWithAllMembers).Should().NotHaveMemberWithName("PropertyB").AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task BeNestedTest()
        {
            var helper = new TypeAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            helper.AddSnapshotSubHeader("Conditions");
            Types().That().Are(helper.InnerClassA).Should().BeNested().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            Types().That().Are(helper.InnerClassA).Should().Be(Types().That().AreNested()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            helper.AddSnapshotSubHeader("Conditions");
            Types().That().Are(helper.NonNestedClass).Should().BeNested().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            Types().That().Are(helper.NonNestedClass).Should().Be(Types().That().AreNested()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Types().That().Are(helper.InnerClassA, helper.OtherInnerClassA).Should().BeNested().AssertNoViolations(helper);
            Types().That().Are(helper.InnerClassA, helper.NonNestedClass).Should().BeNested().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotBeNestedTest()
        {
            var helper = new TypeAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            helper.AddSnapshotSubHeader("Conditions");
            Types().That().Are(helper.NonNestedClass).Should().NotBeNested().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            Types().That().Are(helper.NonNestedClass).Should().Be(Types().That().AreNotNested()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            helper.AddSnapshotSubHeader("Conditions");
            Types().That().Are(helper.InnerClassA).Should().NotBeNested().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            Types().That().Are(helper.InnerClassA).Should().Be(Types().That().AreNotNested()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Types().That().Are(helper.NonNestedClass, helper.RegularClass).Should().NotBeNested().AssertNoViolations(helper);
            Types().That().Are(helper.NonNestedClass, helper.InnerClassA).Should().NotBeNested().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public void AdhereToPlantUmlDiagramStreamTest()
        {
            var architecture = new ArchLoader()
                .LoadNamespacesWithinAssembly(
                    typeof(IndependentClass).Assembly,
                    "TestAssembly.Diagram.NoDependencies"
                )
                .Build();

            // No violations: diagram correctly represents no cross-namespace dependencies
            using (var stream = new MemoryStream())
            {
                TestDiagram.From(stream)
                    .Component("A")
                    .WithStereoTypes("TestAssembly.Diagram.NoDependencies.Independent.*")
                    .Component("B")
                    .WithStereoTypes("TestAssembly.Diagram.NoDependencies.SomeNamespace.*")
                    .Write();

                var rule = Types().Should().AdhereToPlantUmlDiagram(stream);
                Assert.True(rule.HasNoViolations(architecture));
            }

            // No violations: defined but unused dependency is allowed
            using (var stream = new MemoryStream())
            {
                TestDiagram.From(stream)
                    .Component("A")
                    .WithStereoTypes("TestAssembly.Diagram.NoDependencies.Independent.*")
                    .Component("B")
                    .WithStereoTypes("TestAssembly.Diagram.NoDependencies.SomeNamespace.*")
                    .DependencyFrom("A").To("B")
                    .Write();

                var rule = Types().Should().AdhereToPlantUmlDiagram(stream);
                Assert.True(rule.HasNoViolations(architecture));
            }

            // Violations: SimpleDependency has a real cross-namespace dependency without a diagram arrow
            var simpleDependencyArchitecture = new ArchLoader()
                .LoadNamespacesWithinAssembly(
                    typeof(IndependentClass).Assembly,
                    "TestAssembly.Diagram.SimpleDependency"
                )
                .Build();

            using (var stream = new MemoryStream())
            {
                TestDiagram.From(stream)
                    .Component("Origin")
                    .WithStereoTypes("TestAssembly.Diagram.SimpleDependency.Origin.*")
                    .Component("Target")
                    .WithStereoTypes("TestAssembly.Diagram.SimpleDependency.Target.*")
                    .Write();

                var rule = Types().Should().AdhereToPlantUmlDiagram(stream);
                Assert.False(rule.HasNoViolations(simpleDependencyArchitecture));
            }

            // No violations: SimpleDependency with correct arrow
            using (var stream = new MemoryStream())
            {
                TestDiagram.From(stream)
                    .Component("Origin")
                    .WithStereoTypes("TestAssembly.Diagram.SimpleDependency.Origin.*")
                    .Component("Target")
                    .WithStereoTypes("TestAssembly.Diagram.SimpleDependency.Target.*")
                    .DependencyFrom("Origin").To("Target")
                    .Write();

                var rule = Types().Should().AdhereToPlantUmlDiagram(stream);
                Assert.True(rule.HasNoViolations(simpleDependencyArchitecture));
            }
        }

        [Fact]
        public void AdhereToPlantUmlDiagramFileTest()
        {
            var architecture = new ArchLoader()
                .LoadNamespacesWithinAssembly(
                    typeof(IndependentClass).Assembly,
                    "TestAssembly.Diagram.SimpleDependency"
                )
                .Build();

            string path = Path.Combine(
                Path.GetTempPath(),
                "plantuml_type_test_" + Guid.NewGuid() + ".puml"
            );

            try
            {
                // No violations: file overload with correct dependency arrow
                using (FileStream fileStream = File.Create(path))
                {
                    TestDiagram.From(fileStream)
                        .Component("Origin")
                        .WithStereoTypes("TestAssembly.Diagram.SimpleDependency.Origin.*")
                        .Component("Target")
                        .WithStereoTypes("TestAssembly.Diagram.SimpleDependency.Target.*")
                        .DependencyFrom("Origin").To("Target")
                        .Write();
                }

                var rule = Types().Should().AdhereToPlantUmlDiagram(path);
                Assert.True(rule.HasNoViolations(architecture));
            }
            finally
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
        }
    }
}
