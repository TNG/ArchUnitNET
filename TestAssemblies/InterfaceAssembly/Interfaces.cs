namespace InterfaceAssembly;

public interface IBaseInterface;

public interface IChildInterface : IBaseInterface;

public interface IOtherBaseInterface;

public interface IOtherChildInterface : IOtherBaseInterface;

public interface IInterfaceWithMultipleDependencies : IBaseInterface, IOtherBaseInterface;

public interface IInterfaceWithoutDependencies;
