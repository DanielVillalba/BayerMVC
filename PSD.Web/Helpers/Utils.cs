using System.Collections.Generic;
using System.Linq;

namespace PSD.Web.Helpers
{
    public static class Utils
    {
        /// <summary>
        /// Returns the next element of a given element from a IEnumerable 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        public static T GetNext<T>(IEnumerable<T> list, T current)
        {
            try
            {
                T nextItem = default(T);
                var arrayOfItems = list.ToArray();
                for (int i = 0; i < arrayOfItems.Length; i++)
                {
                    if (arrayOfItems[i].Equals(current))
                    {
                        nextItem = i == (arrayOfItems.Length - 1) ? arrayOfItems.FirstOrDefault() : arrayOfItems[i + 1];
                        return nextItem;
                    }
                }
                return nextItem;
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// Returns the previous element of a given element from a IEnumerable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        public static T GetPrevious<T>(IEnumerable<T> list, T current)
        {
            try
            {
                T previousItem = default(T);
                var arrayOfItems = list.ToArray();
                for(int i = 0; i < arrayOfItems.Length; i++)
                {
                    if (arrayOfItems[i].Equals(current))
                    {
                        previousItem = i == 0 ? arrayOfItems.LastOrDefault() : arrayOfItems[i - 1];
                        return previousItem;
                    }
                }
                return previousItem;
            }
            catch
            {
                return default(T);
            }
        }
    }
}