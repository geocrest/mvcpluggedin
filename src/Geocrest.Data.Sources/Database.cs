namespace Geocrest.Data.Sources
{
    using System.Data.Entity;
    using Geocrest.Data.Contracts;
    using Geocrest.Web.Infrastructure;
    /// <summary>
    /// Handles retrieval of database objects using Entity Framework.
    /// </summary>  
    public class Database : DbContext,IDbSchema
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Geocrest.Data.Sources.Database" /> class.
        /// </summary>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <param name="schema">The schema that is used in the database.</param>
        /// <remarks>
        /// This constructor is provided for run-time.
        /// </remarks>
        public Database(string connectionString, string schema)
            : base(Preconditions.CheckNotNullOrEmpty(connectionString, "connectionString"))
        {
            Throw.IfArgumentNullOrEmpty(schema, "schema");
            this.Schema = schema;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Data.Sources.Database" /> class.
        /// </summary>
        /// <param name="schema">The schema that is used in the database.</param>
        /// <remarks>
        /// This constructor is provided for migrations.
        /// </remarks>
        public Database(string schema) 
        {
            Throw.IfArgumentNullOrEmpty(schema, "schema");
            this.Schema = schema;
        }

        /// <summary>
        /// Gets the schema that is used in the database.
        /// </summary>
        public string Schema
        {
            get;
            private set;
        }   
    }
}
