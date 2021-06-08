using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;

namespace ArchUnitNETTests.Domain.PlantUml
{
    public class TestDiagram
    {
        private string _path;
        private Stream _stream;
        private IList<string> _lines = new List<string>();

        public TestDiagram(string path)
        {
            _path = path;
        }

        public TestDiagram(Stream stream)
        {
            _stream = stream;
        }

        public static TestDiagram In(string path)
        {
            return new TestDiagram(path);
        }

        internal static TestDiagram From(Stream stream)
        {
            return new TestDiagram(stream);
        }

        public ComponentBuilder Component(string componentName)
        {
            return new ComponentBuilder(componentName, this);
        }

        public DependencyFromCreator DependencyFrom(string origin)
        {
            return new DependencyFromCreator(origin, this);
        }

        public DependencyToCreator DependencyTo(string target)
        {
            return new DependencyToCreator(target, this);
        }

        private TestDiagram AddComponent(ComponentBuilder creator)
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
            if(_path == null)
            {
                throw new InvalidOperationException("Path must be provided to call this method. Try with Stream()");
            }
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

        internal Stream Stream()
        {
            if (_stream == null)
            {
                throw new InvalidOperationException("Stream must be provided to call this method. Try with Write()");
            }
            using (StreamWriter file = new StreamWriter(_stream, leaveOpen:true))
            {
                file.WriteLine("@startuml");
                foreach (var line in _lines)
                {
                    file.WriteLine(line);
                }
                file.WriteLine("@enduml");
                file.Flush();
                _stream.Seek(0, SeekOrigin.Begin);
                return _stream;
            }
        }

        private StreamWriter CreateTempFile(string path)
        {
            return File.CreateText(path);
        }

        public class ComponentBuilder
        {
            private readonly TestDiagram _testDiagram;

            public ComponentBuilder(string componentName, TestDiagram testDiagram)
            {
                ComponentName = componentName;
                _testDiagram = testDiagram;
            }

            public string ComponentName { get; }
            public string Alias { get; private set; }

            public List<string> Stereotypes { get; } = new List<string>();

            public ComponentBuilder WithAlias(string alias)
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

        public class DependencyFromCreator
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

        public class DependencyToCreator
        {
            private readonly string _target;
            private readonly TestDiagram _testDiagram;

            public DependencyToCreator(string target, TestDiagram testDiagram)
            {
                _target = target;
                _testDiagram = testDiagram;
            }

            public TestDiagram From(string origin)
            {
                string dependency = _target + " <-- " + origin;
                return _testDiagram.RawLine(dependency);
            }
        }

        
    }
}
