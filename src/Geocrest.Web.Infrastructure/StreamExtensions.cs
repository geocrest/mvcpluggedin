namespace Geocrest.Web.Infrastructure
{
    using System;
    using System.IO;

    /// <summary>
    /// Extension methods used with streams.
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Saves the stream to file.
        /// </summary>
        /// <param name="stream">The <see cref="T:System.IO.Stream"/> containing the contents to write.</param>
        /// <param name="fileFullPath">The full path of the file to write the stream.</param>
        public static void SaveStreamToFile(this Stream stream, string fileFullPath)
        {
            if (stream.Length == 0) return;

            // Create a FileStream object to write a stream to a file
            using (FileStream fileStream = System.IO.File.Create(fileFullPath))
            {
                stream.CopyTo(fileStream);
            }
        }
        /// <summary>
        /// Converts the stream to its base 64 string representation.
        /// </summary>
        /// <param name="stream">The stream to convert.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:System.String"/>
        /// </returns>
        public static string ToBase64(this Stream stream)
        {
            byte[] data = new byte[(int)stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(data, 0, (int)stream.Length);
            return Convert.ToBase64String(data);
        }
    }
}
