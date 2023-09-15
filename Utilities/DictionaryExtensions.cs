using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace deeprockitems.Utilities
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Adds the provided key and value pair to the dictionary. Returns the instance of the dictionary.
        /// </summary>
        /// <typeparam name="tkey"></typeparam>
        /// <typeparam name="tvalue"></typeparam>
        /// <param name="thisDictionary"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when no argument was supplied</exception>
        /// <exception cref="ArgumentException">Thrown when args is empty</exception>
        public static Dictionary<tkey, tvalue> AddThis<tkey, tvalue>(this Dictionary<tkey, tvalue> thisDictionary, tkey Key, tvalue Value)
        {
            // You must feed the method with information :p
            if (thisDictionary == null)
            {
                throw new ArgumentNullException(nameof(thisDictionary));
            }
            if (Key == null)
            {
                throw new ArgumentNullException(nameof(Key));
            }
            if (Value == null)
            {
                throw new ArgumentNullException(nameof(Value));
            }

            thisDictionary.Add(Key, Value);
            return thisDictionary;
        }
        /// <summary>
        /// Adds the provided key and value pair to the dictionary. Returns the instance of the dictionary.
        /// </summary>
        /// <typeparam name="tkey"></typeparam>
        /// <typeparam name="tvalue"></typeparam>
        /// <param name="thisDictionary"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when no argument was supplied</exception>
        /// <exception cref="ArgumentException">Thrown when args is empty</exception>
        public static Dictionary<tkey, int> AddThis<tkey>(this Dictionary<tkey, int> thisDictionary, tkey Key, int Value = 1)
        {
            if (thisDictionary == null)
            {
                throw new ArgumentNullException(nameof(thisDictionary));
            }
            if (Key == null)
            {
                throw new ArgumentNullException(nameof(Key));
            }

            thisDictionary.Add(Key, Value);
            return thisDictionary;
        }
    }
}
