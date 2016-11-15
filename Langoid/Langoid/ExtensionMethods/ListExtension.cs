using System.Collections.Generic;

namespace Langoid.ExtensionMethods
{
    public static class ListExtension
    {
        public static T NextOf<T>(this IList<T> list, T item)
        {
            var itemIndex = list.IndexOf(item);
            return list[itemIndex + 1 == list.Count ? 0 : itemIndex + 1];
        }
    }
}