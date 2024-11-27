using System.Web;

namespace KnowYourToolset.BackEnd.Api.ApiModels;

public sealed class PaginatedList<T>
{
    public IReadOnlyCollection<T> Items { get; init; } = [];

    public int PageNumber { get; init; }

    public int PageSize { get; init; }

    public int TotalPages { get; init; }

    public int TotalCount { get; init; }

    public Uri? First { get; set; }

    public Uri? Last { get; set; }

    public Uri? Prev { get; set; }

    public Uri? Next { get; set; }

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;

    private PaginatedList(IReadOnlyCollection<T> items, int totalCount, int pageNumber, int pageSize, string requestUri)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        TotalCount = totalCount;
        Items = items;
        First = GetPageUri(requestUri, 1, pageSize);
        Last = GetPageUri(requestUri, TotalPages, pageSize);
        Next = pageNumber < TotalPages ? GetPageUri(requestUri, pageNumber + 1, pageSize) : null;
        Prev = pageNumber > 1 && pageNumber <= TotalPages ? GetPageUri(requestUri, pageNumber - 1, pageSize) : null;
    }


    private static Uri GetPageUri(string requestUri, int pageNumber, int pageSize)
    {
        UriBuilder uriBuilder = new(requestUri);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["pageNumber"] = pageNumber.ToString();
        query["pageSize"] = pageSize.ToString();
        uriBuilder.Query = query.ToString();

        return new Uri(uriBuilder.ToString());
    }

    public static PaginatedList<T> CreateAsync(IReadOnlyCollection<T> items, int totalCount, int pageNumber, int pageSize, string requestUri)
    {
        return new(items, totalCount, pageNumber, pageSize, requestUri);
    }
}

