namespace FilteredDirectoryUnavailableTypesAssembly;

[AttributeUsage(AttributeTargets.Assembly)]
public class AssemblyAttribute : Attribute { }

[AttributeUsage(AttributeTargets.All)]
public class BaseAttribute : Attribute { }
