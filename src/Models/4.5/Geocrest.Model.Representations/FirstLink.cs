namespace Geocrest.Model
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The target IRI that refers to the furthest preceding resource in a series of resources.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class FirstLink : Link
    {
        /// <summary>
        /// The relation this link has to the context IRI.
        /// </summary>
        public const string Relation = "first";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.FirstLink"/> class.
        /// </summary>
        /// <param name="href">The href.</param>
        /// <param name="title">The title.</param>
        public FirstLink(string href, string title = null)
            : base(Relation, href, title)
        {
        }
    }
}
