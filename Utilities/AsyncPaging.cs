using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BabyStore.Utilities
{
    public static class AsyncPaging
    {
        public static async Task<List<T>> ReturnPages<T>(this IQueryable<T> inputCollection, int pageNumber, int pageSize)   //Adding the 'this' Keyword now allows this method to act as an extension method to the type IQueryable<T>
        {
            return await inputCollection.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }
    }
}
