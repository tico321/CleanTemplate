namespace CleanTemplate.Application.CrossCuttingConcerns.Persistence
{
    public class PageQuery
    {
        public PageQuery(int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;
        }

        public int Page { get; }
        public int PageSize { get; }
    }
}
