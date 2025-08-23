using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax
{
    public static class DescriptionHelpers
    {
        public static string SelectDescription<T>(
            string emptyDescription,
            string singleDescription,
            string multipleDescription,
            IObjectProvider<T> objectProvider
        )
        {
            if (!(objectProvider is ISizedObjectProvider<T> sizedObjectProvider))
            {
                return $"{multipleDescription} {objectProvider.Description}";
            }

            switch (sizedObjectProvider.Count)
            {
                case 0:
                    return emptyDescription;
                case 1:
                    return $"{singleDescription} {objectProvider.Description}";
            }
            return $"{multipleDescription} {objectProvider.Description}";
        }
    }
}
