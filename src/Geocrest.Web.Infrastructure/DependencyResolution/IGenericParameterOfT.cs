namespace Geocrest.Web.Infrastructure.DependencyResolution
{
    /// <summary>
    /// Provides a wrapper interface that can be used to inject any type of parameter into a class. 
    /// The primary benefit of this interface is when a dependency on a simple value type needs to 
    /// be injected (e.g. string, int, double, etc) and the class type with the dependency does 
    /// not exist in the current appdomain.
    /// </summary>
    /// <typeparam name="T">The type of parameter being injected.</typeparam>
    public interface IGenericParameter<T>
    {
        /// <summary>
        /// Gets the value of the parameter.
        /// </summary>
        /// <value>
        /// A value of type <typeparam name="T">T</typeparam>.
        /// </value>
        T Value { get; }
        /// <summary>
        /// Gets a value indicating whether the parameter value has been created.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the value has been created; otherwise, <c>false</c>.
        /// </value>
        bool IsValueCreated { get; }
    }
}
