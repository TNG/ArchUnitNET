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
using System.Text;
using System.Xml;
using System.Xml.Linq;
using ArchUnitNET.Domain;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Freeze
{
    public class XmlViolationStore : IViolationStore
    {
        private const string DefaultStoragePath = "../../../ArchUnitNET/Storage/FrozenRules.xml";
        private readonly string _storagePath;

        private static readonly XmlWriterSettings WriterSettings = new XmlWriterSettings
            { Indent = true, Encoding = Encoding.UTF8 };

        public XmlViolationStore(string storagePath = DefaultStoragePath)
        {
            _storagePath = storagePath;
        }

        public bool RuleAlreadyFrozen(IArchRule rule)
        {
            var storageDoc = LoadStorage();
            var storedRule = FindStoredRule(storageDoc, rule);
            return storedRule != null;
        }

        public IEnumerable<StringIdentifier> GetFrozenViolations(IArchRule rule)
        {
            var storageDoc = LoadStorage();
            var storedRule = FindStoredRule(storageDoc, rule);

            if (storedRule == null) //rule not stored
            {
                yield break;
            }

            foreach (var xElement in storedRule.Elements())
            {
                yield return new StringIdentifier(xElement.Value);
            }
        }

        public void StoreCurrentViolations(IArchRule rule, IEnumerable<StringIdentifier> violations)
        {
            var directory = Path.GetDirectoryName(_storagePath);

            if (directory == null)
            {
                throw new ArgumentException("Invalid Path");
            }

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var violationElements =
                violations.Select(violation =>
                    new XElement("Violation", violation.Identifier));
            var ruleElement = new XElement("FrozenRule", violationElements);
            ruleElement.SetAttributeValue("ArchRule", rule.Description);

            var storageDoc = LoadStorage();
            var storedRule = FindStoredRule(storageDoc, rule);
            storedRule?.Remove();
            storageDoc.Root.Add(ruleElement);

            using (var writer = XmlWriter.Create(DefaultStoragePath, WriterSettings))
            {
                storageDoc.WriteTo(writer);
            }
        }

        [CanBeNull]
        private static XElement FindStoredRule(XDocument xDocument, IArchRule rule)
        {
            return xDocument.Root?.Elements().FirstOrDefault(element =>
                element.Attribute("ArchRule")?.Value.ToString() == rule.Description);
        }

        private XDocument LoadStorage()
        {
            try
            {
                var doc = XDocument.Load(_storagePath);
                return doc.Root?.Name == "FrozenRules" ? doc : new XDocument(new XElement("FrozenRules"));
            }
            catch (Exception) //file not found or invalid xml document
            {
                return new XDocument(new XElement("FrozenRules"));
            }
        }
    }
}