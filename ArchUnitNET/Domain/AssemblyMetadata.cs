using System;
using System.IO;
using System.Security.Cryptography;

namespace ArchUnitNET.Domain
{
    /// <summary>
    /// Tracks metadata for an assembly file to detect changes
    /// </summary>
    public sealed class AssemblyMetadata : IEquatable<AssemblyMetadata>
    {
        public string FilePath { get; }
        public string FileHash { get; }
        public DateTime LastWriteTimeUtc { get; }
        public long FileSize { get; }

        public AssemblyMetadata(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("File path cannot be null or empty", nameof(filePath));
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Assembly file not found: {filePath}", filePath);
            }

            FilePath = Path.GetFullPath(filePath);
            var fileInfo = new FileInfo(FilePath);
            LastWriteTimeUtc = fileInfo.LastWriteTimeUtc;
            FileSize = fileInfo.Length;
            FileHash = ComputeFileHash(FilePath);
        }

        private static string ComputeFileHash(string filePath)
        {
            using (var sha256 = SHA256.Create())
            using (var stream = File.OpenRead(filePath))
            {
                var hash = sha256.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }

        public bool Equals(AssemblyMetadata other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return string.Equals(FilePath, other.FilePath, StringComparison.OrdinalIgnoreCase)
                && FileHash == other.FileHash
                && LastWriteTimeUtc.Equals(other.LastWriteTimeUtc)
                && FileSize == other.FileSize;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((AssemblyMetadata)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = StringComparer.OrdinalIgnoreCase.GetHashCode(FilePath ?? string.Empty);
                hashCode = (hashCode * 397) ^ (FileHash?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ LastWriteTimeUtc.GetHashCode();
                hashCode = (hashCode * 397) ^ FileSize.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"AssemblyMetadata(Path={FilePath}, Hash={FileHash?.Substring(0, 8)}..., Size={FileSize}, Modified={LastWriteTimeUtc})";
        }
    }
}
