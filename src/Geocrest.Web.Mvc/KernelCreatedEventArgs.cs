
namespace Geocrest.Web.Mvc
{
    using System;
    using Ninject;

    /// <summary>
    /// Event arguments containing the created kernel instance
    /// </summary>
    public class KernelCreatedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.KernelCreatedEventArgs" /> class.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        public KernelCreatedEventArgs(IKernel kernel)
        {
            this.Kernel = kernel;
        }
        /// <summary>
        /// Gets the kernel for the application.
        /// </summary>
        /// <value>
        /// The kernel.
        /// </value>
        public IKernel Kernel { get; private set; }
    }
}
