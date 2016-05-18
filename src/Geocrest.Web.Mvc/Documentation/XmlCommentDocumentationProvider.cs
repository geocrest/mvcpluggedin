namespace Geocrest.Web.Mvc.Documentation
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Resources;
    using System.Text.RegularExpressions;
    using System.Web.Http.Controllers;
    using System.Web.Http.Description;
    using System.Xml.XPath;
    using Geocrest.Web.Infrastructure;
    /// <summary>
    /// Provides documentation from inline XML comments.
    /// </summary>
    public class XmlCommentDocumentationProvider : IDocumentationProvider, IResponseDocumentationProvider,
        IExampleDocumentationProvider
    {
        #region Fields
        private string[] folders;
        private bool searchSub = true;
        private List<string> files = new List<string>();
        private const string METHODEXPRESSION = "/doc/members/member[@name='M:{0}']";
        private const string PARAMETEREXPRESSION = "param[@name='{0}']";
        private const string NODOCUMENTATION = "No Documentation Found.";
        private static Regex nullableTypeNameRegex = new Regex(@"(.*\.Nullable)" + Regex.Escape("`1[[") + "([^,]*),.*");
        private DefaultXmlReplacements defaults = new DefaultXmlReplacements();
        private ResourceManager resourceManager;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Documentation.XmlCommentDocumentationProvider" /> class.
        /// </summary>
        /// <param name="searchSubDirectories">if set to <b>true</b> search sub directories. The default is true.</param>
        /// <param name="folders">The folders.</param>
        /// <exception cref="T:System.ArgumentNullException">folders</exception>
        public XmlCommentDocumentationProvider(bool searchSubDirectories = true, params string[] folders)
        {
            Throw.IfArgumentNullOrEmpty("folders", "folders");
            this.searchSub = searchSubDirectories;
            this.folders = folders;
            foreach (string folder in folders)
            {
                DirectoryInfo di = new DirectoryInfo(folder);
                if (di.Exists)
                {
                    files.AddRange(di.EnumerateFiles("*.xml",
                        searchSubDirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).
                        Select(f => f.FullName));
                }
            }
            this.resourceManager = new ResourceManager("Geocrest.Web.Mvc.Documentation.HelpResources", Assembly.GetExecutingAssembly());
        }

        #region IDocumentationProvider Members

        /// <summary>
        /// Gets the documentation based on <see cref="T:System.Web.Http.Controllers.HttpParameterDescriptor" />.
        /// </summary>
        /// <param name="parameterDescriptor">The parameter descriptor.</param>
        /// <returns>
        /// The documentation for the controller.
        /// </returns>
        public virtual string GetDocumentation(HttpParameterDescriptor parameterDescriptor)
        {
            ReflectedHttpParameterDescriptor reflectedParameterDescriptor =
                (ReflectedHttpParameterDescriptor)parameterDescriptor;
            if (reflectedParameterDescriptor != null)
            {
                string xml = GetXmlFilePath(parameterDescriptor);
                if (xml != null)
                {
                    XPathNavigator methodNode = GetMethodNode(CreateNavigator(xml),
                        reflectedParameterDescriptor.ActionDescriptor);
                    if (methodNode != null)
                    {
                        string parameterName = reflectedParameterDescriptor.ParameterInfo.Name;
                        XPathNavigator parameterNode = methodNode.SelectSingleNode(string.Format(
                            CultureInfo.InvariantCulture, PARAMETEREXPRESSION, parameterName));
                        if (parameterNode != null)
                        {
                            var value = parameterNode.Value.Trim();
                            string defaultValue;
                            if (defaults.TryGetValue(parameterName, out defaultValue) && 
                                value.ToLower() == defaultValue.ToLower())
                            {
                                try
                                {
                                    value = resourceManager.GetString(parameterName);
                                }
                                catch { }
                            }
                            // replace tokens
                            //if (defaults.Tokens.Any(t => value.ToLower().Contains(t.ToLower())))
                            //{

                            //    defaults.Tokens.ForEach(x => value.ToLower().Replace(x.ToLower(),
                            //}
                            return value;
                        }
                    }
                }
            }

            return NODOCUMENTATION;
        }

        /// <summary>
        /// Gets the documentation based on <see cref="T:System.Web.Http.Controllers.HttpActionDescriptor" />.
        /// </summary>
        /// <param name="actionDescriptor">The action descriptor.</param>
        /// <returns>
        /// The documentation for the controller.
        /// </returns>
        public virtual string GetDocumentation(HttpActionDescriptor actionDescriptor)
        {
            return GetNode(actionDescriptor, "summary");
        }

        //
        // WebApi2 Upgrade:
        // 
        /// <summary>
        /// Throws NotImplementedException.
        /// </summary>
        /// <param name="controllerDescriptor">The controller descriptor.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public string GetDocumentation(HttpControllerDescriptor controllerDescriptor)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IResponseDocumentationProvider Members

        /// <summary>
        /// Gets the documentation for a specific method's return value.
        /// </summary>
        /// <param name="actionDescriptor">An <see cref="T:System.Web.Http.Controllers.HttpActionDescriptor">HttpActionDescriptor</see>
        /// containing information about the method.</param>
        /// <returns>
        /// Returns documentation about the return value of the method.
        /// </returns>
        public string GetResponseDocumentation(HttpActionDescriptor actionDescriptor)
        {
            return GetNode(actionDescriptor, "returns");
        }

        #endregion

        #region IExampleDocumentationProvider Members
        
        /// <summary>
        /// Gets example code for a specific method.
        /// </summary>
        /// <param name="actionDescriptor">An <see cref="T:System.Web.Http.Controllers.HttpActionDescriptor">HttpActionDescriptor</see>
        /// containing information about the method.</param>
        /// <returns>
        /// Returns the content of the <c>example</c> tag from inline XML documentation.
        /// </returns>
        public string GetExampleDocumentation(HttpActionDescriptor actionDescriptor)
        {
            return GetNode(actionDescriptor, "example");
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Creates a new XPathNavigator.
        /// </summary>
        /// <param name="path">An object of the type <see cref="T:System.String"/>.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:System.Xml.XPath.XPathNavigator"/>
        /// </returns>
        private XPathNavigator CreateNavigator(string path)
        {
            Throw.IfArgumentNullOrEmpty("path", "path");
            XPathDocument xpath = new XPathDocument(path);
            return xpath.CreateNavigator();
        }
        /// <summary>
        /// Gets the method node.
        /// </summary>
        /// <param name="navigator">An object of the type <see cref="System.Xml.XPath.XPathNavigator"/>.</param>
        /// <param name="actionDescriptor">An object of the type <see cref="System.Web.Http.Controllers.HttpActionDescriptor"/>.</param>
        /// <returns>
        /// Returns an instance of <see cref="System.Xml.XPath.XPathNavigator"/>
        /// </returns>
        private XPathNavigator GetMethodNode(XPathNavigator navigator, HttpActionDescriptor actionDescriptor)
        {
            ReflectedHttpActionDescriptor reflectedActionDescriptor = actionDescriptor as ReflectedHttpActionDescriptor;
            if (reflectedActionDescriptor != null)
            {
                string selectExpression = string.Format(CultureInfo.InvariantCulture, METHODEXPRESSION,
                    GetMemberName(reflectedActionDescriptor.MethodInfo));
                return navigator.SelectSingleNode(selectExpression);
            }
            return null;
        }
        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        /// <param name="method">An object of the type <see cref="System.Reflection.MethodInfo"/>.</param>
        /// <returns></returns>
        private static string GetMemberName(MethodInfo method)
        {
            string name = String.Format(CultureInfo.InvariantCulture, "{0}.{1}",
                method.DeclaringType.FullName, method.Name);
            ParameterInfo[] parameters = method.GetParameters();
            if (parameters.Length != 0)
            {
                string[] parameterTypeNames = parameters.Select(param => GetTypeName(param.ParameterType)).ToArray();
                name += String.Format(CultureInfo.InvariantCulture, "({0})", String.Join(",", parameterTypeNames));
            }
            return name;
        }
        /// <summary>
        /// Gets the name of the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private static string GetTypeName(Type type)
        {
            if (type.IsGenericType)
            {
                // Format the generic type name to something like: Generic{System.Int32,System.String}
                Type genericType = type.GetGenericTypeDefinition();
                Type[] genericArguments = type.GetGenericArguments();
                string typeName = genericType.FullName;

                // Trim the generic parameter counts from the name
                typeName = typeName.Substring(0, typeName.IndexOf('`'));
                string[] argumentTypeNames = genericArguments.Select(t => GetTypeName(t)).ToArray();
                return String.Format(CultureInfo.InvariantCulture, "{0}{{{1}}}", typeName, String.Join(",", argumentTypeNames));
            }

            return type.FullName;
        }
        /// <summary>
        /// Gets the XML file path containing the specified descriptor.
        /// </summary>
        /// <param name="actionDescriptor">The action descriptor.</param>
        /// <returns>
        /// Returns an instance of <see cref="System.String">System.String</see>.
        /// </returns>
        private string GetXmlFilePath(HttpActionDescriptor actionDescriptor)
        {
            return GetXmlFilePath(actionDescriptor.ControllerDescriptor.ControllerType.Assembly.ManifestModule.Name);
        }
        /// <summary>
        /// Gets the XML file path containing the specified descriptor.
        /// </summary>
        /// <param name="parameterDescriptor">The parameter descriptor.</param>
        /// <returns>
        /// Returns an instance of <see cref="System.String">System.String</see>.
        /// </returns>
        private string GetXmlFilePath(HttpParameterDescriptor parameterDescriptor)
        {
            return GetXmlFilePath(parameterDescriptor.ActionDescriptor.ControllerDescriptor.ControllerType.Assembly
                .ManifestModule.Name);
        }
        /// <summary>
        /// Gets the XML file path based on the path of the assembly.
        /// </summary>
        /// <param name="dll">The DLL.</param>
        /// <returns>
        /// Returns an instance of <see cref="System.String">System.String</see>.
        /// </returns>
        private string GetXmlFilePath(string dll)
        {
            return this.files.SingleOrDefault(x => x.ToLower().Contains(dll.ToLower().Replace(".dll", ".xml")));
        }
        private string GetNode(HttpActionDescriptor actionDescriptor, string nodeType)
        {
            string xml = GetXmlFilePath(actionDescriptor);
            if (xml != null)
            {
                XPathNavigator methodNode = GetMethodNode(CreateNavigator(xml), actionDescriptor);
                if (methodNode != null)
                {
                    XPathNavigator returnNode = methodNode.SelectSingleNode(nodeType);
                    if (returnNode != null)
                    {                       
                        return returnNode.Value.Trim();
                    }
                }
            }
            return NODOCUMENTATION;
        }
        #endregion

    }
}
