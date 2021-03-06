﻿namespace Geocrest.Data.Sources
{
    using Geocrest.Data.Contracts;
    using Geocrest.Web.Infrastructure;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Provides a base class for all repositories that retrieve entity information directly from a database.
    /// </summary>
    /// <typeparam name="C">The <see cref="T:Geocrest.Data.Sources.Database"/> used to retrieve data.</typeparam>
    public abstract class DatabaseRepositoryBase<C> : IRepository
        where C : Database
    {        
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Data.Sources.DatabaseRepositoryBase`1" /> class.
        /// </summary>
        /// <param name="context">The <see cref="T:Geocrest.Data.Sources.Database"/> context.</param>
        protected DatabaseRepositoryBase(C context)
        {
            Throw.IfArgumentNull(context, "context");
            this.Context = context;
            this.options = (context as IObjectContextAdapter).ObjectContext.ContextOptions;
        }
        #endregion

        #region Fields
        private ObjectContextOptions options;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the database context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        protected C Context { get; set; }
        
        /// <summary>
        /// Gets the schema object associated with this repository.
        /// </summary>
        public string Schema
        {
            get { return (this.Context as IDbSchema).Schema; }
        }
        /// <summary>
        /// Gets or sets a Boolean value that determines whether related objects are
        /// loaded automatically when a navigation property is accessed.
        /// </summary>
        /// <value>
        /// A System.Boolean value that is true when lazy loading is enabled; otherwise false.
        /// </value>
        public bool LazyLoadingEnabled
        {
            get { return this.options.LazyLoadingEnabled; }
            set { this.options.LazyLoadingEnabled = value; }
        }

        /// <summary>
        /// Gets or sets a Boolean value that determines whether proxy instances are
        /// created for custom data classes that are persistence ignorant.
        /// </summary>
        /// <value>
        /// A System.Boolean value that is true when proxies are created; otherwise false.
        /// The default value is true.
        /// </value>
        public bool ProxyCreationEnabled
        {
            get { return this.options.ProxyCreationEnabled; }
            set { this.options.ProxyCreationEnabled = value; }
        }
        #endregion

        #region Methods       
        /// <summary>
        /// Changes the server and database of the underlying data connection. All other 
        /// connection properties should remain the same.
        /// </summary>
        /// <param name="serverName">Name of the new server.</param>
        /// <param name="databaseName">Name of the new database.</param>
        public virtual void ChangeConnection(string serverName, string databaseName)
        {
            string datasource = this.Context.Database.Connection.DataSource;
            string database = this.Context.Database.Connection.Database;
            string connection = this.Context.Database.Connection.ConnectionString;
            if (this.Context.Database.Connection.State != System.Data.ConnectionState.Closed)
                this.Context.Database.Connection.Close();
            string newconnection = connection.Replace(datasource, serverName).Replace(database, databaseName);
            this.Context.Database.Connection.ConnectionString = newconnection;
        }
        /// <summary>
        /// Gets all entities of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of entity to retrieve.</typeparam>
        /// <returns>An instance of <see cref="T:System.Linq.IQueryable`1"/></returns>
        public virtual IQueryable<T> All<T>() where T : class
        {
            return this.Context.Set<T>(); 
        }

        /// <summary>
        /// Gets all entities of the specified type with the included properties populated.
        /// </summary>
        /// <typeparam name="T">The type of entity to retrieve.</typeparam>
        /// <param name="includeProperties">The included properties.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:System.Linq.IQueryable`1"/>.
        /// </returns>
        public virtual IQueryable<T> AllIncluding<T>(params Expression<Func<T, object>>[] includeProperties)
            where T : class
        {
            IQueryable<T> query = this.Context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }
        
        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <typeparam name="T">The type of entity to delete.</typeparam>
        /// <param name="id">The id.</param>
        public virtual void Delete<T>(int id) where T : class
        {
            var entity = this.Find<T>(id);
            this.Context.Set<T>().Remove(entity);
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <typeparam name="T">The type of entity to delete.</typeparam>
        /// <param name="id">The id.</param>
        public virtual void Delete<T>(string id) where T : class
        {
            var entity = this.Find<T>(id);
            this.Context.Set<T>().Remove(entity);
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <typeparam name="T">The type of entity to delete.</typeparam>
        /// <param name="id">The id.</param>
        public virtual void Delete<T>(Guid id) where T : class
        {
            var entity = this.Find<T>(id);
            this.Context.Set<T>().Remove(entity);
        }

        /// <summary>
        /// Detaches the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Detach(object entity)
        {
            ((IObjectContextAdapter)this.Context).ObjectContext.Detach(entity);
        }

        /// <summary>
        /// Finds the specified entity by id.
        /// </summary>
        /// <typeparam name="T">The type of entity to retrieve.</typeparam>
        /// <param name="id">The id as an integer.</param>
        /// <returns>An instance of <typeparamref name="T"/></returns>
        public virtual T Find<T>(int id) where T : class
        {
            return this.Context.Set<T>().Find(id);
        }

        /// <summary>
        /// Finds the specified entity by id.
        /// </summary>
        /// <typeparam name="T">The type of entity to retrieve.</typeparam>
        /// <param name="id">The id as a string.</param>
        /// <returns>An instance of <typeparamref name="T"/></returns>
        public virtual T Find<T>(string id) where T : class
        {
            // try to parse as GUID and int since this method serves many sub-classes
            Guid parsedGuid;
            int parsedInt;
            if (Guid.TryParse(id, out parsedGuid))
            {
                return this.Find<T>(parsedGuid);
            }
            else if (int.TryParse(id, out parsedInt))
            {
                return this.Find<T>(parsedInt);
            }
            else
            {
                return this.Context.Set<T>().Find(id);
            }
        }

        /// <summary>
        /// Finds the specified entity by id.
        /// </summary>
        /// <typeparam name="T">The type of entity to retrieve.</typeparam>
        /// <param name="id">The id as a guid.</param>
        /// <returns>An instance of <typeparamref name="T"/></returns>
        public virtual T Find<T>(Guid id) where T : class
        {
            return this.Context.Set<T>().Find(id);
        }

        /// <summary>
        /// Finds an entity by the specified predicate.
        /// </summary>
        /// <typeparam name="T">The type of entity to retrieve.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:System.Linq.IQueryable`1"/>.
        /// </returns>
        public virtual IQueryable<T> FindBy<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            IQueryable<T> query = this.Context.Set<T>().Where(predicate);
            return query;
        }

        /// <summary>
        /// Converts the properties of the specified type to a comma-delimited string.
        /// </summary>
        /// <typeparam name="T">The type whose properties are to be included.</typeparam>
        /// <returns>
        /// A comma-delimited string of database fields that correspond to the properties of the class.
        /// </returns>
        public string GetSqlFields<T>() where T : class
        {
            string sql = "* ";
            Type type = typeof(T);
            var props = (from p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                         where (p.GetCustomAttributes(false).Cast<Attribute>()
                         .Count(x => x.GetType().Name == "NotMappedAttribute") == 0 &&
                         p.GetGetMethod().IsVirtual != true) || p.GetCustomAttributes(false).Cast<Attribute>()
                         .Count(x => x.GetType().Name == "MappedAttribute") > 0
                         select p);
            if (props.Count() > 0)
            {
                sql = "";
                foreach (var prop in props)
                {
                    if (prop.GetCustomAttributes(typeof(ColumnAttribute), false).Length > 0 &&
                        !string.IsNullOrEmpty((prop.GetCustomAttributes(typeof(ColumnAttribute), false)[0]
                        as ColumnAttribute).Name))
                        sql += string.Format("[{0}] as {1},",
                            (prop.GetCustomAttributes(typeof(ColumnAttribute), false)[0]
                            as ColumnAttribute).Name, prop.Name);
                    else
                        sql += string.Format("[{0}],", prop.Name);
                }
            }

            return sql.Substring(0, sql.Length - 1);
        }

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
        public string GetSqlFields<T>(string sqlAggregateFunctionName) where T : class
        {
            sqlAggregateFunctionName = string.IsNullOrEmpty(sqlAggregateFunctionName) ? "max" : sqlAggregateFunctionName;
            string sql = "* ";
            Type type = typeof(T);
            var props = (from p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                         where (p.GetCustomAttributes(false).Cast<Attribute>()
                         .Count(x => x.GetType().Name == "NotMappedAttribute") == 0 &&
                         p.GetGetMethod().IsVirtual != true) || p.GetCustomAttributes(false).Cast<Attribute>()
                         .Count(x => x.GetType().Name == "MappedAttribute") > 0
                         select p);
            if (props.Count() > 0)
            {
                sql = "";
                foreach (var prop in props)
                {
                    if (prop.GetCustomAttributes(typeof(ColumnAttribute), false).Length > 0 &&
                        !string.IsNullOrEmpty((prop.GetCustomAttributes(typeof(ColumnAttribute), false)[0]
                        as ColumnAttribute).Name))
                        sql += string.Format(sqlAggregateFunctionName + "([{0}]) as {1},",
                            (prop.GetCustomAttributes(typeof(ColumnAttribute), false)[0]
                            as ColumnAttribute).Name, prop.Name);
                    else
                        sql += string.Format(sqlAggregateFunctionName + "([{0}]) as {0},", prop.Name);
                }
            }

            return sql.Substring(0, sql.Length - 1);
        }

        /// <summary>
        /// Inserts a new entity.
        /// </summary>
        /// <typeparam name="T">The type of entity to insert.</typeparam>
        /// <param name="entity">The entity.</param>
        public virtual void Insert<T>(T entity) where T : class
        {
            this.Context.Set<T>().Add(entity);
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <typeparam name="T">The type of entity to update.</typeparam>
        /// <param name="entity">The entity.</param>
        public virtual void Update<T>(T entity) where T : class
        {
            this.Context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public virtual void Save()
        {
            this.Context.SaveChanges();
        }

        /// <summary>
        /// Uses a raw SQL query to return entities in this set.
        /// </summary>
        /// <typeparam name="T">The type of the entity to return.</typeparam>
        /// <param name="sql">A SQL statement to use in the call to the database.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:System.Linq.IQueryable`1"/>
        /// </returns>
        public IQueryable<T> SqlQuery<T>(string sql) where T : class
        {
            return this.Context.Set<T>().SqlQuery(sql).AsQueryable<T>();
        }
        #endregion
    }
}
