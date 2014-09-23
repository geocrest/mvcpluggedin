namespace Geocrest.Web.Infrastructure.DependencyResolution
{
    using System;
    using Ninject;

    /// <summary>
    /// Specifies that a class, method, or property should be included in Ninject's bindings.
    /// </summary>
    /// <remarks>
    /// <para>The primary reason this class exists is so that the web api framework
    /// can identify the classes within the various modules that need to be added to the application's
    /// <see cref="P:Geocrest.Web.WebApi.Mvc.Application.Kernel">StandardKernel</see>.</para>
    /// <para>
    /// This class adds the ability to apply the attribute to classes.
    /// </para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class NinjectionAttribute : InjectAttribute
    {
        private string named;

        /// <summary>
        /// Gets or sets an optional named attribute for distinguishing multiple implementations
        /// of an interface or base class so that this attribute can be set in the same layer where
        /// the interface or class is defined.
        /// </summary>
        public string Named
        {
            get
            {
                return this.named;
            }
            set
            {
                this.named = value;
            }
        }
    }
}
