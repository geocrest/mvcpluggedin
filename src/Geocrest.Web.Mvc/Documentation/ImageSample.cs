namespace Geocrest.Web.Mvc.Documentation
{
    using Geocrest.Web.Infrastructure;

    /// <summary>
    /// This represents an image sample on the help page. There's a display template named ImageSample associated with this class.
    /// </summary>
    public class ImageSample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Documentation.ImageSample"/> class.
        /// </summary>
        /// <param name="src">The URL of an image.</param>
        /// <exception cref="T:System.ArgumentNullException">src</exception>
        public ImageSample(string src)
        {
            Throw.IfArgumentNullOrEmpty(src, "src");
            Src = src;
        }

        /// <summary>
        /// Gets the image source.
        /// </summary>
        /// <value>
        /// The image source.
        /// </value>
        public string Src { get; private set; }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object" /> to compare with this instance.</param>
        /// <returns>
        /// <b>true</b>, if the specified <see cref="T:System.Object" /> is equal to this instance; otherwise, <b>false</b>.
        /// </returns>
        public override bool Equals(object obj)
        {
            ImageSample other = obj as ImageSample;
            return other != null && Src == other.Src;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Src.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="T:System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Src;
        }
    }
}