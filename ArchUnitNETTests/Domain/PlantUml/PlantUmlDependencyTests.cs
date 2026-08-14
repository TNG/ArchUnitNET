using ArchUnitNET.Domain.PlantUml.Export;
using Xunit;

namespace ArchUnitNETTests.Domain.PlantUml
{
    /// <summary>
    ///     PlantUML references a package by its bare name but a component only inside brackets.
    ///     Getting that wrong is invisible for names that happen to be plain identifiers and breaks
    ///     the diagram for everything else, so the bracketing is pinned per arrow type here.
    /// </summary>
    public class PlantUmlDependencyTests
    {
        private static string Render(string origin, string target, DependencyType type)
        {
            return new PlantUmlDependency(origin, target, type)
                .GetPlantUmlString(new RenderOptions())
                .TrimEnd();
        }

        [Theory]
        [InlineData(DependencyType.OneToOne)]
        [InlineData(DependencyType.OneToOneCompact)]
        [InlineData(DependencyType.OneToOneIfSameParentNamespace)]
        [InlineData(DependencyType.Circle)]
        public void ComponentEndsAreBracketed(DependencyType type)
        {
            var result = Render("App.Orders.Http", "App.Orders.Grpc", type);
            Assert.Contains("[App.Orders.Http]", result);
            Assert.Contains("[App.Orders.Grpc]", result);
        }

        [Fact]
        public void PackageEndsAreNotBracketed()
        {
            var result = Render(
                "App.Orders.Http",
                "App.Shipping.Grpc",
                DependencyType.PackageToOne
            );
            Assert.StartsWith("Http ", result);
            Assert.Contains("[App.Shipping.Grpc]", result);
        }

        [Fact]
        public void DeeperTargetIsReducedToItsPackage()
        {
            var result = Render(
                "App.Http",
                "App.Orders.Grpc",
                DependencyType.OneToOneIfSameParentNamespace
            );
            Assert.Equal("[App.Http] --> Orders", result);
        }

        [Fact]
        public void DeeperOriginIsReducedToItsPackage()
        {
            var result = Render(
                "App.Orders.Grpc",
                "App.Http",
                DependencyType.OneToOneIfSameParentNamespace
            );
            Assert.Equal("Orders -> [App.Http]", result);
        }

        // Bare names happen to work while they are plain identifiers, which is why this went
        // unnoticed. They stop working as soon as one is not -- ".." collides with PlantUML's own
        // dashed-arrow operator, and "*" is not an identifier character at all.
        [Theory]
        [InlineData("App.Orders..Http", "App.Orders..Grpc")]
        [InlineData("App.Orders.*.Http", "App.Orders.*.Grpc")]
        public void NamesThatAreNotIdentifiersStayInsideBrackets(string origin, string target)
        {
            var result = Render(origin, target, DependencyType.OneToOneIfSameParentNamespace);
            Assert.Equal("[" + origin + "] --|> [" + target + "]", result);
        }
    }
}
