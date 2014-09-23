
#if NET40 || SILVERLIGHT
namespace Geocrest.Model.ArcGIS.Tasks
{
    using System.Runtime.Serialization;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Represents a geoprocessing result parameter.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class GPResult
    {
        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        [DataMember]
        public string ParamName { get; set; }

        /// <summary>
        /// Gets or sets the type of the data.
        /// </summary>
        /// <value>
        /// The type of the data.
        /// </value>
        [DataMember]
        public esriDataType DataType { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [DataMember]
        public dynamic Value { get; set; }

        /// <summary>
        /// Called when deserialized.
        /// </summary>
        /// <param name="context">An object of the type <see cref="T:System.Runtime.Serialization.StreamingContext"/>.</param>
        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            if (this.Value == null || !(this.Value is JToken)) return;
            GPMapValue image = null;
            switch (this.DataType)
            {
                case esriDataType.GPRecordSet:
                    this.Value = new GPRecordSet(this.ParamName, (this.Value as JToken).ToObject<FeatureSet>());
                    break;
                case esriDataType.GPFeatureRecordSetLayer:
                    // try image first                    
                    image = (this.Value as JToken).ToObject<GPMapValue>();
                    if (image.mapImage != null)
                    {
                        this.Value = image.mapImage;
                    }
                    else
                    {
                        this.Value = new GPFeatureRecordSetLayer(this.ParamName, (this.Value as JToken).ToObject<FeatureSet>());
                    }
                    break;
                case esriDataType.GPLinearUnit:
                    this.Value = (this.Value as JToken).ToObject<GPLinearUnit>();
                    this.Value.Name = this.ParamName;
                    break;
                case esriDataType.GPDataFile:
                case esriDataType.GPRasterData:
                    this.Value = (this.Value as JToken).ToObject<GPDataFile>();
                    this.Value.Name = this.ParamName;
                    break;
                case esriDataType.GPRasterDataLayer:
                    // try image first
                    image = (this.Value as JToken).ToObject<GPMapValue>();
                    if (image.mapImage != null)
                    {
                        this.Value = image.mapImage;
                    }
                    else
                    {
                        this.Value = (this.Value as JToken).ToObject<GPDataFile>();
                        this.Value.Name = this.ParamName;
                    }
                    break;
                case esriDataType.GPMultiValue:
                    this.Value = new GPMultiValue<GPString>(this.ParamName, (this.Value as JToken).ToObject<GPString[]>());
                    this.Value.Name = this.ParamName;
                    break;
            }
        }
    }
}
#endif
