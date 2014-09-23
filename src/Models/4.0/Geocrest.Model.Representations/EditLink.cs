namespace Geocrest.Model
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The target IRI that can be used to edit the link's context.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class EditLink : Link
    {
        /// <summary>
        /// The relation this link has to the context IRI.
        /// </summary>
        public const string Relation = "edit";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.EditLink"/> class.
        /// </summary>
        /// <param name="href">The href.</param>
        /// <param name="title">The title.</param>
        public EditLink(string href, string title = null)
            : base(Relation, href, title)
        {
        }
    }
}
