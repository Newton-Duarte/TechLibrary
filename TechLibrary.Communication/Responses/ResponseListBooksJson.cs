namespace TechLibrary.Communication.Responses;

public class ResponseListBooksJson
{
    public ResponsePaginationJson Pagination { get; set; } = default!;
    public List<ResponseBookJson> Books { get; set; } = [];
}
