namespace Geocrest.Web.Mvc
{
    using System.Net.Http;

    /// <summary>
    /// Provides a custom method for retrieving the file name of a file uploaded to the server.
    /// </summary>
    public class BaseMultipartFormProvider : MultipartFormDataStreamProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.BaseMultipartFormProvider" /> class.
        /// </summary>
        /// <param name="rootPath">The root path where the content of MIME multipart body parts are written to.</param>
        public BaseMultipartFormProvider(string rootPath) : base(rootPath) { }
        /// <summary>
        /// Gets the name of the local file which will be combined with the root path to create an absolute file name where the contents of the current MIME body part will be stored.
        /// </summary>
        /// <param name="headers">The headers for the current MIME body part.</param>
        /// <returns>
        /// A relative filename with no path component.
        /// </returns>
        public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
        {
            var name = !string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName) ? headers.ContentDisposition.FileName : "NoName";
            return name.Replace("\"", string.Empty); //this is here because Chrome submits files in quotation marks which get treated as part of the filename and get escaped
        }
    }
}
