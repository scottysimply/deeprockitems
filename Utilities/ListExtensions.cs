using System;
using System.Collections.Generic;

namespace deeprockitems.Utilities
{
    public static class ListExtension
    {
        public static List<T> AddThis<T>(this List<T> list, T item)
        {
            list.Add(item);
            return list;
        }
    }
}
