using System;
using System.IO;

namespace ArchUnitNETTests.Fluent
{
    internal sealed class TempStoreDirectory : IDisposable
    {
        private readonly string _directory = Path.Combine(
            Path.GetTempPath(),
            "archunitnet-freeze-" + Guid.NewGuid()
        );

        public string PathFor(string fileName) => Path.Combine(_directory, fileName);

        public string CreateFile(string fileName, string content)
        {
            Directory.CreateDirectory(_directory);
            var path = PathFor(fileName);
            File.WriteAllText(path, content);
            return path;
        }

        public void Dispose()
        {
            if (Directory.Exists(_directory))
            {
                Directory.Delete(_directory, true);
            }
        }
    }
}
