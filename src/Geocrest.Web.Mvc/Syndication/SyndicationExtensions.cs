namespace Geocrest.Web.Mvc.Syndication
{
    using System;
    using System.Linq;
    using System.ServiceModel.Syndication;
    using System.Xml;
    using Geocrest.Web.Infrastructure;

    /// <summary>
    /// Provides extension methods for syndicating AtomPub content.
    /// </summary>
    public static class SyndicationExtensions
    {
        /// <summary>
        /// Converts the specified feed into Atom syntax.
        /// </summary>
        /// <param name="feed">The feed.</param>
        /// <returns>A syndicated feed.</returns>
        public static SyndicationFeed Syndicate(this IPublicationFeed feed)
        {
            Throw.IfArgumentNull(feed, "feed");

            var atomFeed = new SyndicationFeed
            {
                Title = new TextSyndicationContent(feed.Title),
                Description = new TextSyndicationContent(feed.Summary ?? string.Empty)
            };

            atomFeed.Authors.Add(new SyndicationPerson { Name = feed.Author });

            feed.Links.ForEach(link =>
                atomFeed.Links.Add(new SyndicationLink(new Uri(link.Href)) { RelationshipType = link.Rel, Title = link.Title }));

            return atomFeed;
        }
        /// <summary>
        /// Converts the specified item into Atom syntax.
        /// </summary>
        /// <param name="publication">The publication.</param>
        /// <returns>A syndicated publication.</returns>
        public static SyndicationItem Syndicate(this IPublication publication)
        {
            Throw.IfArgumentNull(publication, "publication");

            var item = new SyndicationItem
            {
                Id = publication.Id,
                Title = new TextSyndicationContent(publication.Title, TextSyndicationContentKind.Plaintext),
                LastUpdatedTime = publication.PublishDate != null && publication.PublishDate.HasValue ? publication.PublishDate.Value :
                    publication.LastUpdated != null && publication.LastUpdated.HasValue ? publication.LastUpdated.Value :
                    new DateTime(), // use publish date if it exists (for posts)
                Summary = new TextSyndicationContent(publication.Summary, TextSyndicationContentKind.Plaintext),
                Content = GetSyndicationContent(publication)
            };

            if (publication.PublishDate.HasValue) // Optional according to Atom spec
            {
                item.PublishDate = publication.PublishDate.Value;
            }

            publication.Categories.ForEach(category =>
                item.Categories.Add(new SyndicationCategory(category.Name, publication.CategoriesScheme, category.Label)));

            publication.Links.ForEach(link =>
                item.Links.Add(new SyndicationLink(new Uri(link.Href,UriKind.RelativeOrAbsolute)) { RelationshipType = link.Rel, Title = link.Title }));

            return item;
        }
        /// <summary>
        /// Converts the specified media item into Atom syntax.
        /// </summary>
        /// <param name="mediaResource">The media resource.</param>
        /// <returns>A syndicated media resource.</returns>
        public static SyndicationItem Syndicate(this IMediaResource mediaResource)
        {
            Throw.IfArgumentNull(mediaResource, "mediaResource");
            var item = new SyndicationItem
            {
                Id = mediaResource.Id,
                Title = new TextSyndicationContent(mediaResource.Title, TextSyndicationContentKind.Plaintext),
                Summary = new TextSyndicationContent(mediaResource.Summary, TextSyndicationContentKind.Plaintext),
                Content = new UrlSyndicationContent(new Uri(mediaResource.MediaUrl), mediaResource.ContentType)
            };
            mediaResource.Links.ForEach(link =>
                item.Links.Add(new SyndicationLink(new Uri(link.Href)) { RelationshipType = link.Rel, Title = link.Title }));
            return item;
        }
        /// <summary>
        /// Converts the input syndication item into a model.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="command">The command.</param>
        /// <param name="item">The item.</param>
        public static void ReadSyndicationItem<TCommand>(this TCommand command, SyndicationItem item)
           where TCommand : IPublication
        {
            Throw.IfArgumentNull(command, "command");
            Throw.IfArgumentNull(item, "item");
            command.Title = item.Title.Text;
            command.Summary = item.Summary != null ? item.Summary.Text : null;
            command.Content = ((TextSyndicationContent)item.Content).Text;
            command.ContentType = item.Content.Type;
            command.Categories = item.Categories.Select(c => new PublicationCategory(c.Name));
            command.PublishDate = GetPublishDate(item.PublishDate);
            command.IsDraft = GetIsDraft(item);
        }
        /// <summary>
        /// Converts the input syndication item into a model.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="command">The command.</param>
        /// <param name="item">The item.</param>
        public static void ReadMediaItem<TCommand>(this TCommand command, SyndicationItem item)
            where TCommand : IMediaResourceCommand
        {
            Throw.IfArgumentNull(command, "command");
            Throw.IfArgumentNull(item, "item");
            command.Title = item.Title.Text;
            command.Summary = item.Summary != null ? item.Summary.Text : null;          
        }
        /// <summary>
        /// Gets the publish date for a syndicated item.
        /// </summary>
        /// <param name="syndicationDate">The syndication date.</param>
        /// <returns>
        /// Returns the date the item was published.
        /// </returns>
        public static DateTime GetPublishDate(DateTimeOffset syndicationDate)
        {
            var publishDate = syndicationDate.UtcDateTime;
            // if the publish date has not been set it will be equal to DateTime.MinValue
            return publishDate == DateTime.MinValue ? DateTime.UtcNow : publishDate;
        }

        private static bool GetIsDraft(SyndicationItem item)
        {
            foreach (var element in item.ElementExtensions)
            {
                if (element.OuterName.ToLower() == "control")
                {
                    var control = element.GetObject<XmlElement>();
                    if (control.HasChildNodes)
                    {
                        foreach (XmlNode node in control.ChildNodes)
                        {
                            if (node.NodeType == XmlNodeType.Element && node.LocalName.ToLower() == "draft")
                            {
                                return node.InnerText == "yes" ? true : false;
                            }
                        }
                    }
                }
            }
            return false;
        }
        private static SyndicationContent GetSyndicationContent(IPublication publication)
        {
            if (string.IsNullOrEmpty(publication.Content))
                return SyndicationContent.CreatePlaintextContent(string.Empty);
            switch(publication.ContentType)
            {
                case PublicationContentTypes.Text:
                    return SyndicationContent.CreatePlaintextContent(publication.Content);
                case PublicationContentTypes.HTML:
                    return SyndicationContent.CreateHtmlContent(publication.Content);
                default:
                    Throw.NotSupported(string.Format("{0} is not a supported content type.",publication.ContentType));
                    return null;
            }
        }
    }
}
