namespace $rootnamespace$.Models
{
	using Geocrest.Data.Sources;
	using Geocrest.Web.Infrastructure;
	/// <summary>
	/// Represents the Entity Framework data context for which this application's models are mapped.
	/// </summary>
	public class Context : Database
	{
		private static string schema = "dbo";
		/// <summary>
		/// Initializes a new instance of the <see cref="T:$rootnamespace$.Models.Context" /> class.
		/// </summary>
		/// <param name="connectionString">The connection string for the database.</param>
		/// <param name="schema">The schema that is used in the database.</param>
		/// <remarks>
		/// This constructor is provided for run-time.
		/// </remarks>
		public Context(string connectionString, string schema) :
			base(Preconditions.CheckNotNullOrEmpty(connectionString, "connectionString"), schema)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="T:$rootnamespace$.Models.Context" /> class.
		/// </summary>
		/// <remarks>
		/// This constructor is provided for code-first migrations.
		/// </remarks>
		public Context()
			: base(schema)
		{
		}
		/// <summary>
        /// This method is called when the model for a derived context has been initialized, but
        /// before the model has been locked down and used to initialize the context.  The default
        /// implementation of this method does nothing, but it can be overridden in a derived class
        /// such that the model can be further configured before it is locked down.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        /// <remarks>
        /// Typically, this method is called only once when the first instance of a derived context
        /// is created.  The model for that context is then cached and is for all further instances of
        /// the context in the app domain.  This caching can be disabled by setting the ModelCaching
        /// property on the given ModelBuidler, but note that this can seriously degrade performance.
        /// More control over caching is provided through use of the DbModelBuilder and DbContextFactory
        /// classes directly.
        /// </remarks>
        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            // The following demonstrates how to map a class to a database table
			// For more information on the Fluent API see http://msdn.microsoft.com/en-US/data/jj591617
			// modelBuilder.Entity<SampleModel>().ToTable("SampleModels", this.Schema);
        }
	}
}
