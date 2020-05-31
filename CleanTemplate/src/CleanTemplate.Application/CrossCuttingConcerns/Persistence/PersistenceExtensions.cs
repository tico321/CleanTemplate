using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CleanTemplate.Application.CrossCuttingConcerns.Persistence
{
    public static class PersistenceExtensions
    {
        public static async Task<Page<T>> WithPaging<T>(
            this IQueryable<T> items,
            PageQuery page,
            CancellationToken cancellationToken)
        {
            var totalItems = await items.CountAsync(cancellationToken);
            var skip = (page.Page - 1) * page.PageSize;
            var pagedResult = await items
                .Skip(skip)
                .Take(page.PageSize)
                .ToListAsync(cancellationToken);
            return new Page<T>(pagedResult, page.Page, page.PageSize, totalItems);
        }
    }
}
