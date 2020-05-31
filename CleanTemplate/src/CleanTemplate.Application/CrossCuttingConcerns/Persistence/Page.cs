using System;
using System.Collections.Generic;

namespace CleanTemplate.Application.CrossCuttingConcerns.Persistence
{
    public class Page<T>
    {
        public Page(IEnumerable<T> data, int currentPage, int pageSize, int totalItems)
        {
            Data = data;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling((decimal)totalItems / pageSize);
            StartPage = 1;
            EndPage = TotalPages;
            TotalItems = totalItems;
        }

        public IEnumerable<T> Data { get; }
        public int CurrentPage { get; }
        public int PageSize { get; }
        public int TotalPages { get; }
        public int TotalItems { get; }
        public int StartPage { get; }
        public int EndPage { get; }
    }
}
