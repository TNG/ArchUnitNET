//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ArchUnitNET.Domain.Exceptions;
using Newtonsoft.Json;

namespace ArchUnitNET.Fluent.Freeze
{
    public class JsonViolationStore : IViolationStore
    {
        private const string DefaultStoragePath = "..\\..\\..\\ArchUnitNET\\Storage\\FrozenRules.json";
        private readonly string _storagePath;

        public JsonViolationStore(string storagePath = DefaultStoragePath)
        {
            _storagePath = storagePath;
        }

        public bool RuleAlreadyFrozen(IArchRule rule)
        {
            var storedRules = LoadStorage();
            return storedRules.Any(r => r.ArchRuleDescription == rule.Description);
        }

        public IEnumerable<StringIdentifier> GetFrozenViolations(IArchRule rule)
        {
            var storedRules = LoadStorage();
            var matchingRules = storedRules.Where(r => r.ArchRuleDescription == rule.Description).ToList();
            if (!matchingRules.Any())
            {
                return Enumerable.Empty<StringIdentifier>();
            }

            if (matchingRules.Count > 1)
            {
                throw new MultipleOccurrencesInSequenceException("Multiple stored rules with same description found.");
            }

            return matchingRules.First().Violations.Select(violation => new StringIdentifier(violation));
        }

        public void StoreCurrentViolations(IArchRule rule, IEnumerable<StringIdentifier> violations)
        {
            var violationIdentifiers = violations.Select(violation => violation.Identifier).ToList();
            var directory = Path.GetDirectoryName(_storagePath);

            if (directory == null)
            {
                throw new ArgumentException("Invalid Path");
            }

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var storedRules = LoadStorage();
            var matchingRulesAmount = storedRules.Count(r => r.ArchRuleDescription == rule.Description);
            if (matchingRulesAmount > 1)
            {
                throw new MultipleOccurrencesInSequenceException(
                    "Multiple Rules with the same description were found in the given Json.");
            }

            if (matchingRulesAmount == 1)
            {
                storedRules.First(r => r.ArchRuleDescription == rule.Description).Violations = violationIdentifiers;
            }
            else
            {
                var frozenRule = new FrozenRule(rule.Description, violationIdentifiers);
                storedRules.Add(frozenRule);
            }

            var json = JsonConvert.SerializeObject(storedRules, Formatting.Indented);

            using (var w = new StreamWriter(_storagePath))
            {
                w.Write(json);
            }
        }

        private List<FrozenRule> LoadStorage()
        {
            try
            {
                string file;
                using (var r = new StreamReader(_storagePath))
                {
                    file = r.ReadToEnd();
                }

                var results = JsonConvert.DeserializeObject<List<FrozenRule>>(file);

                return results ?? new List<FrozenRule>();
            }
            catch (Exception)
            {
                return new List<FrozenRule>();
            }
        }
    }
}