using System;
using System.Collections.Generic;

namespace BachMaiCR.Utilities
{
    public class PagedList<T> : List<T>, IPagedList
    {
        public int PageCount { get; protected set; }

        public int TotalItemCount { get; protected set; }

        public int PageIndex { get; protected set; }

        public int PageSize { get; protected set; }

        public bool IsFirstPage
        {
            get
            {
                return this.PageIndex < 1;
            }
        }

        public bool IsLastPage
        {
            get
            {
                return this.PageIndex + 1 == this.PageCount;
            }
        }

        public PagedList(IEnumerable<T> list, int totalItemCount, int pageIndex, int pageSize)
          : base(list ?? new List<T>())
        {
            if (pageIndex < 0 || pageSize < 1)
                throw new ArgumentOutOfRangeException("PageIndex >= 0, PageSize >=1");
            this.PageCount = this.GetNumberOfPages(totalItemCount, pageSize);
            this.TotalItemCount = totalItemCount;
            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
        }

        public PagedList()
          : this(new List<T>(), 0, 0, 10)
        {
        }

        public int GetNumberOfPages(int totalItemCount, int pageSize)
        {
            return totalItemCount % pageSize == 0 ? totalItemCount / pageSize : totalItemCount / pageSize + 1;
        }
    }
}