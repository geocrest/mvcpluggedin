
namespace Geocrest.Web.Tokens
{
    /// <summary>
    /// A utility class for handling special characters in URLs.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Initializes the <see cref="T:Geocrest.Web.Tokens.Utility"/> class.
        /// </summary>
        static Utility()
        {
        }

        /// <summary>
        /// Replaces forward slashes (/) in the specified token with underscores (_).
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>
        /// Returns a token with underscores instead of forward slashes.
        /// </returns>
        public static string UrlEncodeToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return "";
            else
                return token.Replace('/', '_').Replace('+', '-').Replace('=', '.');
        }

        /// <summary>
        /// Replaces underscores (_) in the specified token with forward slashes (/).
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>
        /// Returns a token with forward slashes instead of underscores.
        /// </returns>
        public static string UrlDecodeToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return "";
            else
                return token.Replace('_', '/').Replace('-', '+').Replace('.', '=');
        }
    }
}
