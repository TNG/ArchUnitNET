using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Xml.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Freeze;
using Newtonsoft.Json.Linq;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent
{
    public enum StoreKind
    {
        Json,
        Xml,
    }

    public enum DuplicateKind
    {
        Type,
        Member,
    }

    /// <summary>
    /// DuplicateClassAcrossAssemblies.DuplicateClass exists in two assemblies of the loader test
    /// architecture, so its type and its constructor each have the same full name twice.
    /// </summary>
    internal static class DuplicateFullNames
    {
        public const string AssemblyA = "DuplicateFullNameAssembly";
        public const string AssemblyB = "OtherDuplicateFullNameAssembly";

        private const string TypeName = "DuplicateClassAcrossAssemblies.DuplicateClass";
        private const string ConstructorName =
            "System.Void DuplicateClassAcrossAssemblies.DuplicateClass::.ctor()";

        public static readonly Architecture Architecture =
            StaticTestArchitectures.LoaderTestArchitecture;

        // Each fails for both duplicates and for nothing else.
        private static readonly IArchRule TypeRule = Types()
            .That()
            .HaveFullName(TypeName)
            .Should()
            .BePrivate();

        private static readonly IArchRule MemberRule = MethodMembers()
            .That()
            .HaveFullName(ConstructorName)
            .Should()
            .BePrivate();

        public static string FullName(DuplicateKind kind) =>
            kind == DuplicateKind.Type ? TypeName : ConstructorName;

        public static IArchRule Rule(DuplicateKind kind) =>
            kind == DuplicateKind.Type ? TypeRule : MemberRule;

        public static string Identifier(DuplicateKind kind, string assembly) =>
            FullName(kind) + ", " + assembly;

        public static string AssemblyOf(EvaluationResult result) =>
            ((ICanBeAnalyzed)result.EvaluatedObject).Assembly.Name;
    }

    /// <summary>
    /// Creates and reads violation stores on disk. Stores are written by hand to pin the on-disk
    /// formats.
    /// </summary>
    internal static class FreezeTestStores
    {
        public static string FileName(StoreKind kind) =>
            kind == StoreKind.Json ? "FrozenRules.json" : "FrozenRules.xml";

        public static IViolationStore Create(StoreKind kind, string path)
        {
            return kind == StoreKind.Json
                ? (IViolationStore)new JsonViolationStore(path)
                : new XmlViolationStore(path);
        }

        public static string HandWritten(
            StoreKind kind,
            params (string description, string[] violations)[] rules
        )
        {
            return kind == StoreKind.Json
                ? "[\n"
                    + string.Join(
                        ",\n",
                        rules.Select(rule =>
                            "  {\n"
                            + $"    \"ArchRuleDescription\": \"{JsonEscape(rule.description)}\",\n"
                            + "    \"Violations\": ["
                            + string.Join(", ", rule.violations.Select(v => $"\"{JsonEscape(v)}\""))
                            + "]\n  }"
                        )
                    )
                    + "\n]"
                : "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<FrozenRules>\n"
                    + string.Concat(
                        rules.Select(rule =>
                            $"  <FrozenRule ArchRule=\"{SecurityElement.Escape(rule.description)}\">\n"
                            + string.Concat(
                                rule.violations.Select(v =>
                                    $"    <Violation>{SecurityElement.Escape(v)}</Violation>\n"
                                )
                            )
                            + "  </FrozenRule>\n"
                        )
                    )
                    + "</FrozenRules>";
        }

        public static List<string> ReadViolations(StoreKind kind, string path, string description)
        {
            var raw = ReadRaw(kind, path, description);
            return kind == StoreKind.Json
                ? ((JObject)raw)["Violations"].Select(v => (string)v).ToList()
                : ((XElement)raw).Elements().Select(e => e.Value).ToList();
        }

        // The serialized section of a rule: a JObject for JSON stores, an XElement for XML.
        public static object ReadRaw(StoreKind kind, string path, string description)
        {
            return kind == StoreKind.Json
                ? (object)
                    JArray
                        .Parse(File.ReadAllText(path))
                        .Cast<JObject>()
                        .Single(rule => (string)rule["ArchRuleDescription"] == description)
                : XDocument
                    .Load(path)
                    .Root.Elements()
                    .Single(rule => rule.Attribute("ArchRule")?.Value == description);
        }

        private static string JsonEscape(string value) =>
            value.Replace("\\", "\\\\").Replace("\"", "\\\"");
    }
}
