namespace SlicesTestAssembly.DotDotSemantics.AlphaService;

// A single segment that ends with the literal "Service", unlike Alpha/Service below, which really
// is two segments. "DotDotSemantics.(*)..Service" tells the two apart only if ".." matches whole
// segments.
public class AlphaServiceClass { }
