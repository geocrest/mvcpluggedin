namespace Geocrest.Model
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Identifies a related resource.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class RelatedLink : Link
    {
        /// <summary>
        /// The relation this link has to the context IRI.
        /// </summary>
        public const string Relation = "related";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.RelatedLink"/> class.
        /// </summary>
        /// <param name="href">The href.</param>
        /// <param name="title">The title.</param>
        public RelatedLink(string href, string title = null)
            : base(Relation, href, title)
        {
        }
    }
}
