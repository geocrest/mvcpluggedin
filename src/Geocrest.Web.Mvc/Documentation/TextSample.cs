namespace Geocrest.Web.Mvc.Documentation
{
    using System.Collections.ObjectModel;
    using System.Net.Http.Headers;
    using Geocrest.Web.Infrastructure;

    /// <summary>
    /// This represents a preformatted text sample on the help page. There's a display template named TextSample associated with this class.
    /// </summary>
    public class TextSample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Documentation.TextSample" /> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="mediaTypes">The media types for this sample.</param>
        /// <exception cref="T:System.ArgumentNullException">text</exception>
        /// <exception cref="T:System.ArgumentNullException">mediaTypes</exception>
        public TextSample(string text, Collection<MediaTypeHeaderValue> mediaTypes)
        {
            Throw.IfArgumentNullOrEmpty(text, "text");
            Throw.IfArgumentNull(mediaTypes, "mediaTypes");
            Text = text;
            MediaTypes = mediaTypes;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Documentation.TextSample" /> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <exception cref="T:System.ArgumentNullException">text</exception>
        public TextSample(string text)
        {
            Throw.IfArgumentNullOrEmpty(text, "text");
            Text = text;
        }
        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text { get; private set; }
        /// <summary>
        /// Gets the type of media.
        /// </summary>
        /// <value>
        /// The type of media.
        /// </value>
        public Collection<MediaTypeHeaderValue>MediaTypes{get;private set;}
        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object" /> to compare with this instance.</param>
        /// <returns>
        /// <b>true</b>, if the specified <see cref="T:System.Object" /> is equal to this instance; otherwise, <b>false</b>.
        /// </returns>
        public override bool Equals(object obj)
        {
            TextSample other = obj as TextSample;
            return other != null && Text == other.Text;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Text.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="T:System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Text;
        }
    }
}