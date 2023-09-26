using Terraria;
using Microsoft.Xna.Framework;

namespace deeprockitems.Utilities
{
    public static class ArrayExtensions
    {
        public static bool Contains<T>(this T[] array, T value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Equals(value)) return true;
            }
            return false;
        }
        public static int FindFirstIndexOf<T>(this T[] array, T item)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Equals(item)) return i;
            }
            return -1;
        }
        public static T[] AddAndReplace<T>(this T[] array, T item, T replace = default)
        {
            if (!array.Contains(replace))
            {
                throw new System.ArgumentOutOfRangeException(nameof(array), $"{nameof(array)} did not contain {nameof(item)}.");
            }
            else
            {
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i].Equals(replace))
                    {
                        array[i] = item;
                        return array;
                    }
                }
            }
            return array;
        }
        public static T[] PopAndShuffle<T>(this T[] array, T item)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Equals(default(T)))
                {
                    array[i] = item;
                    break;
                }
            }
            int index = array.FindFirstIndexOf(default(T));
            if (index == -1) return array;
            
            for (int i = index; i < array.Length - 1; i++)
            {
                if (array[i].Equals(default(T)))
                {
                    (array[i], array[i + 1]) = (array[i + 1], array[i]);
                }
            }
            return array;
        }
    }
}
