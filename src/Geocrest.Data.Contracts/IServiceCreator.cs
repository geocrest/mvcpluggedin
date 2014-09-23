
namespace Geocrest.Data.Contracts
{
    /// <summary>
    /// Provides methods for creating WCF service clients
    /// </summary>
    /// <typeparam name="T">The service client type.</typeparam>
    /// <typeparam name="I">The interface type that <typeparamref name="T"/> implements.</typeparam>
    public interface IServiceCreator<T,I> where T : I 
    {
        /// <summary>
        /// Creates a service client using a default endpoint.
        /// </summary>
        /// <returns>A service client of type <typeparamref name="T"/></returns>
        T Create();
        /// <summary>
        /// Creates a service client using the specified endpoint.
        /// </summary>
        /// <param name="address">The address endpoint.</param>
        /// <returns>A service client of type <typeparamref name="T"/></returns>
        T Create(string address);
    }
}
