namespace Geocrest.Model
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Conveys an identifier for the link's context.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class SelfLink : Link
    {
        /// <summary>
        /// The relation this link has to the context IRI.
        /// </summary>
        public const string Relation = "self";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.SelfLink"/> class.
        /// </summary>
        /// <param name="href">The href.</param>
        /// <param name="title">The title.</param>
        public SelfLink(string href, string title = null)
            : base(Relation, href, title)
        {
        }
    }
}
