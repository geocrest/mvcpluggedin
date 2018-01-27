
namespace Geocrest.Model.ArcGIS.WebMap
{
    using System.Runtime.Serialization;
    
    /// <summary>
    /// Represents input for a definition editor.
    /// </summary>
    public class Input
    {
        /// <summary>
        /// Gets or sets the hint.
        /// </summary>
        /// <value>
        /// The hint.
        /// </value>
        [DataMember(Name = "hint")]
        public string Hint { get; set; }

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        [DataMember(Name = "parameters")]
        public Parameter[] Parameters { get; set; }

        /// <summary>
        /// Gets or sets the prompt.
        /// </summary>
        /// <value>
        /// The prompt.
        /// </value>
        [DataMember(Name = "prompt")]
        public string Prompt { get; set; }
    }
}
