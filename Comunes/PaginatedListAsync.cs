using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCRUDwithORACLE.Comunes
{
    public class PaginatedListAsync<T> : List<T>
    {

        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedListAsync(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        public static async Task<PaginatedListAsync<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            //var count = await source.CountAsync();
            var count = source.Count();
            List<T> items = null;
            //    return Task.FromResult(source.ToList());
            //return source.ToListAsync();
            if (!(source is IAsyncEnumerable<T>))
                items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            else
                items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedListAsync<T>(items, count, pageIndex, pageSize);
        }

    }
}
