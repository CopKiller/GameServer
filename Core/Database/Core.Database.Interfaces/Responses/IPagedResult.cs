namespace Core.Database.Interfaces.Responses;

public interface IPagedResult<T>
{
    IEnumerable<T>? Results { get; set; }
    int CurrentPage { get; set; }
    int PageSize { get; set; }
    int TotalCount { get; set; }
}