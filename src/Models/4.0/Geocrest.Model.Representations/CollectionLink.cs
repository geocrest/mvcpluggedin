namespace Geocrest.Model
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The target IRI points to a resource which represents the collection resource for the context IRI.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class CollectionLink : Link
    {
        /// <summary>
        /// The relation this link has to the context IRI.
        /// </summary>
        public const string Relation = "collection";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.CollectionLink"/> class.
        /// </summary>
        /// <param name="href">The href.</param>
        /// <param name="title">The title.</param>
        public CollectionLink(string href, string title = null)
            : base(Relation, href, title)
        {
        }
    }
}
