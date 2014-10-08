namespace Geocrest.Model.ArcGIS.Geometry
{
    using System;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// Base class for all geometries
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class Geometry
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.ArcGIS.Geometry.Geometry"/> class.
        /// </summary>
        public Geometry() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.ArcGIS.Geometry.Geometry"/> class.
        /// </summary>
        /// <param name="x">The X value.</param>
        /// <param name="y">The Y value.</param>
        /// <param name="sr">A well-known spatial reference ID.</param>
        public Geometry(double x, double y, WKID sr)
        {
            X = x;
            Y = y;
            SpatialReference = new SpatialReference(sr);
            GeometryType =  esriGeometryType.esriGeometryPoint;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.ArcGIS.Geometry.Geometry"/> class.
        /// </summary>
        /// <param name="xmin">The XMin value.</param>
        /// <param name="ymin">The YMin value.</param>
        /// <param name="xmax">The XMax value.</param>
        /// <param name="ymax">The YMax value.</param>
        /// <param name="sr">The <see cref="T:Geocrest.Model.ArcGIS.Geometry.SpatialReference"/> 
        /// of the geometry.</param>
        public Geometry(double xmin, double ymin, double xmax, double ymax, SpatialReference sr)
        {
            XMin = xmin;
            YMin = ymin;
            XMax = xmax;
            YMax = ymax;
            SpatialReference = sr;
            GeometryType = esriGeometryType.esriGeometryEnvelope;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the geometry type.
        /// </summary>
        /// <value>
        /// The type of the geometry.
        /// </value>
        [DataMember(Name = "geometryType")]
        public esriGeometryType GeometryType { get; set; }

        /// <summary>
        /// Gets or sets the paths.
        /// </summary>
        /// <value>
        /// The paths.
        /// </value>
        [DataMember(Name = "paths", EmitDefaultValue = false)]
        public double[][][] Paths { get; set; }

        /// <summary>
        /// Gets or sets the rings for polygons.
        /// </summary>
        /// <value>
        /// The rings.
        /// </value>
        [DataMember(Name = "rings", EmitDefaultValue = false)]
        public double[][][] Rings { get; set; }

        /// <summary>
        /// Gets or sets the spatial reference.
        /// </summary>
        /// <value>
        /// The spatial reference.
        /// </value>
        [DataMember(Name = "spatialReference")]
        public SpatialReference SpatialReference { get; set; }

        /// <summary>
        /// Gets or sets the X coordinate value.
        /// </summary>
        /// <value>
        /// The X value.
        /// </value>
        [DataMember(Name = "x", EmitDefaultValue = false)]
        public double? X { get; set; }

        /// <summary>
        /// Gets or sets the maximum X value.
        /// </summary>
        /// <value>
        /// The X max.
        /// </value>
        [DataMember(Name = "xmax", EmitDefaultValue = false)]
        public double? XMax { get; set; }

        /// <summary>
        /// Gets or sets the minumum X value.
        /// </summary>
        /// <value>
        /// The X min.
        /// </value>
        [DataMember(Name = "xmin", EmitDefaultValue = false)]
        public double? XMin { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate value.
        /// </summary>
        /// <value>
        /// The Y value.
        /// </value>
        [DataMember(Name = "y", EmitDefaultValue = false)]
        public double? Y { get; set; }

        /// <summary>
        /// Gets or sets the maximum Y value.
        /// </summary>
        /// <value>
        /// The Y max.
        /// </value>
        [DataMember(Name = "ymax", EmitDefaultValue = false)]
        public double? YMax { get; set; }

        /// <summary>
        /// Gets or sets the minimum Y value.
        /// </summary>
        /// <value>
        /// The Y min.
        /// </value>
        [DataMember(Name = "ymin", EmitDefaultValue = false)]
        public double? YMin { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Determines whether the specified property name exists.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        /// <b>true</b>, if the specified property name exists; otherwise, <b>false</b>.
        /// </returns>
        public bool HasProperty(string propertyName)
        {
            var type = this.GetType();
            return type.GetProperty(propertyName) != null;
        }

        /// <summary>
        /// Called when deserialized.
        /// </summary>
        /// <param name="context">An object of the type <see cref="T:System.Runtime.Serialization.StreamingContext"/>.</param>
        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            if (Rings != null)
                GeometryType = esriGeometryType.esriGeometryPolygon;
            else if (Paths != null)
                GeometryType = esriGeometryType.esriGeometryPolyline;
            else if (X.HasValue && Y.HasValue)
                GeometryType = esriGeometryType.esriGeometryPoint;
            else if (XMin.HasValue && YMin.HasValue && XMax.HasValue && YMax.HasValue)
                GeometryType = esriGeometryType.esriGeometryEnvelope;
        }

        /// <summary>
        /// Returns a string that represents this instance as a JSON object.
        /// </summary>
        /// <returns>
        /// A JSON string representing this instance.
        /// </returns>
        public override string ToString()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None,
                DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore,
                ContractResolver = new OptionalPropertiesContractResolver("geometryType")
            };
            return JsonConvert.SerializeObject(this, settings);
        }
        #endregion
    }
}
