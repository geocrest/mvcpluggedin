namespace Geocrest.Model.ArcGIS.Tasks
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Geocrest.Model.ArcGIS.Geometry;
    using Geocrest.Model.ArcGIS.Schema;

    /// <summary>
    /// Represents a collection of features
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    [JsonObject]
    public class FeatureSet : RecordSet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.ArcGIS.Tasks.FeatureSet"/> class.
        /// </summary>
        public FeatureSet() 
        { 
            this.Features = new Feature[]{};
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.ArcGIS.Tasks.FeatureSet"/> class.
        /// </summary>
        /// <param name="geometry">A single geometry object.</param>
        public FeatureSet(Geometry geometry)
        {
            List<Feature> features = new List<Feature>();
            if (geometry != null)
            {
                features.Add(new Feature(){ Geometry = geometry,Attributes= new Dictionary<string,object>()});
                this.SpatialReference = geometry.SpatialReference;
                this.GeometryType = geometry.GeometryType;
            }
            this.Features = features.ToArray();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.ArcGIS.Tasks.FeatureSet" /> class.
        /// </summary>
        /// <param name="geometries">The geometries to add to the set.</param>
        /// <exception cref="T:System.ArgumentNullException">geometries parameter must not be null and must contain at least one geometry.</exception>
        /// <exception cref="T:System.ArgumentException">
        /// geometries contain multiple spatial references.
        /// or
        /// geometries contain multiple geometry types.
        /// </exception>
        public FeatureSet(IEnumerable<Geometry> geometries)
        {
            if (geometries == null || geometries.Count() == 0)
                throw new ArgumentNullException("geometries", "geometries parameter must not be null and must contain at least one geometry.");
            if (geometries.Select(x => x.SpatialReference).Distinct().Count() > 1)
                throw new ArgumentException("geometries contain multiple spatial references.");
            if (geometries.Select(x => x.GeometryType).Distinct().Count() > 1)
                throw new ArgumentException("geometries contain multiple geometry types.");
            this.Features = new List<Feature>(geometries.Select(x => new Feature
            {
                Geometry = x,
                Attributes = new Dictionary<string, object>()
            })).ToArray();
            this.SpatialReference = geometries.First().SpatialReference;
            this.GeometryType = geometries.First().GeometryType;
        }
        /// <summary>
        /// Gets or sets the spatial reference.
        /// </summary>
        /// <value>
        /// The spatial reference.
        /// </value>
        [DataMember(Name="spatialReference")]
        public SpatialReference SpatialReference { get; set; }

        /// <summary>
        /// Gets or sets the type of the geometry.
        /// </summary>
        /// <value>
        /// The type of the geometry.
        /// </value>
        [DataMember(Name="geometryType")]
        public esriGeometryType GeometryType { get; set; }

        /// <summary>
        /// Gets or sets the features.
        /// </summary>
        /// <value>
        /// The features.
        /// </value>
        [DataMember(Name="features")]
        public Feature[] Features { get; set; }

        /// <summary>
        /// Gets or sets the field to use for the display name.
        /// </summary>
        /// <value>
        /// The field to use for the display name.
        /// </value>
        [DataMember(Name="displayFieldName")]
        public string DisplayFieldName { get; set; }

        /// <summary>
        /// Gets or sets the field aliases.
        /// </summary>
        /// <value>
        /// The field aliases.
        /// </value>
        [DataMember(Name="fieldAliases")]
        public IDictionary<string, string> FieldAliases { get; set; }

        /// <summary>
        /// Gets or sets the fields.
        /// </summary>
        /// <value>
        /// The fields.
        /// </value>
        [DataMember(Name="fields")]
        public Field[] Fields { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this featureset has exceeded the transfer limit.
        /// </summary>
        /// <value>
        /// <b>true</b>, if exceeded transfer limit; otherwise, <b>false</b>.
        /// </value>
        [DataMember(Name="exceededTransferLimit")]
        public bool ExceededTransferLimit { get; set; }

        /// <summary>
        /// Called when deserialized.
        /// </summary>
        /// <param name="context">An object of the type <see cref="T:System.Runtime.Serialization.StreamingContext"/>.</param>
        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            if (Features != null && SpatialReference != null)
            {
                foreach (Feature feat in Features.Where(x => x.Geometry != null))
                    feat.Geometry.SpatialReference = SpatialReference;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        public override IEnumerator GetEnumerator()
        {
            if (Features != null)
            {
                foreach (Feature f in Features)
                    yield return f;
            }
        }
    }
}