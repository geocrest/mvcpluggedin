namespace Geocrest.Web.Mvc.Syndication
{
    using System.Linq;
    using System.Collections.Generic;
    using Geocrest.Web.Infrastructure;
    /// <summary>
    /// Provides the base class for all ATOM resources.
    /// </summary>
    public abstract class AtomResource 
    {
        private ICollection<Link> links;
        /// <summary>
        /// Gets or sets the unique id for this resource.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public int Id { get; set; }
        /// <summary>
        /// Gets the link collection for this resource.
        /// </summary>
        /// <value>
        /// The links.
        /// </value>
        public virtual ICollection<Link> Links
        {
            get { return links; }
            set { this.links = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Syndication.AtomResource" /> class.
        /// </summary>
        public AtomResource()
        {
            links = new List<Link>();
        }

        /// <summary>
        /// Adds a new link to this resource's collection.
        /// </summary>
        /// <param name="link">The link.</param>
        public void AddLink(Link link)
        {
            Throw.IfArgumentNull(link, "link");
            if (this.links == null) this.links = new List<Link>();
            if (link is SelfLink)
            {
                if (this.links.Count(x => x.Rel == link.Rel) > 0)
                    this.links.Remove(this.links.First(x => x.Rel == link.Rel));
            }
            if (this.links.Count(x => x.Rel == link.Rel && x.Href == link.Href) == 0)
                this.links.Add(link);
        }
    }
}
