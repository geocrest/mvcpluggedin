﻿//-------------------------------------------------------------------------------
// <copyright file="NinjectDependencyResolver.cs" company="bbv Software Services AG">
//   Copyright (c) 2012 bbv Software Services AG
//   Author: Remo Gloor (remo.gloor@gmail.com)
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
//-------------------------------------------------------------------------------

namespace Geocrest.Web.Mvc.DependencyResolution
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http.Dependencies;
    using Ninject;
    using Ninject.Parameters;
    using Ninject.Syntax;

    /// <summary>
    /// Dependency scope implementation for ninject.
    /// </summary>
    public class NinjectScope: IDependencyScope
    {
        /// <summary>
        /// The resolution root used to resolve dependencies.
        /// </summary>
        private IResolutionRoot resolutionRoot;
        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.DependencyResolution.NinjectScope" /> class.
        /// </summary>
        /// <param name="resolutionRoot">The resolution root.</param>
        public NinjectScope(IResolutionRoot resolutionRoot)
        {
            this.resolutionRoot = resolutionRoot;
        }

        /// <summary>
        /// Gets the service of the specified type.
        /// </summary>
        /// <param name="serviceType">The type of the service.</param>
        /// <returns>The service instance or <see langword="null"/> if none is configured.</returns>
        public object GetService(Type serviceType)
        {
            var request = this.resolutionRoot.CreateRequest(serviceType, null, new Parameter[0], true, true);
            return this.resolutionRoot.Resolve(request).SingleOrDefault();
        }

        /// <summary>
        /// Gets the services of the specified type.
        /// </summary>
        /// <param name="serviceType">The type of the service.</param>
        /// <returns>All service instances or an empty enumerable if none is configured.</returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.resolutionRoot.GetAll(serviceType).ToList();
        }
       
        #region IDisposable Members
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method. Therefore, you should call GC.SupressFinalize to 
            // take this object off the finalization queue and prevent finalization code for this object 
            // from executing a second time.
            GC.SuppressFinalize(this);            
        }
        // Dispose(bool disposing) executes in two distinct scenarios. If disposing equals true, 
        // the method has been called directly or indirectly by a user's code. Managed and unmanaged resources 
        // can be disposed. If disposing equals false, the method has been called by the runtime from inside 
        // the finalizer and you should not reference other objects. Only unmanaged resources can be disposed. 
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed and unmanaged resources. 
                if (disposing)
                {
                    IDisposable disposable = (IDisposable)resolutionRoot;
                    if (disposable != null) disposable.Dispose();
                    this.resolutionRoot = null;                    
                }                               
                disposed = true;
            }
        }
        #endregion
    }
}
