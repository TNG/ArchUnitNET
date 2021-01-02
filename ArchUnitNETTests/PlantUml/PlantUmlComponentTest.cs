using ArchUnitNET.PlantUml;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ArchUnitNETTests.PlantUml
{
    public class PlantUmlComponentTest
    {
        [Fact]
        public void TwoComponentsWithSameNameStereoTypeAndAliasShouldBeEqual()
        {
            var component1 = new PlantUmlComponent(new ComponentName("someName"), new HashSet<Stereotype>() { new Stereotype("someStereoType") }, new Alias("someAlias"));
            var component2 = new PlantUmlComponent(new ComponentName("someName"), new HashSet<Stereotype>() { new Stereotype("someStereoType") }, new Alias("someAlias"));

            Assert.Equal(component1, component2);
        }

        [Fact]
        public void TwoNonIdenticalComponentsShouldNotBeEqual()
        {
            var component1 = new PlantUmlComponent(new ComponentName("someName1"), new HashSet<Stereotype>() { new Stereotype("someStereoType") }, new Alias("someAlias"));
            var component2 = new PlantUmlComponent(new ComponentName("someName2"), new HashSet<Stereotype>() { new Stereotype("someStereoType") }, new Alias("someAlias"));

            Assert.NotEqual(component1, component2);
        }

        [Fact]
        public void TwoComponentsWithSameNameStereoTypeAndAliasHaveSameHashCode()
        {
            var component1 = new PlantUmlComponent(new ComponentName("someName"), new HashSet<Stereotype>() { new Stereotype("someStereoType") }, new Alias("someAlias"));
            var component2 = new PlantUmlComponent(new ComponentName("someName"), new HashSet<Stereotype>() { new Stereotype("someStereoType") }, new Alias("someAlias"));

            Assert.Equal(component1.GetHashCode(), component2.GetHashCode());
        }
    }
}
