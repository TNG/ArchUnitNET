namespace ArchUnitNET.Loader
{
    public class ArchLoaderException : System.Exception
    {
        public ArchLoaderException(string message)
            : base(message) { }

        public ArchLoaderException(string message, System.Exception innerException)
            : base(message, innerException) { }
    }
}
