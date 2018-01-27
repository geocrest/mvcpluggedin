
namespace Geocrest.Model.ArcGIS.WebMap
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Provides interactive filters for a layer in an ArcGIS web map.
    /// </summary>
    public class DefinitionEditor
    {
        /// <summary>
        /// Gets or sets the inputs.
        /// </summary>
        /// <value>
        /// The inputs.
        /// </value>
        [DataMember(Name = "inputs")]
        public Input[] Inputs { get; set; }

        /// <summary>
        /// Gets or sets the parameterized expression.
        /// </summary>
        /// <value>
        /// The parameterized expression.
        /// </value>
        [DataMember(Name = "parameterizedExpression")]
        public string ParameterizedExpression { get; set; }
    }
}
