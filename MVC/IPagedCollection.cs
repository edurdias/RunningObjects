namespace RunningObjects.MVC
{
    public interface IPagedCollection
    {
        int PageSize { get; set; }
        int PageNumber { get; set; }
        int PageCount { get; set; }
    }
}