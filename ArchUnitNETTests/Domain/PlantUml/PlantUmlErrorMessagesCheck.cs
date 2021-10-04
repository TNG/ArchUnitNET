using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.xUnit;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Domain.PlantUml
{
    public class PlantUmlErrorMessagesCheck
    {
        public PlantUmlErrorMessagesCheck()
        {
            Filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Domain", "PlantUml",
                "zzz_test_version_with_errors.puml");
        }

        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture;

        private static string Filename { get; set; }


        [Fact]
        public void NoDuplicatesInErrorMessageTest()
        {
            var testPassed = CheckByPuml(out var rawErrormessage);
            Assert.False(testPassed);

            //CheckForDuplications returns false when errormessage contains no duplications
            var isDuplications = !CheckForDuplications(rawErrormessage, out var explainErrormessage);

            var errormessage = "\nOriginal (ArchUnitNet) Exception:\n" + rawErrormessage +
                               "\n\nExplained Error:\n" + explainErrormessage + "\n";

            Assert.False(isDuplications, errormessage);
        }

        private static bool CheckByPuml(out string errormessage)
        {
            errormessage = null;

            try
            {
                IArchRule adhereToPlantUmlDiagram = Types().Should().AdhereToPlantUmlDiagram(Filename);
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

        private bool CheckForDuplications(string uncutMessage, out string errormessage)
        {
            var isDuplications = false;
            errormessage = null;
            var message = new StringBuilder();

            var errors = uncutMessage.Split('\n');

            if (CheckGroupsForDuplications(string.Empty, errors, out errors, out errormessage))
            {
                isDuplications = true;
                message.Append(errormessage);
            }

            foreach (var error in errors)
            {
                if (!string.IsNullOrWhiteSpace(error))
                {
                    var ands = error.Split("and");
                    var veryFirstError = ands[0].Split("does depend on");
                    var firstError = veryFirstError.Length == 2 ? veryFirstError[1] : string.Empty;

                    if (CheckGroupsForDuplications(firstError, ands, out ands, out errormessage))
                    {
                        isDuplications = true;
                        message.Append(errormessage);
                    }
                }
            }

            errormessage = message.ToString();
            if (string.IsNullOrWhiteSpace(errormessage))
            {
                errormessage = "\nNo Errors\n";
            }


            return isDuplications;
        }

        private static bool CheckGroupsForDuplications(string firstError, string[] splitMessages,
            out string[] filteredMessages, out string errormessage)
        {
            errormessage = null;

            var groups = splitMessages.GroupBy(x => x.Trim().Trim('\t'));
            var duplications = groups.Where(x => !string.IsNullOrWhiteSpace(x.FirstOrDefault())
                                                 && (x.Count() > 1 || firstError.Contains(x.First())));

            //Checks if any group contains multiple elements
            //true if it contains any duplications
            var isGroupsDuplications = duplications.Count() != 0;

            if (isGroupsDuplications)
            {
                errormessage = GetErrormessage(firstError, splitMessages, duplications);
            }

            filteredMessages = groups.Select(x => x.First().Trim().Trim('\t')).ToArray();

            return isGroupsDuplications;
        }

        private static string GetErrormessage(string firstError, IEnumerable<string> originalArray,
            IEnumerable<IGrouping<string, string>> duplications)
        {
            var errormessage = new StringBuilder();

            var array = originalArray.Select(x => "\nand " + x + " ");
            errormessage.Append("\nMessage:\n" + string.Concat(array) + "\ncontains the following duplications:\n");

            foreach (var duplication in duplications)
            {
                var duplicate = duplication.First();
                var count = duplication.Count();
                var duplicateCount = firstError.Contains(duplicate) ? ++count : count;
                errormessage.Append("Text: " + duplicate +
                                    "\nNumber of duplications: " + duplicateCount + "\n");
            }

            return errormessage.ToString();
        }
    }
}