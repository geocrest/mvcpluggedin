﻿namespace Geocrest.Web.Mvc.Syndication
{
    using System.Collections.Generic;

    /// <summary>
    /// An interface for publication feeds.
    /// </summary>
    public interface IPublicationFeed
    {
        /// <summary>
        /// Required. The title of the publication feed.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// A summary of the publication feed.
        /// </summary>
        string Summary { get; }

        /// <summary>
        /// Required. The author of the publications in this feed.
        /// </summary>
        string Author { get; }

        /// <summary>
        /// The items in the feed.
        /// </summary>
        IEnumerable<IPublication> Items { get; }

        /// <summary>
        /// A collection of related resource links. A feed should contain a link back to itself.
        /// </summary>
        ICollection<Link> Links { get; }
    }
}
