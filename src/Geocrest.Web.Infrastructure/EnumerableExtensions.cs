namespace Geocrest.Web.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Provides extension methods for the <see cref="T:System.Collections.Generic.IEnumerable`1" /> type.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Performs the given <see cref="T:System.Action`1" /> on each item in the enumerable.
        /// </summary>
        /// <typeparam name="T">The type of item in the enumerable.</typeparam>
        /// <param name="items">The enumerable of items.</param>
        /// <param name="action">The action to perform on each item.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// items
        /// or
        /// action
        /// </exception>
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            Throw.IfArgumentNull(items, "items");
            Throw.IfArgumentNull(action, "action");
            
            foreach (T item in items)
                action(item);
        }
        /// <summary>
        /// Adds the value only if it is not null.
        /// </summary>
        /// <typeparam name="T">The type of item to add.</typeparam>
        /// <param name="collection">An object of the type <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <param name="value">An object of the type <typeparamref name="T"/>.</param>
        public static void AddIfNotNull<T>(this ICollection<T> collection, T value)
        {
            if (collection != null && value != null && !string.IsNullOrEmpty(value.ToString()))
            {
                collection.Add(value);
            }
        }
        /// <summary>
        /// Sorts the elements of a sequence in ascending order according to a key.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="property">The property key used to sort the sequence.</param>
        /// <returns>An <see cref="T:System.Linq.IOrderedEnumerable`1"/> whose elements are sorted according to <paramref name="property"/>.</returns>
        public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> source, string property)
        {
            return ApplyOrder<T,IOrderedEnumerable<T>>(source, property, "OrderBy");
        }
        /// <summary>
        /// Sorts the elements of a sequence in descending order according to a key.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="property">The property key used to sort the sequence.</param>
        /// <returns>An <see cref="T:System.Linq.IOrderedEnumerable`1"/> whose elements are sorted according to <paramref name="property"/>.</returns>
        public static IOrderedEnumerable<T> OrderByDescending<T>(this IEnumerable<T> source, string property)
        {
            return ApplyOrder<T, IOrderedEnumerable<T>>(source, property, "OrderByDescending");
        }
        /// <summary>
        /// Sorts the elements of a sequence in ascending order according to a key.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="property">The property key used to sort the sequence.</param>
        /// <returns>An <see cref="T:System.Linq.IOrderedEnumerable`1"/> whose elements are sorted according to <paramref name="property"/>.</returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder<T, IOrderedQueryable<T>>(source, property, "OrderBy");
        }
        /// <summary>
        /// Sorts the elements of a sequence in descending order according to a key.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="property">The property key used to sort the sequence.</param>
        /// <returns>An <see cref="T:System.Linq.IOrderedEnumerable`1"/> whose elements are sorted according to <paramref name="property"/>.</returns>
        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder<T, IOrderedQueryable<T>>(source, property, "OrderByDescending");
        }
        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in ascending order.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="property">The property key used to sort the sequence.</param>
        /// <returns>An <see cref="T:System.Linq.IOrderedEnumerable`1"/> whose elements are sorted according to <paramref name="property"/>.</returns>
        public static IOrderedEnumerable<T> ThenBy<T>(this IOrderedEnumerable<T> source, string property)
        {
            return ApplyOrder<T, IOrderedEnumerable<T>>(source, property, "ThenBy");
        }
        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in descending order.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="property">The property key used to sort the sequence.</param>
        /// <returns>An <see cref="T:System.Linq.IOrderedEnumerable`1"/> whose elements are sorted according to <paramref name="property"/>.</returns>
        public static IOrderedEnumerable<T> ThenByDescending<T>(this IOrderedEnumerable<T> source, string property)
        {
            return ApplyOrder<T, IOrderedEnumerable<T>>(source, property, "ThenByDescending");
        }
        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in ascending order.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="property">The property key used to sort the sequence.</param>
        /// <returns>An <see cref="T:System.Linq.IOrderedQueryable`1"/> whose elements are sorted according to <paramref name="property"/>.</returns>
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder<T, IOrderedQueryable<T>>(source, property, "ThenBy");
        }
        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in descending order.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="property">The property key used to sort the sequence.</param>
        /// <returns>An <see cref="T:System.Linq.IOrderedQueryable`1"/> whose elements are sorted according to <paramref name="property"/>.</returns>
        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder<T, IOrderedQueryable<T>>(source, property, "ThenByDescending");
        }
        static TResult ApplyOrder<T, TResult>(IEnumerable<T> source, string property, string methodName) where TResult : IEnumerable<T>
        {
            string[] props = property.Split('.');
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (string prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                PropertyInfo pi = type.GetProperty(prop);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);
            Type generic = typeof(IQueryable).IsAssignableFrom(typeof(TResult)) ? typeof(Queryable) : typeof(Enumerable);
            object result = generic.GetMethods().Single(
                    method => method.Name == methodName
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), type)
                    .Invoke(null, new object[] { source, lambda });
            return (TResult)result;
        }         
    }   
}