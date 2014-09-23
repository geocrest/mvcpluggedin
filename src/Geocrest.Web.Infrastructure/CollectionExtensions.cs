namespace Geocrest.Web.Infrastructure
{
    using System;
    using System.Linq;

    /// <summary>
    /// Provides helper methods for arrays and collections
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>Converts all values in an array of one type to another type.</summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="array">The array to convert.</param>
        /// <param name="converter">The converter.</param>
        /// <returns>Returns an instance of <see cref="TOutput[]"/></returns>
        public static TOutput[] ConvertAll<TInput, TOutput>(this TInput[] array, Converter<TInput, TOutput> converter)
        {
            if (array == null)
                throw new ArgumentException();

            return (from item in array select converter(item)).ToArray();
        }
    }
}
