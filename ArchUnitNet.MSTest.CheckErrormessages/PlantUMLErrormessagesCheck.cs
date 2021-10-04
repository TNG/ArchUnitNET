using Microsoft.VisualStudio.TestTools.UnitTesting;

using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using ExampleTest.PlantUml.Addresses;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using System.IO;
using System.Linq;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace ArchUnitNet.MSTest.CheckErrormessages
{
    [TestClass]
    public class PlantUMLErrormessagesCheck
    {
        public PlantUMLErrormessagesCheck()
        {
            string exeLocation = Directory.GetCurrentDirectory();
            string[] paths = exeLocation.Split("ArchUnitNet.MSTest.CheckErrormessages");
            Filename = Path.Combine(paths[0], "ExampleTest", "Resources" , "zzz_test_version_with_errors.puml");
        }

        private static readonly Architecture Architecture =
            new ArchLoader().LoadNamespacesWithinAssembly(typeof(Address).Assembly, "ExampleTest.PlantUml").Build();

        private static string Filename { get; set; }


        [TestMethod]
        public void StartTest()
        {
            CheckByPuml(out string rawErrormessage);
            //CheckForDuplications returns false when errormessage contains no duplications
            bool isDuplications = !CheckForDuplications(rawErrormessage, out string explainErrormessage);

            string errormessage = "\nOriginal (ArchUnitNet) Exception:\n" + rawErrormessage +
                "\n\nExplained Error:\n" + explainErrormessage + "\n";

            Assert.IsTrue(isDuplications, errormessage);
        }

        public bool CheckByPuml(out string errormessage)
        {
            errormessage = null;

            try
            {
                IArchRule adhereToPlantUmlDiagram = Types().Should().AdhereToPlantUmlDiagram(Filename);
                adhereToPlantUmlDiagram.Check(Architecture);
            }
            //xUnit
            catch (FailedArchRuleException xUFAREx)
            {
                errormessage = xUFAREx.Message;

                return false;
            }

            return true;
        }

        public bool CheckForDuplications(string uncutMessage, out string errormessage)
        {
            bool isDuplications = false;
            errormessage = null;
            StringBuilder message = new();

            string[] errors = uncutMessage.Split('\n');
            
            if (CheckGroupsForDuplications(String.Empty, errors, out errors, out errormessage))
            {
                isDuplications = true;
                message.Append(errormessage);
            }

            foreach (string error in errors)
            {
                if (!String.IsNullOrWhiteSpace(error))
                {
                    string[] ands = error.Split("and");
                    string[] veryFirstError = ands[0].Split("does depend on");
                    string firstError = veryFirstError.Length == 2 ? veryFirstError[1] : String.Empty;

                    if (CheckGroupsForDuplications(firstError, ands, out ands, out errormessage))
                    {
                        isDuplications = true;
                        message.Append(errormessage);
                    }
                }
            }

            errormessage = message.ToString();
            if (String.IsNullOrWhiteSpace(errormessage))
            {
                errormessage = "\nNo Errors\n";
            }


            return isDuplications;
        }

        public bool CheckGroupsForDuplications(string firstError, string[] splittedMessages,
            out string[] filteredMessages, out string errormessage)
        {
            errormessage = null;

            var groups = splittedMessages.GroupBy(x => x.Trim().Trim('\t'));
            var duplications = groups.Where(x => !String.IsNullOrWhiteSpace(x.FirstOrDefault()) 
            && (x.Count() > 1 || firstError.Contains(x.First())));
            

            //Checks if any group contains multiple elements
            //true if it contains any duplications
            bool isGroupsDuplications = duplications.Count() != 0;

            if (isGroupsDuplications)
            {
                errormessage = GetErrormessage(firstError, splittedMessages, duplications);
            }

            filteredMessages = groups.Select(x => x.First().Trim().Trim('\t')).ToArray();

            return isGroupsDuplications;
        }

        public string GetErrormessage(string firstError, string[] originalArray, IEnumerable<IGrouping<string, string>> duplications)
        {
            StringBuilder errormessage = new();

            var array = originalArray.Select(x => "\nand " + x + " ");
            errormessage.Append("\nMessage:\n" + String.Concat(array) + "\ncontains the following duplications:\n");

            foreach (var duplication in duplications)
            {
                string duplicate = duplication.First();
                int count = duplication.Count();
                int duplicateCount = firstError.Contains(duplicate) ? ++count : count;
                errormessage.Append("Text: " + duplicate +
                    "\nNumber of duplications: " + duplicateCount + "\n");
            }

            return errormessage.ToString();
        }
    }
}
