using System.Collections.Generic;

namespace BachMaiCR.Utilities
{
    public static class PagedListExtensions
    {
        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> list, int totalItemCount, int pageIndex, int pageSize)
        {
            return new PagedList<T>(list, totalItemCount, pageIndex, pageSize);
        }
    }
}