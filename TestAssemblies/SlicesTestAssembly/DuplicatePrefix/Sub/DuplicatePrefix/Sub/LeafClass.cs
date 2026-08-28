namespace SlicesTestAssembly.DuplicatePrefix.Sub.DuplicatePrefix.Sub;

// The full slice prefix "DuplicatePrefix.Sub" occurs twice in this namespace: matching it with a
// leading ".." (Contains-based) pattern is ambiguous about which occurrence is "the" prefix.
public class LeafClass { }
