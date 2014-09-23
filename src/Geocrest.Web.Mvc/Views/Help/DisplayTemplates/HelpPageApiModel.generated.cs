﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Geocrest.Web.Mvc.EmbeddedViews
{
    using System;
    using System.Collections.Generic;
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Help/DisplayTemplates/HelpPageApiModel.cshtml")]
    public partial class _HelpPageApiModel : System.Web.Mvc.WebViewPage<Geocrest.Web.Mvc.Documentation.HelpPageApiModel>
    {
        public _HelpPageApiModel()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Views\Help\DisplayTemplates\HelpPageApiModel.cshtml"
  
    var description = Model.ApiDescription;
    bool hasParameters = description.ParameterDescriptions.Count > 0;
    bool hasRequestSamples = Model.SampleRequests.Count > 0;
    bool hasResponseSamples = Model.SampleResponses.Count > 0;
    bool hasLiveSamples = Model.HtmlSamples.Count > 0;

            
            #line default
            #line hidden
WriteLiteral("\r\n<div");

WriteLiteral(" class=\"container\"");

WriteLiteral(">\r\n    <section");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"col-md-12\"");

WriteLiteral(">\r\n            <h3>\r\n                <code>\r\n");

WriteLiteral("                    ");

            
            #line 15 "..\..\Views\Help\DisplayTemplates\HelpPageApiModel.cshtml"
               Write(description.HttpMethod.Method);

            
            #line default
            #line hidden
WriteLiteral(" ");

            
            #line 15 "..\..\Views\Help\DisplayTemplates\HelpPageApiModel.cshtml"
                                               Write(description.RelativePath.Contains("?") ?
                    description.RelativePath.Substring(0, description.RelativePath.IndexOf("?")).ToLower() :
                    description.RelativePath.ToLower());

            
            #line default
            #line hidden
WriteLiteral("\r\n                </code>\r\n            </h3>\r\n        </div>\r\n        <div");

WriteLiteral(" class=\"col-md-12\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"well\"");

WriteLiteral(">\r\n");

            
            #line 23 "..\..\Views\Help\DisplayTemplates\HelpPageApiModel.cshtml"
                
            
            #line default
            #line hidden
            
            #line 23 "..\..\Views\Help\DisplayTemplates\HelpPageApiModel.cshtml"
                 if (description.Documentation != null)
                {
                    
            
            #line default
            #line hidden
            
            #line 25 "..\..\Views\Help\DisplayTemplates\HelpPageApiModel.cshtml"
               Write(MvcHtmlString.Create(description.Documentation));

            
            #line default
            #line hidden
            
            #line 25 "..\..\Views\Help\DisplayTemplates\HelpPageApiModel.cshtml"
                                                                    
                }
                else
                {

            
            #line default
            #line hidden
WriteLiteral("                    ");

WriteLiteral("No documentation available.\r\n");

            
            #line 30 "..\..\Views\Help\DisplayTemplates\HelpPageApiModel.cshtml"
                }

            
            #line default
            #line hidden
WriteLiteral("            </div>\r\n        </div>\r\n    </section>\r\n    <section");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"col-md-12\"");

WriteLiteral(">\r\n");

            
            #line 36 "..\..\Views\Help\DisplayTemplates\HelpPageApiModel.cshtml"
            
            
            #line default
            #line hidden
            
            #line 36 "..\..\Views\Help\DisplayTemplates\HelpPageApiModel.cshtml"
             if (hasLiveSamples)
            {

            
            #line default
            #line hidden
WriteLiteral("                <h3>Live Samples</h3>\r\n");

            
            #line 39 "..\..\Views\Help\DisplayTemplates\HelpPageApiModel.cshtml"
                foreach (var sample in Model.HtmlSamples)
                {
                    
            
            #line default
            #line hidden
            
            #line 41 "..\..\Views\Help\DisplayTemplates\HelpPageApiModel.cshtml"
               Write(Html.DisplayFor(s => sample, "HtmlSample"));

            
            #line default
            #line hidden
            
            #line 41 "..\..\Views\Help\DisplayTemplates\HelpPageApiModel.cshtml"
                                                               
                }
            }

            
            #line default
            #line hidden
WriteLiteral("        </div>\r\n    </section>\r\n    <section");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"col-md-12\"");

WriteLiteral(">\r\n");

            
            #line 48 "..\..\Views\Help\DisplayTemplates\HelpPageApiModel.cshtml"
            
            
            #line default
            #line hidden
            
            #line 48 "..\..\Views\Help\DisplayTemplates\HelpPageApiModel.cshtml"
             if (hasParameters || hasRequestSamples)
            {

            
            #line default
            #line hidden
WriteLiteral("                <h3>Request Information</h3>\r\n");

            
            #line 51 "..\..\Views\Help\DisplayTemplates\HelpPageApiModel.cshtml"
                if (hasParameters)
                {

            
            #line default
            #line hidden
WriteLiteral("                    <h4>Parameters</h4>\r\n");

            
            #line 54 "..\..\Views\Help\DisplayTemplates\HelpPageApiModel.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 54 "..\..\Views\Help\DisplayTemplates\HelpPageApiModel.cshtml"
               Write(Html.DisplayFor(apiModel => apiModel.ApiDescription.ParameterDescriptions, "Parameters"));

            
            #line default
            #line hidden
            
            #line 54 "..\..\Views\Help\DisplayTemplates\HelpPageApiModel.cshtml"
                                                                                                             
                }
                if (hasRequestSamples)
                {

            
            #line default
            #line hidden
WriteLiteral("                    <h4>Request formats</h4>\r\n");

            
            #line 59 "..\..\Views\Help\DisplayTemplates\HelpPageApiModel.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 59 "..\..\Views\Help\DisplayTemplates\HelpPageApiModel.cshtml"
               Write(Html.DisplayFor(apiModel => apiModel.SampleRequests, "Samples", new { isResponse = false }));

            
            #line default
            #line hidden
            
            #line 59 "..\..\Views\Help\DisplayTemplates\HelpPageApiModel.cshtml"
                                                                                                                
                }
            }

            
            #line default
            #line hidden
WriteLiteral("        </div>\r\n    </section>\r\n    <section");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"col-md-12\"");

WriteLiteral(">\r\n");

            
            #line 66 "..\..\Views\Help\DisplayTemplates\HelpPageApiModel.cshtml"
            
            
            #line default
            #line hidden
            
            #line 66 "..\..\Views\Help\DisplayTemplates\HelpPageApiModel.cshtml"
             if (hasResponseSamples)
            {

            
            #line default
            #line hidden
WriteLiteral("                <h3");

WriteLiteral(" id=\"responseInformation\"");

WriteLiteral(">Response Information</h3>\r\n");

            
            #line 69 "..\..\Views\Help\DisplayTemplates\HelpPageApiModel.cshtml"
                if (!String.IsNullOrEmpty(Model.ResponseDocumentation))
                {

            
            #line default
            #line hidden
WriteLiteral("                    <div");

WriteLiteral(" class=\"well\"");

WriteLiteral(">\r\n                        <p>");

            
            #line 72 "..\..\Views\Help\DisplayTemplates\HelpPageApiModel.cshtml"
                      Write(MvcHtmlString.Create(Model.ResponseDocumentation));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                    </div>\r\n");

            
            #line 74 "..\..\Views\Help\DisplayTemplates\HelpPageApiModel.cshtml"
                }

            
            #line default
            #line hidden
WriteLiteral("                <h4>Response body formats</h4>\r\n");

            
            #line 76 "..\..\Views\Help\DisplayTemplates\HelpPageApiModel.cshtml"
                Dictionary<string, object> samples = Model.SampleResponses.GroupBy(pair => pair.Value).ToDictionary(
                    pair => String.Join(", ", pair.Select(m => m.Key.ToString()).ToArray()),
                    pair => pair.Key);
                var mediaTypes = samples.Keys;
                foreach (var type in mediaTypes)
                {

            
            #line default
            #line hidden
WriteLiteral("                    <a");

WriteAttribute("href", Tuple.Create(" href=\'", 3134), Tuple.Create("\'", 3220)
            
            #line 82 "..\..\Views\Help\DisplayTemplates\HelpPageApiModel.cshtml"
, Tuple.Create(Tuple.Create("", 3141), Tuple.Create<System.Object, System.Int32>(string.Format("#{0}",type.Replace(", ","_").Replace("/","_").Replace("+", ""))
            
            #line default
            #line hidden
, 3141), false)
);

WriteLiteral(" \r\n                       class=\"btn btn-info btn-small scroll\"");

WriteLiteral(">");

            
            #line 83 "..\..\Views\Help\DisplayTemplates\HelpPageApiModel.cshtml"
                                                        Write(type);

            
            #line default
            #line hidden
WriteLiteral("</a>\r\n");

            
            #line 84 "..\..\Views\Help\DisplayTemplates\HelpPageApiModel.cshtml"
                }
                
            
            #line default
            #line hidden
            
            #line 85 "..\..\Views\Help\DisplayTemplates\HelpPageApiModel.cshtml"
           Write(Html.DisplayFor(apiModel => apiModel.SampleResponses, "Samples", new { isResponse = true }));

            
            #line default
            #line hidden
            
            #line 85 "..\..\Views\Help\DisplayTemplates\HelpPageApiModel.cshtml"
                                                                                                            
            }

            
            #line default
            #line hidden
WriteLiteral("        </div>\r\n    </section>\r\n</div>");

        }
    }
}
#pragma warning restore 1591
