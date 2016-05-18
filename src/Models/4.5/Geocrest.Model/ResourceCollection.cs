namespace Geocrest.Model
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// A collection of top-level <see cref="T:Geocrest.Model.Resource"/> objects.
    /// </summary>
    /// <typeparam name="T">A type of <see cref="T:Geocrest.Model.Resource"/>.</typeparam>    
    public class ResourceCollection<T> : Resource, 
#if REPRESENTATIONS
        IHalResourceCollection,
#endif
        ICollection<T> where T: Resource
    {
        /// <summary>
        /// A private list of the collection items.
        /// </summary>
        List<T> resources;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.ResourceCollection`1"/> class.
        /// </summary>
        public ResourceCollection()
        {
            this.resources = new List<T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.ResourceCollection`1"/> class.
        /// </summary>
        /// <param name="items">The items.</param>
        public ResourceCollection(IEnumerable<T> items)
        {
            this.resources = new List<T>(items);            
        }

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <value>
        /// An item of type <typeparamref name="T" />.
        /// </value>
        /// <param name="index">The zero-based index.</param>
        /// <returns>
        /// The object at the specified index.
        /// </returns>
        public T this[int index]
        {
            get { return this.resources[index]; }
        }
        
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this.resources.GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>
        /// Returns an instance of <see cref="T:System.Collections.IEnumerator"/>.
        /// </returns>
        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

#if REPRESENTATIONS
        /// <summary>
        /// Adds the specified resource to the collection.
        /// </summary>
        /// <param name="resource">The HAL resource.</param>
        /// <exception cref="T:System.NotSupportedException">
        /// The 'Add' method requires a type derived from Geocrest.Model.Resource.
        /// </exception>
        public void Add(IHalResource resource)
        {
            if (resource.GetType().IsSubclassOf(typeof(Resource)))
                this.resources.Add((T)resource);
            else
                throw new NotSupportedException("The 'Add' method requires a type derived from Geocrest.Model.Resource.");
        }
#endif

        #region ICollection<T> Members

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        public void Add(T item)
        {
            this.resources.Add(item);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public void Clear()
        {
            this.resources.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false.
        /// </returns>
        public bool Contains(T item)
        {
            return this.resources.Contains(item);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            this.resources.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
        [DataMember]
        public int Count
        {
            get { return this.resources.Count; }
            protected set { }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only; otherwise, false.</returns>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false. This method also returns false if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        public bool Remove(T item)
        {
            return this.resources.Remove(item);
        }

        #endregion
    }
}
