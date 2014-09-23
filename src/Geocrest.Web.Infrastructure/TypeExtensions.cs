namespace Geocrest.Web.Infrastructure
{
    /// <summary>
    /// Extension methods for <see cref="T:System.Type"/> objects.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Determines whether the <paramref name="type"/> implements <typeparamref name="T"/>.
        /// </summary>
        public static bool Implements<T>(this System.Type type)
        {
            Throw.IfArgumentNull(type, "type");
            return typeof(T).IsAssignableFrom(type);
        }
    }
}
