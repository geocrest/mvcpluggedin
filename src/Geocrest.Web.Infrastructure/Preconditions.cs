using System;

namespace Geocrest.Web.Infrastructure
{
    /// <summary>
    /// Provides precondition checks for constructor arguments that need to be passed
    /// to base class constructors
    /// </summary>
    public static class Preconditions
    {
        /// <summary>
        /// Checks the value for a null value.
        /// </summary>
        /// <typeparam name="T">The type of the input value.</typeparam>
        /// <param name="value">The value to check.</param>
        /// <param name="argumentname">The argumentname.</param>
        /// <returns>
        /// Returns an instance of <typeparamref name="T"/>.
        /// </returns>
        public static T CheckNotNull<T>(T value, string argumentname) where T: class
        {           
            Throw.IfArgumentNull(value, argumentname);            
            return value;
        }
        /// <summary>
        /// Checks the not null or empty.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="argumentname">The argumentname.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:System.String">System.String</see>.
        /// </returns>
        public static string CheckNotNullOrEmpty(string value, string argumentname)
        {
            Throw.IfArgumentNullOrEmpty(value, argumentname);
            return value;
        }
    }
}
