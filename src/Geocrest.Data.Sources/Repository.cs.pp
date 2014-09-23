namespace $rootnamespace$.Models
{
	using Geocrest.Data.Sources;
	/// <summary>
	/// Provides a generic data repository for retrieving Entity Framework data.
	/// </summary>
    public class Repository : DatabaseRepositoryBase<Context>
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="T:$rootnamespace$.Models.Repository" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public Repository(Context context)
            : base(context)
        {
        }
    }
}
