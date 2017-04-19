using System;
using System.Linq;

namespace BachMaiCR.Utilities
{
  public static class PageQuery
  {
    public static PagedList<T> Paginate<T>(this IQueryable<T> query, int pageIndex, int pageSize, int totalItemCount = 0)
    {
        if (totalItemCount == 0)
            totalItemCount = query.Count();
        return query.Skip(Math.Max(pageSize * pageIndex, 0))
            .Take(pageSize)
            .ToPagedList(totalItemCount, pageIndex, pageSize);
    }
  }
}
