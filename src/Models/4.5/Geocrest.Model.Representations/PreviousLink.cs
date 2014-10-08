namespace Geocrest.Model
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The target IRI indicating that the link's context is a part of a series, and that the previous in the series is the link target.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class PreviousLink : Link
    {
        /// <summary>
        /// The relation this link has to the context IRI.
        /// </summary>
        public const string Relation = "prev";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.PreviousLink"/> class.
        /// </summary>
        /// <param name="href">The href.</param>
        /// <param name="title">The title.</param>
        public PreviousLink(string href, string title = null)
            : base(Relation, href, title)
        {
        }
    }
}
