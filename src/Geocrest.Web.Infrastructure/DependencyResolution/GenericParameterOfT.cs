namespace Geocrest.Web.Infrastructure.DependencyResolution
{
    /// <summary>
    /// Provides a basic implementation of the <see cref="T:Geocrest.Web.Infrastructure.DependencyResolution.IGenericParameter`1"/> 
    /// interface for injecting basic types.
    /// </summary>
    /// <typeparam name="T">The type of parameter being injected.</typeparam>
    public class GenericParameter<T> : IGenericParameter<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Infrastructure.DependencyResolution.GenericParameter{T}" /> class.
        /// </summary>
        /// <param name="value">The value of the parameter.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="value"/> is null</exception>
        public GenericParameter(T value)
        {
            Throw.IfArgumentNull(value, "value");
            this.Value = value;
            this.IsValueCreated = true;
        }
        /// <summary>
        /// Gets the value of the parameter.
        /// </summary>
        /// <value>
        /// A value of type <typeparam name="T">T</typeparam>.
        public T Value { get; private set; }
        /// <summary>
        /// Gets a value indicating whether the parameter value has been created.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the value has been created; otherwise, <c>false</c>.
        /// </value>
        public bool IsValueCreated { get; private set; }
    }
}
