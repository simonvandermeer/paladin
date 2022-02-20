using System;
using System.Diagnostics;

namespace Paladin.Interop.Extensions
{
    public static class ProcessModuleCollectionExtensions
    {
        public static bool Any(this ProcessModuleCollection collection, Func<ProcessModule, bool> predicate)
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
}
