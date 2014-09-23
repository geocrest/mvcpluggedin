
namespace Geocrest.Web.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Represents a controller name with an associated version
    /// </summary>
    public struct ControllerIdentification : IEquatable<ControllerIdentification>
    {
        private static readonly Lazy<IEqualityComparer<ControllerIdentification>> ComparerInstance = 
            new Lazy<IEqualityComparer<ControllerIdentification>>(() => new ControllerNameComparer());

        /// <summary>
        /// Gets an comparer for comparing <see cref="T:Geocrest.Web.Mvc.Controllers.ControllerIdentification"/> instances.
        /// </summary>
        public static IEqualityComparer<ControllerIdentification> Comparer
        {
            get { return ComparerInstance.Value; }
        }

        /// <summary>
        /// Gets or sets the name of the controller
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the associated version
        /// </summary>
        public string Version { get; set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Controllers.ControllerIdentification"/> struct.
        /// </summary>
        /// <param name="name">The name of the controller.</param>
        /// <param name="version">The version.</param>
        public ControllerIdentification(string name, string version)
            : this()
        {
            this.Name = name;
            this.Version = version;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(ControllerIdentification other)
        {
            return StringComparer.InvariantCultureIgnoreCase.Equals(other.Name, this.Name) &&
                   other.Version == this.Version;
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object" /> to compare with this instance.</param>
        /// <returns>
        /// <b>true</b>, if the specified <see cref="T:System.Object" /> is equal to this instance; otherwise, <b>false</b>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is ControllerIdentification)
            {
                ControllerIdentification cn = (ControllerIdentification)obj;
                return this.Equals(cn);
            }

            return false;
        }
        /// <summary>
        /// Indicates whether two <see cref="T:Geocrest.Web.Mvc.Controllers.ControllerIdentification"/> 
        /// structures are equal.
        /// </summary>
        /// <param name="left">The <see cref="T:Geocrest.Web.Mvc.Controllers.ControllerIdentification"/>
        /// structure on the left side of the equality operator.</param>
        /// <param name="right">The <see cref="T:Geocrest.Web.Mvc.Controllers.ControllerIdentification"/>
        /// structure on the right side of the equality operator.</param>
        /// <returns><c>true</c>, if the structures are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(ControllerIdentification left, ControllerIdentification right)
        {
            if (Object.ReferenceEquals(left, null))
                return Object.ReferenceEquals(right, null);
            return left.Equals(right);
        }
        /// <summary>
        /// Indicates whether two <see cref="T:Geocrest.Web.Mvc.Controllers.ControllerIdentification"/> 
        /// structures are unequal.
        /// </summary>
        /// <param name="left">The <see cref="T:Geocrest.Web.Mvc.Controllers.ControllerIdentification"/>
        /// structure on the left side of the equality operator.</param>
        /// <param name="right">The <see cref="T:Geocrest.Web.Mvc.Controllers.ControllerIdentification"/>
        /// structure on the right side of the equality operator.</param>
        /// <returns><c>true</c>, if the structures are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(ControllerIdentification left, ControllerIdentification right)
        {
            if (Object.ReferenceEquals(left, null))
                return !Object.ReferenceEquals(right, null);
            return !left.Equals(right);
        }
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.ToString().ToUpperInvariant().GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="T:System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            if (this.Version == null)
            {
                return this.Name;
            }

            return VersionedControllerSelector.VersionPrefix + this.Version.ToString(CultureInfo.InvariantCulture) 
                + "." + this.Name;
        }

        /// <summary>
        /// Implementation of an equality comparer for controller identification.
        /// </summary>
        private class ControllerNameComparer : IEqualityComparer<ControllerIdentification>
        {
            /// <summary>
            /// Determines whether the specified objects are equal.
            /// </summary>
            /// <param name="x">The first object of type <paramref name="x" /> to compare.</param>
            /// <param name="y">The second object of type <paramref name="y" /> to compare.</param>
            /// <returns>
            /// true if the specified objects are equal; otherwise, false.
            /// </returns>
            public bool Equals(ControllerIdentification x, ControllerIdentification y)
            {
                return x.Equals(y);
            }

            /// <summary>
            /// Returns a hash code for this instance.
            /// </summary>
            /// <param name="obj">The obj.</param>
            /// <returns>
            /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
            /// </returns>
            public int GetHashCode(ControllerIdentification obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
