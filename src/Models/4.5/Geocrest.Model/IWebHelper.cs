namespace Geocrest.Model
{
    using System;
    using System.Collections.Specialized;
    using System.Net;
    using System.Text;

    /// <summary>
    /// Provides access to methods for downloading resources from the web.
    /// </summary>
    public interface IWebHelper
    {
        /// <summary>
        /// Gets and sets the <see cref="T:System.Text.Encoding"/> used to upload and download strings.
        /// </summary>
        /// <value>
        /// An <see cref="T:System.Text.Encoding"/> that is used to encode strings. The default value 
        /// of this property is the encoding returned by <see cref="P:System.Text.Encoding.Default"/>.
        /// </value>
        Encoding Encoding { get; set; }
        /// <summary>
        /// Gets or sets a collection of header name/value pairs associated with the request.
        /// </summary>
        WebHeaderCollection Headers { get; set;}

        /// <summary>
        /// Gets a collection of header name/value pairs associated with the response.
        /// </summary>
        WebHeaderCollection ResponseHeaders { get; }

        /// <summary>
        /// Occurs when an asynchronous resource-download operation completes.
        /// </summary>
        event EventHandler<WrappedDownloadStringCompletedEventArgs> DownloadStringCompleted;

        /// <summary>
        /// Gets a value that indicates whether a Web request is in progress.
        /// </summary>
        /// <value>
        ///   <c>true</c> if busy; otherwise, <c>false</c>.
        /// </value>
        bool IsBusy { get; }

        /// <summary>
        /// Creates a new instance of a web helper.
        /// </summary>
        IWebHelper GetWebHelper();

        /// <summary>
        /// Downloads the resource specified as a <see cref="T:System.Uri"/>. This method does not 
        /// block the calling thread.
        /// </summary>
        /// <param name="address">A <see cref="T:System.Uri"/> containing the URI to download.</param>      
        void DownloadStringAsync(Uri address);

        /// <summary>
        /// Downloads the specified string to the specified resource. This method does not 
        /// block the calling thread.
        /// </summary>
        /// <param name="address">A <see cref="T:System.Uri"/> containing the URI to download.</param>
        /// <param name="userToken">A user-defined object that is passed to the method invoked 
        /// when the asynchronous operation completes.</param>       
        void DownloadStringAsync(Uri address, object userToken);
        /// <summary>
        /// Downloads the requested resource as a <see cref="T:System.String" />. The resource to download
        /// is specified as a <see cref="T:System.String" /> containing the URI.
        /// </summary>
        /// <param name="address">A <see cref="T:System.String" /> containing the URI to download.</param>
        /// <returns>
        /// A <see cref="T:System.String" /> containing the requested resource.
        /// </returns>       
        string DownloadString(string address);
        /// <summary>
        /// Downloads the requested resource as a <see cref="T:System.String" />. The resource to download
        /// is specified as a <see cref="T:System.Uri"/>.
        /// </summary>
        /// <param name="address">A <see cref="T:System.Uri"/> object containing the URI to download.</param>
        /// <returns>
        /// A <see cref="T:System.String" /> containing the requested resource.
        /// </returns>       
        string DownloadString(Uri address);

        /// <summary>
        /// Occurs when an asynchronous resource-upload operation completes.
        /// </summary>
        event EventHandler<WrappedUploadStringCompletedEventArgs> UploadStringCompleted;
        /// <summary>
        /// Occurs when an asynchronous upload of a name/value collection completes.
        /// </summary>
        event EventHandler<WrappedUploadValuesCompletedEventArgs> UploadValuesCompleted;
        /// <summary>
        /// Cancels the pending asynchronous operation.
        /// </summary>
        void CancelAsync();

        /// <summary>
        /// Uploads the specified string to the specified resource. This method does
        /// not block the calling thread.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the string. 
        /// For HTTP resources, this URI must identify a resource that can accept a 
        /// request sent with the POST method, such as a script or ASP page.</param>
        /// <param name="data">The string to be uploaded.</param>
        void UploadStringAsync(Uri address, string data);

        /// <summary>
        /// Uploads the specified string to the specified resource. This method does
        /// not block the calling thread.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the string.
        /// For HTTP resources, this URI must identify a resource that can accept a
        /// request sent with the POST method, such as a script or ASP page.</param>
        /// <param name="method">The HTTP method used to send the file to the resource. If null, the default
        /// is POST for HTTP.</param>
        /// <param name="data">The string to be uploaded.</param>       
        void UploadStringAsync(Uri address, string method, string data);

        /// <summary>
        /// Uploads the specified string to the specified resource. This method does
        /// not block the calling thread.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the string.
        /// For HTTP resources, this URI must identify a resource that can accept a
        /// request sent with the POST method, such as a script or ASP page.</param>
        /// <param name="method">The HTTP method used to send the file to the resource. If null, the default
        /// is POST for HTTP.</param>
        /// <param name="data">The string to be uploaded.</param>
        /// <param name="userToken">A user-defined object that is passed to the method invoked 
        /// when the asynchronous operation completes.</param>       
        void UploadStringAsync(Uri address, string method, string data, object userToken);

        /// <summary>
        /// Uploads the specified name/value collection to the resource identified by the specified URI.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the collection.</param>
        /// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection"/> to 
        /// send to the resource.</param>
        /// <returns>
        /// A <see cref="T:System.Byte[][]"/> array containing the body of the response from the resource.
        /// </returns>
        byte[] UploadValues(string address, NameValueCollection data);
        /// <summary>
        /// Uploads the specified name/value collection to the resource identified by the specified URI.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the collection.</param>
        /// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection"/> to 
        /// send to the resource.</param>
        /// <returns>
        /// A <see cref="T:System.Byte[][]"/> array containing the body of the response from the resource.
        /// </returns>
        byte[] UploadValues(Uri address, NameValueCollection data);
        /// <summary>
        /// Uploads the specified name/value collection to the resource identified by the specified URI, 
        /// using the specified method.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the collection.</param>
        /// <param name="method">The HTTP method used to send the file to the resource. If null, the 
        /// default is POST for http and STOR for ftp.</param>
        /// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection" /> to 
        /// send to the resource.</param>
        /// <returns>
        /// A <see cref="T:System.Byte[][]" /> array containing the body of the response from the resource.
        /// </returns>
        byte[] UploadValues(string address, string method, NameValueCollection data);
        /// <summary>
        /// Uploads the specified name/value collection to the resource identified by the specified URI, 
        /// using the specified method.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the collection.</param>
        /// <param name="method">The HTTP method used to send the file to the resource. If null, the 
        /// default is POST for http and STOR for ftp.</param>
        /// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection" /> to 
        /// send to the resource.</param>
        /// <returns>
        /// A <see cref="T:System.Byte[][]" /> array containing the body of the response from the resource.
        /// </returns>
        byte[] UploadValues(Uri address, string method, NameValueCollection data);
        /// <summary>
        /// Uploads the data in the specified name/value collection to the resource identified by 
        /// the specified URI. 
        /// This method does not block the calling thread.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the collection. This URI must 
        /// identify a resource that can 
        /// accept a request sent with the default method.</param>
        /// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection"/> to 
        /// send to the resource.</param>
        void UploadValuesAsync(Uri address, NameValueCollection data);
        /// <summary>
        /// Uploads the data in the specified name/value collection to the resource identified by the specified URI. 
        /// This method does not block the calling thread.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the collection. This URI must 
        /// identify a resource that can 
        /// accept a request sent with the method method.</param>
        /// <param name="method">The method used to send the string to the resource. If null, the 
        /// default is POST for http and STOR for ftp.</param>
        /// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection"/> to 
        /// send to the resource.</param>
        void UploadValuesAsync(Uri address, string method, NameValueCollection data);
        /// <summary>
        /// Uploads the data in the specified name/value collection to the resource identified by the specified URI, 
        /// using the specified method. This method does not block the calling thread, and allows the caller to 
        /// pass an object to the method that is invoked when the operation completes.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the collection. This URI must identify a 
        /// resource that can accept a request sent with the method method.</param>
        /// <param name="method">The HTTP method used to send the string to the resource. If null, the 
        /// default is POST for http and STOR for ftp.</param>
        /// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection"/> to 
        /// send to the resource.</param>
        /// <param name="userToken">A user-defined object that is passed to the method invoked 
        /// when the asynchronous operation completes.</param>
        void UploadValuesAsync(Uri address, string method, NameValueCollection data, object userToken);
    }
}
