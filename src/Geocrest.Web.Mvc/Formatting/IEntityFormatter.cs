namespace Geocrest.Web.Mvc.Formatting
{
    using System;
    /// <summary>
    /// Provides a method for performing specific formatting operations on entities.
    /// </summary>
    public interface IEntityFormatter
    {
        /// <summary>
        /// Formats the specified entity with custom logic.
        /// </summary>
        /// <param name="entity">The entity to format.</param>
        /// <returns>The formatted entity.</returns>
        /// <exception cref="T:System.ArgumentNullException">entity</exception>
        object Format(object entity);
    }
}
