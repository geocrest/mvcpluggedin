namespace Geocrest.Model.ArcGIS.Tasks
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using System.Linq;
    /// <summary>
    /// The collection of results associated with a map identify operation.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    [JsonObject]
    public class IdentifyResultCollection : IEnumerable<IdentifyResult>
    {        
        /// <summary>
        /// Gets or sets the results.
        /// </summary>
        /// <value>
        /// The results.
        /// </value>
        [DataMember]
        public IdentifyResult[] Results {get;set;}

        #region Indexers
        /// <summary>
        /// Gets the <see cref="T:Geocrest.Model.ArcGIS.Tasks.IdentifyResult"/> with the specified 
        /// map service layer ID.
        /// </summary>
        /// <value>
        /// The <see cref="T:Geocrest.Model.ArcGIS.Tasks.IdentifyResult"/> with the specified
        /// map service layer ID.
        /// </value>
        /// <param name="layerId">The map service layer ID of the element to return.</param>
        /// <remarks>Note that the input parameter is not the index of an element within the collection but
        /// rather the ID of the map service layer.</remarks>
        /// <returns></returns>
        public IdentifyResult this[int layerId]
        {
            get { return this.Results.SingleOrDefault(x => x.LayerID == layerId); }
        }
        /// <summary>
        /// Gets the <see cref="T:Geocrest.Model.ArcGIS.Tasks.IdentifyResult"/> with the specified layer name.
        /// </summary>
        /// <value>
        /// The <see cref="T:Geocrest.Model.ArcGIS.Tasks.IdentifyResult"/> with the specified name.
        /// </value>
        /// <param name="layerName">The map service layer name of the element to return.</param>
        /// <returns></returns>
        public IdentifyResult this[string layerName]
        {
            get { return this.Results.SingleOrDefault(x => x.LayerName == layerName); }
        }
        #endregion

        #region IEnumerable<IdentifyResult> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<IdentifyResult> GetEnumerator()
        {
            if (this.Results != null)
            {
                foreach (IdentifyResult l in this.Results)
                    yield return l;
            }
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
