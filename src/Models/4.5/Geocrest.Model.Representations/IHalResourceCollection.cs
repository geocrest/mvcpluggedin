
namespace Geocrest.Model
{
    using System.Collections;
    /// <summary>
    /// Provides a method for adding <see cref="T:Geocrest.Model.IHalResource"/>
    /// objects to a collection.
    /// </summary>
    public interface IHalResourceCollection : IHalResource, IEnumerable
    {
        /// <summary>
        /// Adds the specified resource to a collection.
        /// </summary>
        /// <param name="resource">The resource.</param>
        void Add(IHalResource resource);
    }
}
