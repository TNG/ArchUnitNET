using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.xUnit;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Domain.PlantUml
{
    public class PlantUmlErrorMessagesCheck
    {
        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture;
        private readonly string _umlFile;

        public PlantUmlErrorMessagesCheck()
        {
            _umlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Domain", "PlantUml",
                "zzz_test_version_with_errors.puml");
        }

        [Fact]
        public void NoDuplicatesInErrorMessageTest()
        {
            var testPassed = CheckByPuml(out var rawErrormessage);
            Assert.False(testPassed);

            //CheckForDuplicates returns false when errormessage contains duplicates or is empty
            var containsNoDuplicates = ContainsNoDuplicates(rawErrormessage, out var explainErrormessage);

            var errormessage = "\nOriginal (ArchUnitNet) Exception:\n" + rawErrormessage +
                               "\n\nAssert Error:\n" + explainErrormessage + "\n";

            Assert.True(containsNoDuplicates, errormessage);
        }

        private bool CheckByPuml(out string errormessage)
        {
            errormessage = null;

            try
            {
                IArchRule adhereToPlantUmlDiagram = Types().Should().AdhereToPlantUmlDiagram(_umlFile);
                adhereToPlantUmlDiagram.Check(Architecture);
            }
            //xUnit
            catch (FailedArchRuleException exception)
            {
                errormessage = exception.Message;

                return false;
            }

            return true;
        }

        private static bool ContainsNoDuplicates(string uncutMessage, out string errormessage)
        {
            if (string.IsNullOrWhiteSpace(uncutMessage))
            {
                errormessage = "Error message is empty.";
                return false;
            }

            var sources = new List<string>();

            var errors = uncutMessage.Split('\n').Skip(1);

            foreach (var error in errors.Where(e => !string.IsNullOrWhiteSpace(e)))
            {
                var splitError = error.Split(" does depend on ");
                var source = splitError[0].Trim();
                var targets = splitError[1].Trim().Split(" and ");
                if (sources.Contains(source))
                {
                    errormessage = $"Two errors with {source} as source found.";
                    return false;
                }

                sources.Add(source);
                if (targets.Distinct().Count() < targets.Length)
                {
                    errormessage = $"The error \"{error}\" contains duplicate targets.";
                    return false;
                }
            }

            errormessage = "No duplicates found.";
            return true;
        }
    }
}