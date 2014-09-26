namespace Geocrest.Web.Mvc.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    /// <summary>
    /// Provides a list for use in paging applications.
    /// </summary>
    /// <typeparam name="T">The type of model contained within this list.</typeparam>
    public class PagedList<T> : List<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Models.PagedList`1" /> class.
        /// </summary>
        /// <param name="source">The collection source.</param>
        /// <param name="pageIndex">Index of the current page.</param>
        /// <param name="pageSize">Number of items per page.</param>
        public PagedList(IQueryable<T> source, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = source.Count();
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

            this.AddRange(source.Skip(PageIndex * PageSize).Take(PageSize));
        }
        /// <summary>
        /// Gets the index of the current page.
        /// </summary>
        /// <value>
        /// The current page index.
        /// </value>
        public int PageIndex { get; private set; }
        /// <summary>
        /// Gets the total number of items in each page.
        /// </summary>
        /// <value>
        /// The size of the page.
        /// </value>
        public int PageSize { get; private set; }
        /// <summary>
        /// Gets the total count of items in the database.
        /// </summary>
        /// <value>
        /// The total count.
        /// </value>
        public int TotalCount { get; private set; }
        /// <summary>
        /// Gets the total number of pages for the given page size.
        /// </summary>
        /// <value>
        /// The total pages.
        /// </value>
        public int TotalPages { get; private set; }
        /// <summary>
        /// Gets a value indicating whether the list is on a page that has previous pages.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has a previous page; otherwise, <c>false</c>.
        /// </value>
        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 0);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the list is on a page that has more pages after it.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has a next page; otherwise, <c>false</c>.
        /// </value>
        public bool HasNextPage
        {
            get
            {
                return (PageIndex + 1 < TotalPages);
            }
        }
    }
}