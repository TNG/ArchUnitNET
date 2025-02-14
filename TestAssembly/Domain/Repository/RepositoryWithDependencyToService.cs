using TestAssembly.Domain.Marker;
using TestAssembly.Domain.Services;

// ReSharper disable UnusedMember.Global

namespace TestAssembly.Domain.Repository
{
#pragma warning disable 169
    public class RepositoryWithDependencyToService : IRepository
    {
        private TestService _badDependency;
    }
}
