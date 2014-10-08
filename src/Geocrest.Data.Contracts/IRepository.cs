namespace Geocrest.Data.Contracts
{
    using System;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Provides generic methods for interacting with an underlying data source
    /// </summary>
    public interface IRepository
    {
        #region Properties
        /// <summary>
        /// Gets the options associated with the underlying context.
        /// </summary>
        ObjectContextOptions Options { get; }       

        /// <summary>
        /// Gets the schema associated with this repository.
        /// </summary>
        string Schema { get; }
        #endregion

        #region Methods        
        /// <summary>
        /// Changes the server and database of the underlying data connection. All other 
        /// connection properties should remain the same.
        /// </summary>
        /// <param name="serverName">Name of the new server.</param>
        /// <param name="databaseName">Name of the new database.</param>
        void ChangeConnection(string serverName, string databaseName);

        /// <summary>
        /// Gets all entities of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of entity to retrieve.</typeparam>
        /// <returns>An instance of <see cref="T:System.Linq.IQueryable`1"/></returns>
        IQueryable<T> All<T>() where T : class;

        /// <summary>
        /// Gets all entities of the specified type with the included properties populated.
        /// </summary>
        /// <typeparam name="T">The type of entity to retrieve.</typeparam>
        /// <param name="includeProperties">The included properties.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:System.Linq.IQueryable`1"/>.
        /// </returns>
        IQueryable<T> AllIncluding<T>(params Expression<Func<T, object>>[] includeProperties) where T : class;

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <typeparam name="T">The type of entity to delete.</typeparam>
        /// <param name="id">The id.</param>
        void Delete<T>(int id) where T : class;

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <typeparam name="T">The type of entity to delete.</typeparam>
        /// <param name="id">The id.</param>
        void Delete<T>(string id) where T : class;

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <typeparam name="T">The type of entity to delete.</typeparam>
        /// <param name="id">The id.</param>
        void Delete<T>(Guid id) where T : class;

        /// <summary>
        /// Detaches the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Detach(object entity);

        /// <summary>
        /// Finds the specified entity by id.
        /// </summary>
        /// <typeparam name="T">The type of entity to retrieve.</typeparam>
        /// <param name="id">The id as an integer.</param>
        /// <returns>An instance of <typeparamref name="T"/></returns>
        T Find<T>(int id) where T : class;

        /// <summary>
        /// Finds the specified entity by id.
        /// </summary>
        /// <typeparam name="T">The type of entity to retrieve.</typeparam>
        /// <param name="id">The id as a string.</param>
        /// <returns>An instance of <typeparamref name="T"/></returns>
        T Find<T>(string id) where T : class;

        /// <summary>
        /// Finds the specified entity by id.
        /// </summary>
        /// <typeparam name="T">The type of entity to retrieve.</typeparam>
        /// <param name="id">The id as a guid.</param>
        /// <returns>An instance of <typeparamref name="T"/></returns>
        T Find<T>(Guid id) where T : class;

        /// <summary>
        /// Finds an entity by the specified predicate.
        /// </summary>
        /// <typeparam name="T">The type of entity to retrieve.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:System.Linq.IQueryable`1"/>.
        /// </returns>
        IQueryable<T> FindBy<T>(Expression<Func<T, bool>> predicate) where T : class;

        /// <summary>
        /// Converts the properties of the specified type to a comma-delimited string.
        /// </summary>
        /// <typeparam name="T">The type whose properties are to be included.</typeparam>
        /// <returns>
        /// A comma-delimited string of database fields that correspond to the properties of the class.
        /// </returns>
        string GetSqlFields<T>() where T : class;
       
        /// <summary>
        /// Converts the properties of the specified type to a comma-delimited string where each field is
        /// contained in a SQL aggregate function.
        /// </summary>
        /// <typeparam name="T">The type whose properties are to be included.</typeparam>
        /// <param name="sqlAggregateFunctionName">The name of the SQL aggregate function. The default is <c>MAX</c>.</param>
        /// <returns>
        /// A comma-delimited string of database fields enclosed in the supplied SQL aggregate function. For example:
        /// <c>max([Field1]) as Field1, max([Field2]) as Field2</c>.
        /// </returns>
        string GetSqlFields<T>(string sqlAggregateFunctionName) where T : class;
        
        /// <summary>
        /// Inserts a new entity.
        /// </summary>
        /// <typeparam name="T">The type of entity to insert.</typeparam>
        /// <param name="entity">The entity.</param>
        void Insert<T>(T entity) where T : class;

        /// <summary>
        /// Saves this instance.
        /// </summary>
        void Save();

        /// <summary>
        /// Uses a raw SQL query to return entities in this set.
        /// </summary>
        /// <typeparam name="T">The type of the entity to return.</typeparam>
        /// <param name="query">A SQL statement to use in the call to the database.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:System.Linq.IQueryable`1"/>
        /// </returns>
        IQueryable<T> SqlQuery<T>(string query) where T : class;

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <typeparam name="T">The type of entity to update.</typeparam>
        /// <param name="entity">The entity.</param>
        void Update<T>(T entity) where T : class;
        #endregion
    }
}
