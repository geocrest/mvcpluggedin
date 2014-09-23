
namespace Geocrest.Data.Contracts
{

    /// <summary>
    /// Returns the database schema that a connection should use.
    /// </summary>
    public interface IDbSchema
    {
        /// <summary>
        /// Gets the schema that is used in the database.
        /// </summary>
        string Schema { get; }
    }
}
