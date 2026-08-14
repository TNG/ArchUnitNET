namespace SlicesTestAssembly.DotDotSemantics.AlphaService;

// A single segment that ends with the literal "Service": a pattern such as
// "DotDotSemantics.(*)..Service" must not match it by splitting the segment into
// "Alpha" + "Service".
public class AlphaServiceClass { }
