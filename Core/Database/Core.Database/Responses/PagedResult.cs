using Core.Database.Interface.Responses;

namespace Core.Database.Responses;

public class PagedResult<T> : IPagedResult<T>
{
    public IEnumerable<T>? Results { get; set; }
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalCount { get; set; }
}