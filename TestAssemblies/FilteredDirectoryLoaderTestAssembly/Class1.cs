using Serilog;

namespace FilteredDirectoryLoaderTestAssembly;

public class Class1
{
    public ILogger logger;

    public Class1()
    {
        this.logger = new LoggerConfiguration().CreateLogger();
    }
}
