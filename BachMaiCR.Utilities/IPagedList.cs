namespace BachMaiCR.Utilities
{
  public interface IPagedList
  {
    int PageCount { get; }

    int TotalItemCount { get; }

    int PageIndex { get; }

    int PageSize { get; }
  }
}
