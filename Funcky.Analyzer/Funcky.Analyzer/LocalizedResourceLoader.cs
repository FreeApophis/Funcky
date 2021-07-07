using Microsoft.CodeAnalysis;

namespace Funcky.Analyzer
{
    internal static class LocalizedResourceLoader
    {
        internal static LocalizableString LoadFromResource(string resourceName)
            => new LocalizableResourceString(resourceName, Resources.ResourceManager, typeof(Resources));
    }
}
