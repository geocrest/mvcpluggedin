﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Geocrest.Web.Mvc.EmbeddedViews
{
    using System;
    using System.Collections.Generic;
    
    #line 1 "..\..\Views\Help\Index.cshtml"
    using System.Collections.ObjectModel;
    
    #line default
    #line hidden
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using Geocrest.Web.Mvc;
    
    #line 2 "..\..\Views\Help\Index.cshtml"
    using Geocrest.Web.Mvc.Documentation;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Help/Index.cshtml")]
    public partial class Index : System.Web.Mvc.WebViewPage<IEnumerable<System.Web.Http.Description.ApiDescription>>
    {
        public Index()
        {
        }
        public override void Execute()
        {
            
            #line 5 "..\..\Views\Help\Index.cshtml"
  
    // Group APIs by controller
    ILookup<string, System.Web.Http.Description.ApiDescription> apiGroups =
        Model.OrderBy(x => x.ActionDescriptor.ControllerDescriptor.ControllerName)
        .ToLookup(api => api.ActionDescriptor.ControllerDescriptor.ControllerName);
    string css = "Geocrest.Web.Mvc.Documentation.HelpPage.css";

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 12 "..\..\Views\Help\Index.cshtml"
Write(Html.CssLink(css));

            
            #line default
            #line hidden
WriteLiteral("\r\n<div");

WriteLiteral(" class=\"container\"");

WriteLiteral(">\r\n");

            
            #line 14 "..\..\Views\Help\Index.cshtml"
    
            
            #line default
            #line hidden
            
            #line 14 "..\..\Views\Help\Index.cshtml"
     foreach (var group in apiGroups)
    {

            
            #line default
            #line hidden
WriteLiteral("        <section");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"col-md-12\"");

WriteLiteral(">\r\n");

WriteLiteral("                ");

            
            #line 18 "..\..\Views\Help\Index.cshtml"
           Write(Html.DisplayFor(m => group, "ApiGroup"));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n        </section>\r\n");

            
            #line 21 "..\..\Views\Help\Index.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n");

        }
    }
}
#pragma warning restore 1591
