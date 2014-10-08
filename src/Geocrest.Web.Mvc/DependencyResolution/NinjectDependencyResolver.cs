//-------------------------------------------------------------------------------
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
    using System.Web.Http.Dependencies;
    using Ninject;
    using System;
    /// <summary>
    /// Dependency resolver implementation for ninject.
    /// </summary>
    [Obsolete("Install Ninject.Web.WebApi and Ninject.Web.WebApi.WebHost NuGet packages version >= 3.2.2, and allow Ninject to manage the resolver. Do not attempt to set GlobalConfiguration.Configuration.DependencyResolver.")]
    public class NinjectDependencyResolver : NinjectScope, IDependencyResolver
    {

        /// <summary>
        /// The kernel used to resolve dependencies.
        /// </summary>
        private readonly IKernel _kernel;


        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.DependencyResolution.NinjectDependencyResolver" /> class.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        public NinjectDependencyResolver(IKernel kernel) : base (kernel)
        {
            this._kernel = kernel;
        }
               
        #region IDependencyResolver Members

        /// <summary>
        /// Starts a resolution scope.
        /// </summary>
        /// <returns>
        /// A new <see cref="T:Geocrest.Web.Mvc.DependencyResolution.NinjectScope"/>.
        /// </returns>
        public IDependencyScope BeginScope()
        {
            return new NinjectScope(this._kernel.BeginBlock());
        }

        #endregion
    }
}