namespace SlicesTestAssembly.DotDotSemantics.AlphaService;

// A single segment that ends with the literal "Service". Parse() discards everything after the
// first "(*" for single-asterisk patterns, so "DotDotSemantics.(*)..Service" never actually
// applies the "..Service" postfix: this namespace matches it just the same as Alpha/Service below,
// even though "AlphaService" is one segment and not "Alpha" + "Service".
public class AlphaServiceClass { }
