using FilteredDirectoryUnavailableTypesAssembly;
using Serilog;

[assembly: Assembly]

namespace FilteredDirectoryLoaderTestAssembly;

public class Class1
{
    public ILogger logger;

    public Class1()
    {
        this.logger = new LoggerConfiguration().CreateLogger();
    }
}

public class DerivedAttribute : BaseAttribute { }

[Derived]
public class ClassWithDerivedAttribute { }
