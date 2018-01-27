
namespace Geocrest.Model.ArcGIS.Tasks
{
    using System.Runtime.Serialization;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Represents information about a single parameter associated with a geoprocessing task.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class GPParameterInfo
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the type of the data.
        /// </summary>
        /// <value>
        /// The type of the data.
        /// </value>
        [DataMember]
        public esriDataType DataType { get; set; }
        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        [DataMember]
        public string DisplayName { get; set; }
        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        /// <value>
        /// The direction.
        /// </value>
        [DataMember]
        public esriDirection Direction { get; set; }
        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        [DataMember]
        public dynamic DefaultValue { get; set; }
        /// <summary>
        /// Gets or sets the type of the parameter.
        /// </summary>
        /// <value>
        /// The type of the parameter.
        /// </value>
        [DataMember]
        public esriParameterType ParameterType { get; set; }
        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        [DataMember]
        public string Category { get; set; }
        /// <summary>
        /// Gets or sets the choice list.
        /// </summary>
        /// <value>
        /// The choice list.
        /// </value>
        [DataMember]
        public string[] ChoiceList { get; set; }
        /// <summary>
        /// Called when deserialized.
        /// </summary>
        /// <param name="context">An object of the type <see cref="T:System.Runtime.Serialization.StreamingContext"/>.</param>
        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            if (this.DefaultValue == null || !(this.DefaultValue is JToken)) return;
            switch (this.DataType)
            {
                case esriDataType.GPRecordSet:
                case esriDataType.GPFeatureRecordSetLayer:
                    this.DefaultValue = (this.DefaultValue as JToken).ToObject<FeatureSet>();
                    break;
                case esriDataType.GPLinearUnit:
                    this.DefaultValue = (this.DefaultValue as JToken).ToObject<GPLinearUnit>();
                    break;
                case esriDataType.GPDataFile:
                case esriDataType.GPRasterData:
                case esriDataType.GPRasterDataLayer:
                    this.DefaultValue = (this.DefaultValue as JToken).ToObject<GPDataFile>();
                    break;
                case esriDataType.GPMultiValue:
                    this.DefaultValue = (this.DefaultValue as JToken).ToObject<object[]>();
                    break;
            }
        }
    }
}
