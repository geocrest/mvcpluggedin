namespace Geocrest.Web.Mvc.Documentation
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Net.Http.Headers;

    /// <summary>
    /// The model that represents an API displayed on the help page.
    /// </summary>
    public class HelpPageApiModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Documentation.HelpPageApiModel" /> class.
        /// </summary>
        public HelpPageApiModel()
        {
            SampleRequests = new Dictionary<MediaTypeHeaderValue, object>();
            SampleResponses = new Dictionary<MediaTypeHeaderValue, object>();
            HtmlSamples = new List<HtmlSample>();
            ErrorMessages = new Collection<string>();
            HomePageUrl = string.Empty;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Documentation.HelpPageApiModel" /> class.
        /// </summary>
        /// <param name="homePage">The URL of the help home page.</param>
        public HelpPageApiModel(string homePage) : this()
        {
            HomePageUrl = homePage;
        }
        /// <summary>
        /// Gets or sets the <see cref="T:System.Web.Http.Description.ApiDescription"/> that describes the API.
        /// </summary>
        public System.Web.Http.Description.ApiDescription ApiDescription { get; set; }

        /// <summary>
        /// Gets the sample requests associated with the API.
        /// </summary>
        public IDictionary<MediaTypeHeaderValue, object> SampleRequests { get; private set; }

        /// <summary>
        /// Gets the sample responses associated with the API.
        /// </summary>
        public IDictionary<MediaTypeHeaderValue, object> SampleResponses { get; private set; }

        /// <summary>
        /// Gets the HTML samples associated with the API.
        /// </summary>    
        public List<HtmlSample> HtmlSamples { get; private set; }

        /// <summary>
        /// Gets or sets the URL of the help home page.
        /// </summary>
        /// <value>
        /// The home page.
        /// </value>
        public string HomePageUrl { get; set; }

        /// <summary>
        /// Gets or sets the documentation to display for an API's return value.
        /// </summary>
        /// <value>
        /// The response documentation.
        /// </value>
        public string ResponseDocumentation { get; set; }

        /// <summary>
        /// Gets the error messages associated with this model.
        /// </summary>
        public Collection<string> ErrorMessages { get; private set; }
    }
}