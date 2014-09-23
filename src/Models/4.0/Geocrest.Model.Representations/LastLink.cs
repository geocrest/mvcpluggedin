namespace Geocrest.Model
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The target IRI that refers to the furthest following resource in a series of resources.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class LastLink : Link
    {
        /// <summary>
        /// The relation this link has to the context IRI.
        /// </summary>
        public const string Relation = "last";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.LastLink"/> class.
        /// </summary>
        /// <param name="href">The href.</param>
        /// <param name="title">The title.</param>
        public LastLink(string href, string title = null)
            : base(Relation, href, title)
        {
        }
    }
}
