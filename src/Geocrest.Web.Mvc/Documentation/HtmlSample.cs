namespace Geocrest.Web.Mvc.Documentation
{
    using System.Web;

    /// <summary>
    /// Base class for rendering HTML markup as a sample.  There's a display template named HtmlSample associated with this class.
    /// </summary>
    public class HtmlSample
    {
        private string[] scriptbundles = new string[]{};
        private string[] stylebundles = new string[] { };
        
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Documentation.HtmlSample" /> class.
        /// </summary>
        /// <param name="html">The HTML to render in the page.</param>
        /// <exception cref="T:System.ArgumentNullException">html</exception>
        public HtmlSample(string html) : this(html,false) { }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Documentation.HtmlSample" /> class.
        /// </summary>
        /// <param name="html">The HTML to render in the page.</param>
        /// <param name="isPartial">Whether or not the Html property is a file path to render as a partial view.</param>
        public HtmlSample(string html, bool isPartial)
        {
            Html = html;
            IsPartialView = isPartial;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Documentation.HtmlSample" /> class.
        /// </summary>
        public HtmlSample() : this(string.Empty,false) { }

        /// <summary>
        /// Gets a value indicating whether the Html references a partial view.
        /// </summary>
        /// <value>
        /// <b>true</b>, if the HTML property is a partial view; otherwise, <b>false</b>.
        /// </value>
        public bool IsPartialView { get; private set; }
        
        /// <summary>
        /// Gets the HTML to render in the page.
        /// </summary>
        public virtual string Html { get; private set; }
        
        /// <summary>
        /// Gets the html for any script bundles registered with this sample.
        /// </summary>       
        public virtual IHtmlString Scripts 
        { 
            get 
            {                
                return System.Web.Optimization.Scripts.Render(this.scriptbundles); 
            } 
        }

        /// <summary>
        /// Gets the html for any style bundles registered with this sample.
        /// </summary>
        public virtual IHtmlString Styles
        {
            get
            {
                return System.Web.Optimization.Styles.Render(this.stylebundles);
            }
        }
        
        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object" /> to compare with this instance.</param>
        /// <returns>
        /// <b>true</b>, if the specified <see cref="T:System.Object" /> is equal to this instance; otherwise, <b>false</b>.
        /// </returns>
        public override bool Equals(object obj)
        {
            HtmlSample other = obj as HtmlSample;
            return other != null && Html == other.Html;
        }
        
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Html.GetHashCode();
        }

        /// <summary>
        /// Registers the given scripts to be rendered on the output page.
        /// </summary>
        /// <param name="paths">The paths to any script bundles.</param>
        /// <returns>
        /// Returns the current instance for chaining method calls.
        /// </returns>
        public virtual HtmlSample WithScripts(params string[] paths)
        {
            if (paths != null) this.scriptbundles = paths;
            return this;
        }

        /// <summary>
        /// Registers the given styles to be rendered on the output page.
        /// </summary>
        /// <param name="paths">The paths.</param>
        /// <returns>
        /// Returns the current instance for chaining method calls.
        /// </returns>
        public virtual HtmlSample WithStyles(params string[] paths)
        {
            if (paths != null) this.stylebundles = paths;
            return this;
        }
    }
}
