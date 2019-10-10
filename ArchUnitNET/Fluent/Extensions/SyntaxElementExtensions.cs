namespace ArchUnitNET.Fluent.Extensions
{
    public static class SyntaxElementExtensions
    {
        public static string GetShortDescription(this IHasDescription obj, int maxLength = 50)
        {
            return obj.Description.Length > maxLength
                ? obj.Description.Substring(0, maxLength - 3) + "..."
                : obj.Description;
        }
    }
}