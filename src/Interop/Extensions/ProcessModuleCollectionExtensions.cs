using System.Diagnostics;

namespace Paladin.Interop.Extensions;

internal static class ProcessModuleCollectionExtensions
{
    internal static bool Any(this ProcessModuleCollection collection, Func<ProcessModule, bool> predicate)
    {
        for (var i = 0; i < collection.Count; i++)
        {
            if (predicate(collection[i]))
            {
                return true;
            }
        }

        return false;
    }
}
