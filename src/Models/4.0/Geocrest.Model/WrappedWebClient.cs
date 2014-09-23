namespace Geocrest.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Net;
    using System.Text;

    /// <summary>
    /// Wraps a <see cref="T:System.Net.WebClient"/> class to enable unit testing.
    /// </summary>
    public class WrappedWebClient : IWebHelper
    {
        /// <summary>
        /// Gets the <see cref="T:System.Net.WebClient"/> that this instance is wrapped around.
        /// </summary>
        public WebClient Client { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.WrappedWebClient"/> class.
        /// </summary>
        public WrappedWebClient() : this(new WebClient())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.WrappedWebClient"/> class.
        /// </summary>
        /// <param name="webClient">The web client.</param>
        public WrappedWebClient(WebClient webClient)
        {
            if (webClient == null) throw new ArgumentNullException("webClient");
            this.Client = webClient;
            this.Client.UseDefaultCredentials = true;
        }

        #region IWebHelper Members
        /// <summary>
        /// Gets and sets the <see cref="T:System.Text.Encoding" /> used to upload and download strings.
        /// </summary>
        /// <value>
        /// An <see cref="T:System.Text.Encoding" /> that is used to encode strings. The default value
        /// of this property is the encoding returned by <see cref="P:System.Text.Encoding.Default" />.
        /// </value>
        public Encoding Encoding
        {
            get { return this.Client.Encoding; }
            set { this.Client.Encoding = value; }
        }

        /// <summary>
        /// Gets or sets a collection of header name/value pairs associated with the request.
        /// </summary>
        public WebHeaderCollection Headers
        {
            get { return this.Client.Headers; }
            set { this.Client.Headers = value; }
        }

        /// <summary>
        /// Gets a collection of header name/value pairs associated with the response.
        /// </summary>
        public WebHeaderCollection ResponseHeaders
        {
            get { return this.Client.ResponseHeaders; }
        }
        /// <summary>
        /// Gets a value that indicates whether a Web request is in progress.
        /// </summary>
        /// <value>
        ///   <c>true</c> if busy; otherwise, <c>false</c>.
        /// </value>
        public bool IsBusy
        {
            get { return this.Client.IsBusy; }
        }
        /// <summary>
        /// Occurs when an asynchronous resource-download operation completes.
        /// </summary>
        public event EventHandler<WrappedDownloadStringCompletedEventArgs> DownloadStringCompleted;
        /// <summary>
        /// Occurs when an asynchronous resource-upload operation completes.
        /// </summary>
        public event EventHandler<WrappedUploadStringCompletedEventArgs> UploadStringCompleted;
        /// <summary>
        /// Occurs when an asynchronous upload of a name/value collection completes.
        /// </summary>
        public event EventHandler<WrappedUploadValuesCompletedEventArgs> UploadValuesCompleted;
        /// <summary>
        /// Cancels the pending asynchronous operation.
        /// </summary>
        public void CancelAsync()
        {
            Client.CancelAsync();
        }
        /// <summary>
        /// Creates a new instance of a web helper.
        /// </summary>
        /// <returns></returns>
        public IWebHelper GetWebHelper()
        {
            return new WrappedWebClient();
        }

        /// <summary>
        /// Downloads the resource specified as a <see cref="T:System.Uri" />. This method does not block the calling thread.
        /// </summary>
        /// <param name="address">A <see cref="T:System.Uri" /> containing the URI to download.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="address"/> parameter is null.</exception>
        /// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress"/> 
        /// and <paramref name="address"/> is invalid.
        /// -or- 
        /// An error occurred while downloading the resource.</exception>
        public void DownloadStringAsync(Uri address)
        {
            this.DownloadStringAsync(address, null);
        }
        /// <summary>
        /// Downloads the specified string to the specified resource. This method does not block the calling thread.
        /// </summary>
        /// <param name="address">A <see cref="T:System.Uri" /> containing the URI to download.</param>
        /// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous
        /// operation completes.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="address"/> parameter is null.</exception>
        /// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress"/> 
        /// and <paramref name="address"/> is invalid.
        /// -or- 
        /// An error occurred while downloading the resource.</exception>
        public void DownloadStringAsync(Uri address, object userToken)
        {
            this.Client = new WebClient();
            this.Client.UseDefaultCredentials = true;
            HookToClientEvents();
            var token = new List<object>() { address };
            if (userToken != null) token.Add(userToken);
            this.Client.DownloadStringAsync(address, token.ToArray());
        }

        /// <summary>
        /// Downloads the requested resource as a <see cref="T:System.String" />. The resource to download
        /// is specified as a <see cref="T:System.String" /> containing the URI.
        /// </summary>
        /// <param name="address">A <see cref="T:System.String" /> containing the URI to download.</param>
        /// <returns>
        /// A <see cref="T:System.String" /> containing the requested resource.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="address"/> parameter is null.</exception>
        /// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress"/> 
        /// and <paramref name="address"/> is invalid.
        /// -or- 
        /// An error occurred while downloading the resource.</exception>
        /// <exception cref="T:System.NotSupportedException">The method has been called simultaneously on multiple threads.</exception>
        public string DownloadString(string address)
        {
            this.Client = new WebClient();
            this.Client.UseDefaultCredentials = true;
            return this.Client.DownloadString(address);
        }
        /// <summary>
        /// Downloads the requested resource as a <see cref="T:System.String" />. The resource to download
        /// is specified as a <see cref="T:System.Uri" />.
        /// </summary>
        /// <param name="address">A <see cref="T:System.Uri" /> object containing the URI to download.</param>
        /// <returns>
        /// A <see cref="T:System.String" /> containing the requested resource.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="address"/> parameter is null.</exception>
        /// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress"/> 
        /// and <paramref name="address"/> is invalid.
        /// -or- 
        /// An error occurred while downloading the resource.</exception>
        /// <exception cref="T:System.NotSupportedException">The method has been called simultaneously on multiple threads.</exception>
        public string DownloadString(Uri address)
        {
            this.Client = new WebClient();
            this.Client.UseDefaultCredentials = true;
            return this.Client.DownloadString(address);
        }

        /// <summary>
        /// Uploads the specified string to the specified resource. This method does
        /// not block the calling thread.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the string.
        /// For HTTP resources, this URI must identify a resource that can accept a
        /// request sent with the POST method, such as a script or ASP page.</param>
        /// <param name="data">The string to be uploaded.</param>
        /// <exception cref="T:System.ArgumentNullException">The address parameter is null
        /// -or- 
        /// data is null.</exception>
        /// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress"/> 
        /// and <paramref name="address"/> is invalid.
        /// -or- 
        /// An error occurred while downloading the resource.</exception>
        public void UploadStringAsync(Uri address, string data)
        {
            this.UploadStringAsync(address, "POST", data);
        }
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
        /// <exception cref="T:System.ArgumentNullException">The address parameter is null
        /// -or- 
        /// data is null.</exception>
        /// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress"/> 
        /// and <paramref name="address"/> is invalid.
        /// -or- 
        /// An error occurred while downloading the resource.</exception>
        public void UploadStringAsync(Uri address, string method, string data)
        {
            this.UploadStringAsync(address, method, data, null);
        }
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
        /// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous
        /// operation completes.</param>
        /// <exception cref="T:System.ArgumentNullException">The address parameter is null
        /// -or- 
        /// data is null.</exception>        
        /// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress"/> 
        /// and <paramref name="address"/> is invalid.
        /// -or- 
        /// An error occurred while downloading the resource.</exception>
        public void UploadStringAsync(Uri address, string method, string data, object userToken)
        {
            this.Client = new WebClient();
            this.Client.UseDefaultCredentials = true;
            HookToClientEvents();
            var token = new List<object>() { address };
            if (userToken != null) token.Add(userToken);
            this.Client.UploadStringAsync(address, method, data, token.ToArray());
        }

        /// <summary>
        /// Uploads the specified name/value collection to the resource identified by the specified URI.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the collection.</param>
        /// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection" /> to send to the resource.</param>
        /// <returns>
        /// A <see cref="T:System.Byte[][]" /> array containing the body of the response from the resource.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="address"/> parameter is null.
        /// -or-
        /// the <paramref name="data"/> parameter is null.</exception>
        /// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress"/> 
        /// and <paramref name="address"/> is invalid.
        /// -or- 
        /// <paramref name="data"/> is null.
        /// -or-
        /// There was no response from the server hosting the resource.
        /// -or-
        /// An error occurred while opening the stream.
        /// -or-
        /// The <b>Content-type</b> header is not <b>null</b> or "application/x-www-form-urlencoded".</exception>
        public byte[] UploadValues(string address, NameValueCollection data)
        {
            return this.UploadValues(address, data);
        }
        /// <summary>
        /// Uploads the specified name/value collection to the resource identified by the specified URI.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the collection.</param>
        /// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection" /> to send to the resource.</param>
        /// <returns>
        /// A <see cref="T:System.Byte[][]" /> array containing the body of the response from the resource.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="address"/> parameter is null.
        /// -or-
        /// the <paramref name="data"/> parameter is null.</exception>
        /// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress"/> 
        /// and <paramref name="address"/> is invalid.
        /// -or- 
        /// <paramref name="data"/> is null.
        /// -or-
        /// There was no response from the server hosting the resource.
        /// -or-
        /// An error occurred while opening the stream.
        /// -or-
        /// The <b>Content-type</b> header is not <b>null</b> or "application/x-www-form-urlencoded".</exception>
        public byte[] UploadValues(Uri address, NameValueCollection data)
        {
            return this.UploadValues(address, data);
        }
        /// <summary>
        /// Uploads the specified name/value collection to the resource identified by the specified URI, using the specified method.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the collection.</param>
        /// <param name="method">The HTTP method used to send the file to the resource. If null, the default is POST for http and STOR for ftp.</param>
        /// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection" /> to send to the resource.</param>
        /// <returns>
        /// A <see cref="T:System.Byte[][]" /> array containing the body of the response from the resource.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="address"/> parameter is null.
        /// -or-
        /// the <paramref name="data"/> parameter is null.</exception>
        /// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress"/> 
        /// and <paramref name="address"/> is invalid.
        /// -or- 
        /// <paramref name="data"/> is null.
        /// -or-
        /// There was no response from the server hosting the resource.
        /// -or-
        /// An error occurred while opening the stream.
        /// -or-
        /// The <b>Content-type</b> header is not <b>null</b> and is not <b>application/x-www-form-urlencoded</b>.</exception>
        public byte[] UploadValues(string address, string method, NameValueCollection data)
        {
            return this.Client.UploadValues(address, method, data);
        }
        /// <summary>
        /// Uploads the specified name/value collection to the resource identified by the specified URI, using the specified method.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the collection.</param>
        /// <param name="method">The HTTP method used to send the file to the resource. If null, the default is POST for http and STOR for ftp.</param>
        /// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection" /> to send to the resource.</param>
        /// <returns>
        /// A <see cref="T:System.Byte[][]" /> array containing the body of the response from the resource.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="address"/> parameter is null.
        /// -or-
        /// the <paramref name="data"/> parameter is null.</exception>
        /// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress"/> 
        /// and <paramref name="address"/> is invalid.
        /// -or- 
        /// <paramref name="data"/> is null.
        /// -or-
        /// There was no response from the server hosting the resource.
        /// -or-
        /// An error occurred while opening the stream.
        /// -or-
        /// The <b>Content-type</b> header is not <b>null</b> and is not <b>application/x-www-form-urlencoded</b>.</exception>
        public byte[] UploadValues(Uri address, string method, NameValueCollection data)
        {
            return this.Client.UploadValues(address, method, data);
        }

        /// <summary>
        /// Uploads the data in the specified name/value collection to the resource identified by
        /// the specified URI.
        /// This method does not block the calling thread.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the collection. This URI must
        /// identify a resource that can
        /// accept a request sent with the default method.</param>
        /// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection" /> to
        /// send to the resource.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="address"/> parameter is null.
        /// -or-
        /// the <paramref name="data"/> parameter is null.</exception>
        /// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress"/> 
        /// and <paramref name="address"/> is invalid.
        /// -or-
        /// There was no response from the server hosting the resource.</exception>
        public void UploadValuesAsync(Uri address, NameValueCollection data)
        {            
            this.UploadValuesAsync(address, "POST", data);
        }
        /// <summary>
        /// Uploads the data in the specified name/value collection to the resource identified by the specified URI.
        /// This method does not block the calling thread.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the collection. This URI must
        /// identify a resource that can
        /// accept a request sent with the method method.</param>
        /// <param name="method">The method used to send the string to the resource. If null, the
        /// default is POST for http and STOR for ftp.</param>
        /// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection" /> to
        /// send to the resource.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="address"/> parameter is null.
        /// -or-
        /// the <paramref name="data"/> parameter is null.</exception>
        /// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress"/> 
        /// and <paramref name="address"/> is invalid.
        /// -or-
        /// There was no response from the server hosting the resource.
        /// -or-
        /// <paramref name="method"/> cannot be used to send content.</exception>
        public void UploadValuesAsync(Uri address, string method, NameValueCollection data)
        {
            this.UploadValuesAsync(address, method, data, null);
        }
        /// <summary>
        /// Uploads the data in the specified name/value collection to the resource identified by the specified URI,
        /// using the specified method. This method does not block the calling thread, and allows the caller to
        /// pass an object to the method that is invoked when the operation completes.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the collection. This URI must identify a
        /// resource that can accept a request sent with the method method.</param>
        /// <param name="method">The HTTP method used to send the string to the resource. If null, the
        /// default is POST for http and STOR for ftp.</param>
        /// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection" /> to
        /// send to the resource.</param>
        /// <param name="userToken">A user-defined object that is passed to the method invoked
        /// when the asynchronous operation completes.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="address"/> parameter is null.
        /// -or-
        /// the <paramref name="data"/> parameter is null.</exception>
        /// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress"/> 
        /// and <paramref name="address"/> is invalid.
        /// -or-
        /// There was no response from the server hosting the resource.
        /// -or-
        /// <paramref name="method"/> cannot be used to send content.</exception>
        public void UploadValuesAsync(Uri address, string method, NameValueCollection data, object userToken)
        {
            this.Client = new WebClient();
            this.Client.UseDefaultCredentials = true;
            HookToClientEvents();
            var token = new List<object>() { address };
            if (userToken != null) token.Add(userToken);
            this.Client.UploadValuesAsync(address, method, data, token.ToArray());
        }
        #endregion

        #region Protected
        /// <summary>
        /// Raises the <see cref="E:Geocrest.Model.WrappedWebClient.UploadValuesCompleted" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Net.UploadValuesCompletedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnUploadValuesCompleted(UploadValuesCompletedEventArgs e)
        {
            EventHandler<WrappedUploadValuesCompletedEventArgs> handler = UploadValuesCompleted;
            if (handler != null)
                handler(this, new WrappedUploadValuesCompletedEventArgs(e));
        }

        /// <summary>
        /// Raises the <see cref="E:Geocrest.Model.WrappedWebClient.DownloadStringCompleted" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Net.DownloadStringCompletedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDownloadStringCompleted(DownloadStringCompletedEventArgs e)
        {
            EventHandler<WrappedDownloadStringCompletedEventArgs> handler = DownloadStringCompleted;
            if (handler != null)
                handler(this, new WrappedDownloadStringCompletedEventArgs(e));
        }
        /// <summary>
        /// Raises the <see cref="E:Geocrest.Model.WrappedWebClient.UploadStringCompleted" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Net.UploadStringCompletedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnUploadStringCompleted(UploadStringCompletedEventArgs e)
        {
            EventHandler<WrappedUploadStringCompletedEventArgs> handler = UploadStringCompleted;
            if (handler != null)
                handler(this, new WrappedUploadStringCompletedEventArgs(e));
        }
        #endregion

        #region Private
        private void HookToClientEvents()
        {
            Client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(Client_DownloadStringCompleted);
            Client.UploadStringCompleted += new UploadStringCompletedEventHandler(Client_UploadStringCompleted);
            Client.UploadValuesCompleted += new UploadValuesCompletedEventHandler(Client_UploadValuesCompleted);
        }
        private void UnHookFromClientEvents()
        {
            Client.DownloadStringCompleted -= new DownloadStringCompletedEventHandler(Client_DownloadStringCompleted);
            Client.UploadStringCompleted -= new UploadStringCompletedEventHandler(Client_UploadStringCompleted);
            Client.UploadValuesCompleted -= new UploadValuesCompletedEventHandler(Client_UploadValuesCompleted);
        }
        private void Client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            UnHookFromClientEvents();
            OnDownloadStringCompleted(e);
        }
        private void Client_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            UnHookFromClientEvents();
            OnUploadStringCompleted(e);
        }
        private void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            UnHookFromClientEvents();
            OnUploadValuesCompleted(e);
        }
        #endregion
    }
}
