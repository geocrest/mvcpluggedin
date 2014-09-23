
namespace Geocrest.Web.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;

    /// <summary>
    /// Provides extension method for string objects
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Forces the json format using the query string 'f' parameter.
        /// </summary>
        /// <param name="url">The URL to check.</param>
        /// <returns>
        /// The input URL but with a query parameter of 'f=json'.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">url</exception>
        public static string ForceJsonFormat(this string url)
        {
            Throw.IfArgumentNullOrEmpty(url, "url");
            string json = "json";
            string f = "f";
            string query = string.Empty;
            string root = string.Empty;
            if (url.Contains(f + "=" + json)) return url;
            // get root url using string manipulation instead of building a Uri so items like
            // port number and a trailing slash are not added. Want to return url as it was entered
            root = url.Contains("?") ? url.Replace(url.Substring(url.IndexOf("?")), "") : url;

            // set the 'f' parameter of query string
            Uri uri = new Uri(url);
            NameValueCollection queryparams = HttpUtility.ParseQueryString(uri.Query);
            if (queryparams.AllKeys.Contains(f))
            {
                queryparams[f] = json;
            }
            else
            {
                queryparams.Add(f, json);
            }
            query = "?" + queryparams.ToString();

            // build the new Uri            
            return root + query;
        }
        /// <summary>
        /// Verifies a string representation of a version (e.g. Major.Minor.Build.Revision) is valid.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <returns>
        /// Returns a <see cref="T:System.Boolean">System.Boolean</see> value indicating if the version 
        /// complies with the Major.Minor.Build.Revision format.
        /// </returns>
        public static bool IsValidVersionNumber(this string version)
        {
            if (string.IsNullOrEmpty(version)) return false;
            if (!version.Contains(".")) // version is a single segment (e.g. 1)
            {
                int versionInt;
                if (!Int32.TryParse(version, NumberStyles.None, CultureInfo.InvariantCulture, out versionInt))
                    return false;
                else
                    return true;
            }
            else // verify all parts of the version (i.e. 1.2.3.4) are integers
            {
                var segments = version.Split(new[] { "." }, StringSplitOptions.None);

                // should only contain up to four parts
                if (segments.Length > 4) return false;
                foreach (var segment in segments)
                {
                    int versionInt;
                    if (!Int32.TryParse(segment, NumberStyles.None, CultureInfo.InvariantCulture, out versionInt))
                        return false;
                }
                return true;
            }
        }
        /// <summary>
        /// Converts a string representation of a version number into a <see cref="T:System.Version"/> object.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        public static Version GetVersion(this string version)
        {
            Throw.IfArgumentNullOrEmpty(version, "version");
            if (!version.IsValidVersionNumber())
                return new Version();
            if (!version.Contains("."))
            {
                int versionInt = Int32.Parse(version, NumberStyles.None, CultureInfo.InvariantCulture);
                return new Version(versionInt, 0);
            }
            else
            {
                var segments = version.Split(new[] { "." }, StringSplitOptions.None);
                int maj = 0; int min = 0; int b = 0; int r = 0;
                switch (segments.Length)
                {
                    case 4:
                        maj = Int32.Parse(segments[0]);
                        min = Int32.Parse(segments[1]);
                        b = Int32.Parse(segments[2]);
                        r = Int32.Parse(segments[3]);
                        return new Version(maj, min, b, r);
                    case 3:
                        maj = Int32.Parse(segments[0]);
                        min = Int32.Parse(segments[1]);
                        b = Int32.Parse(segments[2]);
                        return new Version(maj, min, b);
                    case 2:
                        maj = Int32.Parse(segments[0]);
                        min = Int32.Parse(segments[1]);
                        return new Version(maj, min);
                    case 1:
                        maj = Int32.Parse(segments[0]);
                        return new Version(maj, 0);
                    default:
                        return new Version();
                }
            }
        }
        /// <summary>
        /// Characters used for delimiting a coordinate list: ',', ' ', '|'.
        /// </summary>
        internal static char[] SplitChars = { ',', ' ', '|' };

        /// <summary>
        /// Parses a string containing a coordinate pair.
        /// </summary>
        /// <param name="coordinates">A string in the format x,y.</param>
        /// <returns>A double array where index zero = X and index one = Y.</returns>
        public static double[] ParseCoordinates(this string coordinates)
        {
            Throw.IfArgumentNullOrEmpty(coordinates, "coordinates");
            string[] s = coordinates.Split(SplitChars);
            double x = Convert.ToDouble(s[0]);
            double y = Convert.ToDouble(s[1]);
            double[] d = new double[2];
            d[0] = x;
            d[1] = y;
            return d;
        }

        /// <summary>
        /// Parses a string containing a coordinate pair.
        /// </summary>
        /// <param name="coordinates">A string in the format x,y.</param>
        /// <param name="x">A reference for the X position at index zero.</param>
        /// <param name="y">A reference for the Y position at index one.</param>
        /// <returns>True if no exceptions are raised.</returns>
        public static bool ParseCoordinates(this string coordinates, ref double x, ref double y)
        {
            Throw.IfArgumentNullOrEmpty(coordinates, "coordinates");
            string[] s = coordinates.Split(SplitChars);
            x = Convert.ToDouble(s[0]);
            y = Convert.ToDouble(s[1]);
            return true;
        }
        /// <summary>
        /// Generates an anchor tag from the portions of a string that contain an http:// or https:// segment.
        /// </summary>
        /// <param name="html">The HTML containing text to linkify.</param>
        /// <returns>The input HTML with http segments converted to anchor tags.</returns>
        public static string Linkify(this string html)
        {
            if (string.IsNullOrEmpty(html)) return string.Empty;
            var value = html;
            if (value.ToLower().Contains("http://") || value.ToLower().Contains("https://"))
            {
                string[] segments = value.Split(' ');
                List<string> newsegments = new List<string>();
                foreach (string segment in segments)
                {
                    string text = segment;
                    if (segment.ToLower().StartsWith("http://") || segment.ToLower().StartsWith("https://"))
                    {
                        text = string.Format("<a href='{0}' target='_blank' title='{0}' rel='tooltip'>{0}</a>", segment);
                    }
                    newsegments.Add(text);
                }
                value = string.Join(" ", newsegments);
            }
            return value;
        }
        /// <summary>
        /// Slugifies a string
        /// </summary>
        /// <param name="value">The string value to slugify</param>
        /// <param name="maxLength">An optional maximum length of the generated slug</param>
        /// <returns>A URL safe slug representation of the input <paramref name="value"/>.</returns>
        public static string ToSlug(this string value, int? maxLength = null)
        {
            Throw.IfArgumentNullOrEmpty(value, "value");

            // if it's already a valid slug, return it
            if (RegexUtils.SlugRegex.IsMatch(value))
                return value;

            return GenerateSlug(value, maxLength);
        }
        /// <summary>
        /// Credit for this method goes to http://stackoverflow.com/questions/2920744/url-slugify-alrogithm-in-cs
        /// </summary>
        private static string GenerateSlug(string value, int? maxLength = null)
        {
            // prepare string, remove accents, lower case and convert hyphens to whitespace
            var result = RemoveAccent(value).Replace("-", " ").ToLowerInvariant();

            result = Regex.Replace(result, @"[^a-z0-9\s-]", string.Empty); // remove invalid characters
            result = Regex.Replace(result, @"\s+", " ").Trim(); // convert multiple spaces into one space

            if (maxLength.HasValue) // cut and trim
                result = result.Substring(0, result.Length <= maxLength ? result.Length : maxLength.Value).Trim();

            return Regex.Replace(result, @"\s", "-"); // replace all spaces with hyphens
        }
        private static string RemoveAccent(string value)
        {
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);
            return Encoding.ASCII.GetString(bytes);
        }
    }
}
