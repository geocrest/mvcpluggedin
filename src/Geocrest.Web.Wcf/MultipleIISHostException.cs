namespace Geocrest.Web.Wcf
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text;

    /// <summary>
    /// Represents an error condition in the MultipleIISHostFactory class.
    /// </summary>
    /// <remarks>
    /// This exception is most likely to be raised when the serving application does not have a
    /// <c>validHosts</c> key, or if the application is served from a site name not found in this key.
    /// </remarks>
    [Serializable]
    public class MultipleIISHostException : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the MultipleIISHostException class.
        /// </summary>
        public MultipleIISHostException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the MultipleIISHostException class.       
        /// </summary>
        /// <param name="message">
        /// A message describing the error.
        /// </param>
        public MultipleIISHostException(string message)
            : base(message) 
        {
        }

        /// <summary>
        /// Initializes a new instance of the MultipleIISHostException class.       
        /// </summary>
        /// <param name="message">
        /// A message describing the error.
        /// </param>
        /// <param name="innerException">
        /// The inner exception related to the error.
        /// </param>
        public MultipleIISHostException(string message, Exception innerException)
            : base(message, innerException) 
        {
        }

        /// <summary>
        /// Initializes a new instance of the MultipleIISHostException class.       
        /// </summary>
        /// <param name="info">
        /// serialization info...
        /// </param>
        /// <param name="context">
        /// streaming context...
        /// </param>
        protected MultipleIISHostException(SerializationInfo info, StreamingContext context)
            : base(info, context) 
        {
        }
    }
}
