﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;

namespace ArchUnitNETTests.Domain.PlantUml
{
    internal class TestDiagram
    {
        private string _path;
        private IList<string> _lines = new List<string>();

        public TestDiagram(string path)
        {
            _path = path;
        }

        public static TestDiagram In(string path)
        {
            return new TestDiagram(path);
        }

        public ComponentCreator Component(string componentName)
        {
            return new ComponentCreator(componentName, this);
        }

        public DependencyFromCreator DependencyFrom(string origin)
        {
            return new DependencyFromCreator(origin, this);
        }

        private TestDiagram AddComponent(ComponentCreator creator)
        {
            string stereotypes = string.Join(" ", creator.Stereotypes.Select(input => "<<" + input + ">>"));
            string line = string.Format("[{0}] {1}", creator.ComponentName, stereotypes);
            if (creator.Alias != null)
            {
                line += " as " + creator.Alias;
            }
            _lines.Add(line);
            return this;
        }

        public TestDiagram RawLine(string line)
        {
            _lines.Add(line);
            return this;
        }

        internal string Write()
        {
            string path = Path.Combine(_path, "plantuml_diagram_" + Guid.NewGuid() + ".puml");
            using (StreamWriter file = CreateTempFile(path))
            {
                file.WriteLine("@startuml");
                foreach (var line in _lines)
                {
                    file.WriteLine(line);
                }
                file.WriteLine("@enduml");
                return path;
            }
        }

        private StreamWriter CreateTempFile(string path)
        {
            return File.CreateText(path);
        }

        internal class ComponentCreator
        {
            private readonly TestDiagram _testDiagram;

            public ComponentCreator(string componentName, TestDiagram testDiagram)
            {
                ComponentName = componentName;
                _testDiagram = testDiagram;
            }

            public string ComponentName { get; }
            public string Alias { get; private set; }

            public List<string> Stereotypes { get; } = new List<string>();

            public ComponentCreator WithAlias(string alias)
            {
                Alias = alias;
                return this;
            }

            public TestDiagram WithStereoTypes(params string[] stereoTypes)
            {
                Stereotypes.AddRange(ImmutableList.CreateRange(stereoTypes));
                return _testDiagram.AddComponent(this);
            }


        }

        internal class DependencyFromCreator
        {
            private readonly string _origin;
            private readonly TestDiagram _testDiagram;

            public DependencyFromCreator(string origin, TestDiagram testDiagram)
            {
                _origin = origin;
                _testDiagram = testDiagram;
            }

            public TestDiagram To(string target)
            {
                string dependency = _origin + " --> " + target;
                return _testDiagram.RawLine(dependency);
            }
        }
    }
}