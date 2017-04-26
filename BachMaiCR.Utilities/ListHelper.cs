using System.Collections.Generic;
using System.Linq;

namespace BachMaiCR.Utilities
{
    public static class ListHelper
    {
        public static bool ContainsAllItems<T>(List<T> a, List<T> b)
        {
            return !b.Except<T>(a).Any<T>();
        }
    }
}