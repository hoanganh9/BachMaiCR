using System;
using System.Linq;

namespace BachMaiCR.Utilities
{
  public static class PageQuery
  {
    public static PagedList<T> Paginate<T>(this IQueryable<T> query, int pageIndex, int pageSize, int totalItemCount = 0)
    {
      if (totalItemCount == 0)
        totalItemCount = Queryable.Count<T>(query);
      return Queryable.Take<T>(Queryable.Skip<T>(query, Math.Max(pageSize * pageIndex, 0)), pageSize).ToPagedList<T>(totalItemCount, pageIndex, pageSize);
    }
  }
}
