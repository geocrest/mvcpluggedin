namespace Geocrest.Model.ArcGIS
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Provides basic information regarding a single layer or table in a map service 
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class LayerTableInfo : LayerTableBase
    {        
         /// <summary>
         /// Gets or sets a value indicating this layer's default visibility (layers only).
         /// </summary>
         /// <value>
         /// <b>true</b>, if visible by default; otherwise, <b>false</b>.
         /// </value>
         [DataMember]
         public bool DefaultVisibility { get; set; }

         /// <summary>
         /// Gets or sets the parent layer ID containing the layer (layers only).
         /// </summary>
         /// <value>
         /// The parent layer ID.
         /// </value>
         [DataMember]
         public int ParentLayerID { get; set; }

         /// <summary>
         /// Gets or sets the sublayerIds contained within this layer (layers only).
         /// </summary>
         /// <value>
         /// The sublayerIds.
         /// </value>
         [DataMember]
         public int[] SubLayerIDs { get; set; }

         /// <summary>
         /// Gets or sets the minimum scale at which this layer is visible (layers only).
         /// </summary>
         /// <value>
         /// The minimum scale.
         /// </value>
         [DataMember]
         public double MinScale { get; set; }

         /// <summary>
         /// Gets or sets the maximum scale at which this layer is visible (layers only).
         /// </summary>
         /// <value>
         /// The maximum scale.
         /// </value>
         [DataMember]
         public double MaxScale { get; set; }
    }
}
