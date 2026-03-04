

namespace PruebaTecnica.Application.Extensions
{
    public static class IQuerableExtensions
    {
        public static IQueryable<T> Page<T>(this IQueryable<T> query, int pageIndex, int PageSize)
        {
            return query.Skip((pageIndex - 1) * PageSize)
                        .Take(PageSize);
        }
    }
}
