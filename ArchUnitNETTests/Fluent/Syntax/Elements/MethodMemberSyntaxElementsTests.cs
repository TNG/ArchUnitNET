using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArchUnitNET.Domain;
using ArchUnitNETTests.AssemblyTestHelper;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements
{
    // csharpier-ignore
    public class MethodMemberSyntaxElementsTests
    {
        [Fact]
        public async Task BeConstructorTest()
        {
            var helper = new TypeAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = MethodMembers().That().Are(helper.ClassWithVirtualMethodConstructor).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.BeConstructor().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(MethodMembers().That().AreConstructors()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = MethodMembers().That().Are(helper.VirtualMethod).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.BeConstructor().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(MethodMembers().That().AreConstructors()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            MethodMembers().That().Are(helper.ClassWithVirtualMethodConstructor, helper.ClassWithNonVirtualMethodConstructor).Should().BeConstructor().AssertNoViolations(helper);
            MethodMembers().That().Are(helper.ClassWithVirtualMethodConstructor, helper.VirtualMethod).Should().BeConstructor().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task BeNoConstructorTest()
        {
            var helper = new TypeAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = MethodMembers().That().Are(helper.VirtualMethod).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.BeNoConstructor().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(MethodMembers().That().AreNoConstructors()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = MethodMembers().That().Are(helper.ClassWithVirtualMethodConstructor).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.BeNoConstructor().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(MethodMembers().That().AreNoConstructors()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            MethodMembers().That().Are(helper.VirtualMethod, helper.NonVirtualMethod).Should().BeNoConstructor().AssertNoViolations(helper);
            MethodMembers().That().Are(helper.VirtualMethod, helper.ClassWithVirtualMethodConstructor).Should().BeNoConstructor().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task BeVirtualTest()
        {
            var helper = new TypeAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = MethodMembers().That().Are(helper.VirtualMethod).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.BeVirtual().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(MethodMembers().That().AreVirtual()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = MethodMembers().That().Are(helper.NonVirtualMethod).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.BeVirtual().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(MethodMembers().That().AreVirtual()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            MethodMembers().That().Are(helper.VirtualMethod, helper.OtherVirtualMethod).Should().BeVirtual().AssertNoViolations(helper);
            MethodMembers().That().Are(helper.VirtualMethod, helper.NonVirtualMethod).Should().BeVirtual().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotBeVirtualTest()
        {
            var helper = new TypeAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = MethodMembers().That().Are(helper.NonVirtualMethod).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotBeVirtual().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(MethodMembers().That().AreNotVirtual()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = MethodMembers().That().Are(helper.VirtualMethod).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotBeVirtual().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(MethodMembers().That().AreNotVirtual()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            MethodMembers().That().Are(helper.NonVirtualMethod, helper.MethodReturningString).Should().NotBeVirtual().AssertNoViolations(helper);
            MethodMembers().That().Are(helper.NonVirtualMethod, helper.VirtualMethod).Should().NotBeVirtual().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task BeCalledByTest()
        {
            var helper = new MethodDependencyAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = MethodMembers().That().Are(helper.CalledMethod).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.BeCalledBy(helper.MethodDependencyClass).AssertNoViolations(helper);
            should.BeCalledBy(helper.MethodDependencyClassSystemType).AssertNoViolations(helper);
            should.BeCalledBy(Types().That().Are(helper.MethodDependencyClass)).AssertNoViolations(helper);
            should.BeCalledBy(new List<IType> { helper.MethodDependencyClass }).AssertNoViolations(helper);
            should.BeCalledBy(new List<System.Type> { helper.MethodDependencyClassSystemType }).AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(MethodMembers().That().AreCalledBy(helper.MethodDependencyClass)).AssertNoViolations(helper);
            should.Be(MethodMembers().That().AreCalledBy(helper.MethodDependencyClassSystemType)).AssertNoViolations(helper);
            should.Be(MethodMembers().That().AreCalledBy(Types().That().Are(helper.MethodDependencyClass))).AssertNoViolations(helper);
            should.Be(MethodMembers().That().AreCalledBy(new List<IType> { helper.MethodDependencyClass })).AssertNoViolations(helper);
            should.Be(MethodMembers().That().AreCalledBy(new List<System.Type> { helper.MethodDependencyClassSystemType })).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = MethodMembers().That().Are(helper.MethodWithoutDependencies).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.BeCalledBy(helper.MethodDependencyClass).AssertOnlyViolations(helper);
            should.BeCalledBy(helper.MethodDependencyClassSystemType).AssertOnlyViolations(helper);
            should.BeCalledBy(Types().That().Are(helper.MethodDependencyClass)).AssertOnlyViolations(helper);
            should.BeCalledBy(new List<IType> { helper.MethodDependencyClass }).AssertOnlyViolations(helper);
            should.BeCalledBy(new List<System.Type> { helper.MethodDependencyClassSystemType }).AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(MethodMembers().That().AreCalledBy(helper.MethodDependencyClass)).AssertOnlyViolations(helper);
            should.Be(MethodMembers().That().AreCalledBy(helper.MethodDependencyClassSystemType)).AssertOnlyViolations(helper);
            should.Be(MethodMembers().That().AreCalledBy(Types().That().Are(helper.MethodDependencyClass))).AssertOnlyViolations(helper);
            should.Be(MethodMembers().That().AreCalledBy(new List<IType> { helper.MethodDependencyClass })).AssertOnlyViolations(helper);
            should.Be(MethodMembers().That().AreCalledBy(new List<System.Type> { helper.MethodDependencyClassSystemType })).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple arguments");
            should = MethodMembers().That().Are(helper.CalledMethod).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.BeCalledBy(helper.MethodDependencyClass, helper.OtherCallingClass).AssertNoViolations(helper);
            should.BeCalledBy(new List<IType> { helper.MethodDependencyClass, helper.OtherCallingClass }).AssertNoViolations(helper);
            should.BeCalledBy(helper.MethodDependencyClassSystemType, helper.OtherCallingClassSystemType).AssertNoViolations(helper);
            should.BeCalledBy(new List<System.Type> { helper.MethodDependencyClassSystemType, helper.OtherCallingClassSystemType }).AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(MethodMembers().That().AreCalledBy(helper.MethodDependencyClass, helper.OtherCallingClass)).AssertNoViolations(helper);
            should.Be(MethodMembers().That().AreCalledBy(new List<IType> { helper.MethodDependencyClass, helper.OtherCallingClass })).AssertNoViolations(helper);
            should.Be(MethodMembers().That().AreCalledBy(helper.MethodDependencyClassSystemType, helper.OtherCallingClassSystemType)).AssertNoViolations(helper);
            should.Be(MethodMembers().That().AreCalledBy(new List<System.Type> { helper.MethodDependencyClassSystemType, helper.OtherCallingClassSystemType })).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Empty arguments");
            should = MethodMembers().That().Are(helper.CalledMethod).Should();

            helper.AddSnapshotSubHeader("Conditions");
            // TODO: Empty IEnumerable<IType>/<Type> crashes with InvalidOperationException
            // (.First() on empty list) — uncomment when AssertException supports lazy evaluation
            // should.BeCalledBy(new List<IType>()).AssertException<InvalidOperationException>(helper);
            // should.BeCalledBy(new List<System.Type>()).AssertException<InvalidOperationException>(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(MethodMembers().That().AreCalledBy(new List<IType>())).AssertOnlyViolations(helper);
            should.Be(MethodMembers().That().AreCalledBy(new List<System.Type>())).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Type not in architecture");
            should = MethodMembers().That().Are(helper.CalledMethod).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.BeCalledBy(new List<System.Type> { typeof(AttributeNamespace.ClassWithoutAttributes) }).AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(MethodMembers().That().AreCalledBy(new List<System.Type> { typeof(AttributeNamespace.ClassWithoutAttributes) })).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            MethodMembers().That().Are(helper.CalledMethod, helper.CalledMethod1).Should().BeCalledBy(helper.MethodDependencyClass).AssertNoViolations(helper);
            MethodMembers().That().Are(helper.CalledMethod, helper.MethodWithoutDependencies).Should().BeCalledBy(helper.MethodDependencyClass).AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotBeCalledByTest()
        {
            var helper = new MethodDependencyAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = MethodMembers().That().Are(helper.MethodWithoutDependencies).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotBeCalledBy(helper.MethodDependencyClass).AssertNoViolations(helper);
            should.NotBeCalledBy(helper.MethodDependencyClassSystemType).AssertNoViolations(helper);
            should.NotBeCalledBy(Types().That().Are(helper.MethodDependencyClass)).AssertNoViolations(helper);
            should.NotBeCalledBy(new List<IType> { helper.MethodDependencyClass }).AssertNoViolations(helper);
            should.NotBeCalledBy(new List<System.Type> { helper.MethodDependencyClassSystemType }).AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(MethodMembers().That().AreNotCalledBy(helper.MethodDependencyClass)).AssertNoViolations(helper);
            should.Be(MethodMembers().That().AreNotCalledBy(helper.MethodDependencyClassSystemType)).AssertNoViolations(helper);
            should.Be(MethodMembers().That().AreNotCalledBy(Types().That().Are(helper.MethodDependencyClass))).AssertNoViolations(helper);
            should.Be(MethodMembers().That().AreNotCalledBy(new List<IType> { helper.MethodDependencyClass })).AssertNoViolations(helper);
            should.Be(MethodMembers().That().AreNotCalledBy(new List<System.Type> { helper.MethodDependencyClassSystemType })).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = MethodMembers().That().Are(helper.CalledMethod).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotBeCalledBy(helper.MethodDependencyClass).AssertOnlyViolations(helper);
            should.NotBeCalledBy(helper.MethodDependencyClassSystemType).AssertOnlyViolations(helper);
            should.NotBeCalledBy(Types().That().Are(helper.MethodDependencyClass)).AssertOnlyViolations(helper);
            should.NotBeCalledBy(new List<IType> { helper.MethodDependencyClass }).AssertOnlyViolations(helper);
            should.NotBeCalledBy(new List<System.Type> { helper.MethodDependencyClassSystemType }).AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(MethodMembers().That().AreNotCalledBy(helper.MethodDependencyClass)).AssertOnlyViolations(helper);
            should.Be(MethodMembers().That().AreNotCalledBy(helper.MethodDependencyClassSystemType)).AssertOnlyViolations(helper);
            should.Be(MethodMembers().That().AreNotCalledBy(Types().That().Are(helper.MethodDependencyClass))).AssertOnlyViolations(helper);
            should.Be(MethodMembers().That().AreNotCalledBy(new List<IType> { helper.MethodDependencyClass })).AssertOnlyViolations(helper);
            should.Be(MethodMembers().That().AreNotCalledBy(new List<System.Type> { helper.MethodDependencyClassSystemType })).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple arguments");
            should = MethodMembers().That().Are(helper.MethodWithoutDependencies).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotBeCalledBy(helper.MethodDependencyClass, helper.OtherCallingClass).AssertNoViolations(helper);
            should.NotBeCalledBy(new List<IType> { helper.MethodDependencyClass, helper.OtherCallingClass }).AssertNoViolations(helper);
            should.NotBeCalledBy(helper.MethodDependencyClassSystemType, helper.OtherCallingClassSystemType).AssertNoViolations(helper);
            should.NotBeCalledBy(new List<System.Type> { helper.MethodDependencyClassSystemType, helper.OtherCallingClassSystemType }).AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(MethodMembers().That().AreNotCalledBy(helper.MethodDependencyClass, helper.OtherCallingClass)).AssertNoViolations(helper);
            should.Be(MethodMembers().That().AreNotCalledBy(new List<IType> { helper.MethodDependencyClass, helper.OtherCallingClass })).AssertNoViolations(helper);
            should.Be(MethodMembers().That().AreNotCalledBy(helper.MethodDependencyClassSystemType, helper.OtherCallingClassSystemType)).AssertNoViolations(helper);
            should.Be(MethodMembers().That().AreNotCalledBy(new List<System.Type> { helper.MethodDependencyClassSystemType, helper.OtherCallingClassSystemType })).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Empty arguments");
            should = MethodMembers().That().Are(helper.CalledMethod).Should();

            helper.AddSnapshotSubHeader("Conditions");
            // TODO: Empty IEnumerable<IType>/<Type> crashes with InvalidOperationException
            // (.First() on empty list) — uncomment when AssertException supports lazy evaluation
            // should.NotBeCalledBy(new List<IType>()).AssertException<InvalidOperationException>(helper);
            // should.NotBeCalledBy(new List<System.Type>()).AssertException<InvalidOperationException>(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(MethodMembers().That().AreNotCalledBy(new List<IType>())).AssertNoViolations(helper);
            should.Be(MethodMembers().That().AreNotCalledBy(new List<System.Type>())).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Type not in architecture");
            should = MethodMembers().That().Are(helper.CalledMethod).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotBeCalledBy(new List<System.Type> { typeof(AttributeNamespace.ClassWithoutAttributes) }).AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(MethodMembers().That().AreNotCalledBy(new List<System.Type> { typeof(AttributeNamespace.ClassWithoutAttributes) })).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            MethodMembers().That().Are(helper.MethodWithoutDependencies, helper.MethodCallingCalledMethod).Should().NotBeCalledBy(helper.MethodDependencyClass).AssertNoViolations(helper);
            MethodMembers().That().Are(helper.CalledMethod, helper.MethodWithoutDependencies).Should().NotBeCalledBy(helper.MethodDependencyClass).AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task HaveDependencyInMethodBodyToTest()
        {
            var helper = new MethodDependencyAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = MethodMembers().That().Are(helper.MethodCallingCalledMethod).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HaveDependencyInMethodBodyTo(helper.MethodDependencyClass).AssertNoViolations(helper);
            should.HaveDependencyInMethodBodyTo(helper.MethodDependencyClassSystemType).AssertNoViolations(helper);
            should.HaveDependencyInMethodBodyTo(Types().That().Are(helper.MethodDependencyClass)).AssertNoViolations(helper);
            should.HaveDependencyInMethodBodyTo(new List<IType> { helper.MethodDependencyClass }).AssertNoViolations(helper);
            should.HaveDependencyInMethodBodyTo(new List<System.Type> { helper.MethodDependencyClassSystemType }).AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(MethodMembers().That().HaveDependencyInMethodBodyTo(helper.MethodDependencyClass)).AssertNoViolations(helper);
            should.Be(MethodMembers().That().HaveDependencyInMethodBodyTo(helper.MethodDependencyClassSystemType)).AssertNoViolations(helper);
            should.Be(MethodMembers().That().HaveDependencyInMethodBodyTo(Types().That().Are(helper.MethodDependencyClass))).AssertNoViolations(helper);
            should.Be(MethodMembers().That().HaveDependencyInMethodBodyTo(new List<IType> { helper.MethodDependencyClass })).AssertNoViolations(helper);
            should.Be(MethodMembers().That().HaveDependencyInMethodBodyTo(new List<System.Type> { helper.MethodDependencyClassSystemType })).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = MethodMembers().That().Are(helper.MethodWithoutDependencies).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HaveDependencyInMethodBodyTo(helper.OtherCallingClass).AssertOnlyViolations(helper);
            should.HaveDependencyInMethodBodyTo(helper.OtherCallingClassSystemType).AssertOnlyViolations(helper);
            should.HaveDependencyInMethodBodyTo(Types().That().Are(helper.OtherCallingClass)).AssertOnlyViolations(helper);
            should.HaveDependencyInMethodBodyTo(new List<IType> { helper.OtherCallingClass }).AssertOnlyViolations(helper);
            should.HaveDependencyInMethodBodyTo(new List<System.Type> { helper.OtherCallingClassSystemType }).AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(MethodMembers().That().HaveDependencyInMethodBodyTo(helper.OtherCallingClass)).AssertOnlyViolations(helper);
            should.Be(MethodMembers().That().HaveDependencyInMethodBodyTo(helper.OtherCallingClassSystemType)).AssertOnlyViolations(helper);
            should.Be(MethodMembers().That().HaveDependencyInMethodBodyTo(Types().That().Are(helper.OtherCallingClass))).AssertOnlyViolations(helper);
            should.Be(MethodMembers().That().HaveDependencyInMethodBodyTo(new List<IType> { helper.OtherCallingClass })).AssertOnlyViolations(helper);
            should.Be(MethodMembers().That().HaveDependencyInMethodBodyTo(new List<System.Type> { helper.OtherCallingClassSystemType })).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple arguments");
            should = MethodMembers().That().Are(helper.MethodCallingCalledMethod).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HaveDependencyInMethodBodyTo(helper.MethodDependencyClass, helper.OtherCallingClass).AssertNoViolations(helper);
            should.HaveDependencyInMethodBodyTo(new List<IType> { helper.MethodDependencyClass, helper.OtherCallingClass }).AssertNoViolations(helper);
            should.HaveDependencyInMethodBodyTo(helper.MethodDependencyClassSystemType, helper.OtherCallingClassSystemType).AssertNoViolations(helper);
            should.HaveDependencyInMethodBodyTo(new List<System.Type> { helper.MethodDependencyClassSystemType, helper.OtherCallingClassSystemType }).AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(MethodMembers().That().HaveDependencyInMethodBodyTo(helper.MethodDependencyClass, helper.OtherCallingClass)).AssertNoViolations(helper);
            should.Be(MethodMembers().That().HaveDependencyInMethodBodyTo(new List<IType> { helper.MethodDependencyClass, helper.OtherCallingClass })).AssertNoViolations(helper);
            should.Be(MethodMembers().That().HaveDependencyInMethodBodyTo(helper.MethodDependencyClassSystemType, helper.OtherCallingClassSystemType)).AssertNoViolations(helper);
            should.Be(MethodMembers().That().HaveDependencyInMethodBodyTo(new List<System.Type> { helper.MethodDependencyClassSystemType, helper.OtherCallingClassSystemType })).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Empty arguments");
            should = MethodMembers().That().Are(helper.MethodCallingCalledMethod).Should();

            helper.AddSnapshotSubHeader("Conditions");
            // TODO: Empty IEnumerable<IType>/<Type> crashes with InvalidOperationException
            // (.First() on empty list) — uncomment when AssertException supports lazy evaluation
            // should.HaveDependencyInMethodBodyTo(new List<IType>()).AssertException<InvalidOperationException>(helper);
            // should.HaveDependencyInMethodBodyTo(new List<System.Type>()).AssertException<InvalidOperationException>(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(MethodMembers().That().HaveDependencyInMethodBodyTo(new List<IType>())).AssertOnlyViolations(helper);
            should.Be(MethodMembers().That().HaveDependencyInMethodBodyTo(new List<System.Type>())).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Type not in architecture");
            should = MethodMembers().That().Are(helper.MethodCallingCalledMethod).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HaveDependencyInMethodBodyTo(new List<System.Type> { typeof(AttributeNamespace.ClassWithoutAttributes) }).AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(MethodMembers().That().HaveDependencyInMethodBodyTo(new List<System.Type> { typeof(AttributeNamespace.ClassWithoutAttributes) })).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            MethodMembers().That().Are(helper.MethodCallingCalledMethod, helper.AnotherMethodCallingCalledMethod).Should().HaveDependencyInMethodBodyTo(helper.MethodDependencyClass).AssertNoViolations(helper);
            MethodMembers().That().Are(helper.MethodCallingCalledMethod, helper.MethodWithoutDependencies).Should().HaveDependencyInMethodBodyTo(helper.MethodDependencyClass).AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotHaveDependencyInMethodBodyToTest()
        {
            var helper = new MethodDependencyAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = MethodMembers().That().Are(helper.MethodWithoutDependencies).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHaveDependencyInMethodBodyTo(helper.OtherCallingClass).AssertNoViolations(helper);
            should.NotHaveDependencyInMethodBodyTo(helper.OtherCallingClassSystemType).AssertNoViolations(helper);
            should.NotHaveDependencyInMethodBodyTo(Types().That().Are(helper.OtherCallingClass)).AssertNoViolations(helper);
            should.NotHaveDependencyInMethodBodyTo(new List<IType> { helper.OtherCallingClass }).AssertNoViolations(helper);
            should.NotHaveDependencyInMethodBodyTo(new List<System.Type> { helper.OtherCallingClassSystemType }).AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(MethodMembers().That().DoNotHaveDependencyInMethodBodyTo(helper.OtherCallingClass)).AssertNoViolations(helper);
            should.Be(MethodMembers().That().DoNotHaveDependencyInMethodBodyTo(helper.OtherCallingClassSystemType)).AssertNoViolations(helper);
            should.Be(MethodMembers().That().DoNotHaveDependencyInMethodBodyTo(Types().That().Are(helper.OtherCallingClass))).AssertNoViolations(helper);
            should.Be(MethodMembers().That().DoNotHaveDependencyInMethodBodyTo(new List<IType> { helper.OtherCallingClass })).AssertNoViolations(helper);
            should.Be(MethodMembers().That().DoNotHaveDependencyInMethodBodyTo(new List<System.Type> { helper.OtherCallingClassSystemType })).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = MethodMembers().That().Are(helper.MethodCallingCalledMethod).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHaveDependencyInMethodBodyTo(helper.MethodDependencyClass).AssertOnlyViolations(helper);
            should.NotHaveDependencyInMethodBodyTo(helper.MethodDependencyClassSystemType).AssertOnlyViolations(helper);
            should.NotHaveDependencyInMethodBodyTo(Types().That().Are(helper.MethodDependencyClass)).AssertOnlyViolations(helper);
            should.NotHaveDependencyInMethodBodyTo(new List<IType> { helper.MethodDependencyClass }).AssertOnlyViolations(helper);
            should.NotHaveDependencyInMethodBodyTo(new List<System.Type> { helper.MethodDependencyClassSystemType }).AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(MethodMembers().That().DoNotHaveDependencyInMethodBodyTo(helper.MethodDependencyClass)).AssertOnlyViolations(helper);
            should.Be(MethodMembers().That().DoNotHaveDependencyInMethodBodyTo(helper.MethodDependencyClassSystemType)).AssertOnlyViolations(helper);
            should.Be(MethodMembers().That().DoNotHaveDependencyInMethodBodyTo(Types().That().Are(helper.MethodDependencyClass))).AssertOnlyViolations(helper);
            should.Be(MethodMembers().That().DoNotHaveDependencyInMethodBodyTo(new List<IType> { helper.MethodDependencyClass })).AssertOnlyViolations(helper);
            should.Be(MethodMembers().That().DoNotHaveDependencyInMethodBodyTo(new List<System.Type> { helper.MethodDependencyClassSystemType })).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple arguments");
            should = MethodMembers().That().Are(helper.MethodWithoutDependencies).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHaveDependencyInMethodBodyTo(helper.MethodDependencyClass, helper.OtherCallingClass).AssertNoViolations(helper);
            should.NotHaveDependencyInMethodBodyTo(new List<IType> { helper.MethodDependencyClass, helper.OtherCallingClass }).AssertNoViolations(helper);
            should.NotHaveDependencyInMethodBodyTo(helper.MethodDependencyClassSystemType, helper.OtherCallingClassSystemType).AssertNoViolations(helper);
            should.NotHaveDependencyInMethodBodyTo(new List<System.Type> { helper.MethodDependencyClassSystemType, helper.OtherCallingClassSystemType }).AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(MethodMembers().That().DoNotHaveDependencyInMethodBodyTo(helper.MethodDependencyClass, helper.OtherCallingClass)).AssertNoViolations(helper);
            should.Be(MethodMembers().That().DoNotHaveDependencyInMethodBodyTo(new List<IType> { helper.MethodDependencyClass, helper.OtherCallingClass })).AssertNoViolations(helper);
            should.Be(MethodMembers().That().DoNotHaveDependencyInMethodBodyTo(helper.MethodDependencyClassSystemType, helper.OtherCallingClassSystemType)).AssertNoViolations(helper);
            should.Be(MethodMembers().That().DoNotHaveDependencyInMethodBodyTo(new List<System.Type> { helper.MethodDependencyClassSystemType, helper.OtherCallingClassSystemType })).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Empty arguments");
            should = MethodMembers().That().Are(helper.MethodCallingCalledMethod).Should();

            helper.AddSnapshotSubHeader("Conditions");
            // TODO: Empty IEnumerable<IType>/<Type> crashes with InvalidOperationException
            // (.First() on empty list) — uncomment when AssertException supports lazy evaluation
            // should.NotHaveDependencyInMethodBodyTo(new List<IType>()).AssertException<InvalidOperationException>(helper);
            // should.NotHaveDependencyInMethodBodyTo(new List<System.Type>()).AssertException<InvalidOperationException>(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(MethodMembers().That().DoNotHaveDependencyInMethodBodyTo(new List<IType>())).AssertNoViolations(helper);
            should.Be(MethodMembers().That().DoNotHaveDependencyInMethodBodyTo(new List<System.Type>())).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Type not in architecture");
            should = MethodMembers().That().Are(helper.MethodCallingCalledMethod).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHaveDependencyInMethodBodyTo(new List<System.Type> { typeof(AttributeNamespace.ClassWithoutAttributes) }).AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(MethodMembers().That().DoNotHaveDependencyInMethodBodyTo(new List<System.Type> { typeof(AttributeNamespace.ClassWithoutAttributes) })).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            MethodMembers().That().Are(helper.MethodWithoutDependencies, helper.CalledMethod).Should().NotHaveDependencyInMethodBodyTo(helper.OtherCallingClass).AssertNoViolations(helper);
            MethodMembers().That().Are(helper.MethodCallingCalledMethod, helper.MethodWithoutDependencies).Should().NotHaveDependencyInMethodBodyTo(helper.MethodDependencyClass).AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task HaveReturnTypeTest()
        {
            var helper = new TypeAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = MethodMembers().That().Are(helper.MethodReturningRegularClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HaveReturnType(helper.RegularClass).AssertNoViolations(helper);
            should.HaveReturnType(helper.RegularClassSystemType).AssertNoViolations(helper);
            should.HaveReturnType(Types().That().Are(helper.RegularClass)).AssertNoViolations(helper);
            should.HaveReturnType(new List<IType> { helper.RegularClass }).AssertNoViolations(helper);
            should.HaveReturnType(new List<System.Type> { helper.RegularClassSystemType }).AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(MethodMembers().That().HaveReturnType(helper.RegularClass)).AssertNoViolations(helper);
            should.Be(MethodMembers().That().HaveReturnType(helper.RegularClassSystemType)).AssertNoViolations(helper);
            should.Be(MethodMembers().That().HaveReturnType(Types().That().Are(helper.RegularClass))).AssertNoViolations(helper);
            should.Be(MethodMembers().That().HaveReturnType(new List<IType> { helper.RegularClass })).AssertNoViolations(helper);
            should.Be(MethodMembers().That().HaveReturnType(new List<System.Type> { helper.RegularClassSystemType })).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = MethodMembers().That().Are(helper.MethodReturningRegularClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HaveReturnType(helper.OtherRegularClass).AssertOnlyViolations(helper);
            should.HaveReturnType(helper.OtherRegularClassSystemType).AssertOnlyViolations(helper);
            should.HaveReturnType(Types().That().Are(helper.OtherRegularClass)).AssertOnlyViolations(helper);
            should.HaveReturnType(new List<IType> { helper.OtherRegularClass }).AssertOnlyViolations(helper);
            should.HaveReturnType(new List<System.Type> { helper.OtherRegularClassSystemType }).AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(MethodMembers().That().HaveReturnType(helper.OtherRegularClass)).AssertOnlyViolations(helper);
            should.Be(MethodMembers().That().HaveReturnType(helper.OtherRegularClassSystemType)).AssertOnlyViolations(helper);
            should.Be(MethodMembers().That().HaveReturnType(Types().That().Are(helper.OtherRegularClass))).AssertOnlyViolations(helper);
            should.Be(MethodMembers().That().HaveReturnType(new List<IType> { helper.OtherRegularClass })).AssertOnlyViolations(helper);
            should.Be(MethodMembers().That().HaveReturnType(new List<System.Type> { helper.OtherRegularClassSystemType })).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple arguments");
            should = MethodMembers().That().Are(helper.MethodReturningRegularClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HaveReturnType(helper.RegularClass, helper.OtherRegularClass).AssertNoViolations(helper);
            should.HaveReturnType(new List<IType> { helper.RegularClass, helper.OtherRegularClass }).AssertNoViolations(helper);
            should.HaveReturnType(helper.RegularClassSystemType, helper.OtherRegularClassSystemType).AssertNoViolations(helper);
            should.HaveReturnType(new List<System.Type> { helper.RegularClassSystemType, helper.OtherRegularClassSystemType }).AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(MethodMembers().That().HaveReturnType(helper.RegularClass, helper.OtherRegularClass)).AssertNoViolations(helper);
            should.Be(MethodMembers().That().HaveReturnType(new List<IType> { helper.RegularClass, helper.OtherRegularClass })).AssertNoViolations(helper);
            should.Be(MethodMembers().That().HaveReturnType(helper.RegularClassSystemType, helper.OtherRegularClassSystemType)).AssertNoViolations(helper);
            should.Be(MethodMembers().That().HaveReturnType(new List<System.Type> { helper.RegularClassSystemType, helper.OtherRegularClassSystemType })).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            MethodMembers().That().Are(helper.MethodReturningRegularClass, helper.MethodReturningOtherRegularClass).Should().HaveReturnType(helper.RegularClass).AssertAnyViolations(helper);
            MethodMembers().That().Are(helper.MethodReturningRegularClass, helper.MethodReturningOtherRegularClass).Should().HaveReturnType(helper.RegularClass, helper.OtherRegularClass).AssertNoViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task DoNotHaveReturnTypeTest()
        {
            var helper = new TypeAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = MethodMembers().That().Are(helper.MethodReturningRegularClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHaveReturnType(helper.OtherRegularClass).AssertNoViolations(helper);
            should.NotHaveReturnType(helper.OtherRegularClassSystemType).AssertNoViolations(helper);
            should.NotHaveReturnType(Types().That().Are(helper.OtherRegularClass)).AssertNoViolations(helper);
            should.NotHaveReturnType(new List<IType> { helper.OtherRegularClass }).AssertNoViolations(helper);
            should.NotHaveReturnType(new List<System.Type> { helper.OtherRegularClassSystemType }).AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(MethodMembers().That().DoNotHaveReturnType(helper.OtherRegularClass)).AssertNoViolations(helper);
            should.Be(MethodMembers().That().DoNotHaveReturnType(helper.OtherRegularClassSystemType)).AssertNoViolations(helper);
            should.Be(MethodMembers().That().DoNotHaveReturnType(Types().That().Are(helper.OtherRegularClass))).AssertNoViolations(helper);
            should.Be(MethodMembers().That().DoNotHaveReturnType(new List<IType> { helper.OtherRegularClass })).AssertNoViolations(helper);
            should.Be(MethodMembers().That().DoNotHaveReturnType(new List<System.Type> { helper.OtherRegularClassSystemType })).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = MethodMembers().That().Are(helper.MethodReturningRegularClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHaveReturnType(helper.RegularClass).AssertOnlyViolations(helper);
            should.NotHaveReturnType(helper.RegularClassSystemType).AssertOnlyViolations(helper);
            should.NotHaveReturnType(Types().That().Are(helper.RegularClass)).AssertOnlyViolations(helper);
            should.NotHaveReturnType(new List<IType> { helper.RegularClass }).AssertOnlyViolations(helper);
            should.NotHaveReturnType(new List<System.Type> { helper.RegularClassSystemType }).AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(MethodMembers().That().DoNotHaveReturnType(helper.RegularClass)).AssertOnlyViolations(helper);
            should.Be(MethodMembers().That().DoNotHaveReturnType(helper.RegularClassSystemType)).AssertOnlyViolations(helper);
            should.Be(MethodMembers().That().DoNotHaveReturnType(Types().That().Are(helper.RegularClass))).AssertOnlyViolations(helper);
            should.Be(MethodMembers().That().DoNotHaveReturnType(new List<IType> { helper.RegularClass })).AssertOnlyViolations(helper);
            should.Be(MethodMembers().That().DoNotHaveReturnType(new List<System.Type> { helper.RegularClassSystemType })).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple arguments");
            should = MethodMembers().That().Are(helper.MethodReturningRegularClass).Should();

            // NotHaveReturnType(IEnumerable<IType>) condition uses .All(type => !match) — fails if ANY type matches
            // With {OtherRegularClass, RegularClass}: OtherRegularClass doesn't match (true) AND RegularClass matches (false) => All = false => violation
            helper.AddSnapshotSubHeader("Conditions");
            should.NotHaveReturnType(helper.OtherRegularClass, helper.RegularClass).AssertOnlyViolations(helper);
            should.NotHaveReturnType(new List<IType> { helper.OtherRegularClass, helper.RegularClass }).AssertOnlyViolations(helper);
            should.NotHaveReturnType(helper.OtherRegularClassSystemType, helper.RegularClassSystemType).AssertOnlyViolations(helper);
            should.NotHaveReturnType(new List<System.Type> { helper.OtherRegularClassSystemType, helper.RegularClassSystemType }).AssertOnlyViolations(helper);

            // DoNotHaveReturnType predicate uses .Any(type => !match) for IEnumerable<IType> — passes if ANY type doesn't match
            // With {OtherRegularClass, RegularClass}: OtherRegularClass doesn't match (true) => Any = true => passes
            // BUT: IEnumerable<Type> and IObjectProvider overloads use .All(type => !match) — fails if ANY type matches
            helper.AddSnapshotSubHeader("Predicates");
            should.Be(MethodMembers().That().DoNotHaveReturnType(helper.OtherRegularClass, helper.RegularClass)).AssertNoViolations(helper);
            should.Be(MethodMembers().That().DoNotHaveReturnType(new List<IType> { helper.OtherRegularClass, helper.RegularClass })).AssertNoViolations(helper);
            should.Be(MethodMembers().That().DoNotHaveReturnType(helper.OtherRegularClassSystemType, helper.RegularClassSystemType)).AssertOnlyViolations(helper);
            should.Be(MethodMembers().That().DoNotHaveReturnType(new List<System.Type> { helper.OtherRegularClassSystemType, helper.RegularClassSystemType })).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            MethodMembers().That().Are(helper.MethodReturningRegularClass, helper.MethodReturningOtherRegularClass).Should().NotHaveReturnType(helper.RegularClass).AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task BeMethodMembersThatTest()
        {
            var helper = new TypeAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            MethodMembers().That().Are(helper.ClassWithVirtualMethodConstructor).Should().BeMethodMembersThat().AreConstructors().AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            MethodMembers().That().Are(helper.VirtualMethod).Should().BeMethodMembersThat().AreConstructors().AssertOnlyViolations(helper);

            await helper.AssertSnapshotMatches();
        }
    }
}
