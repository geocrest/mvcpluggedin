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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/Http404.cshtml")]
    public partial class Http404 : System.Web.Mvc.WebViewPage<Geocrest.Web.Mvc.NotFoundViewModel>
    {
        public Http404()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Views\Shared\Http404.cshtml"
  
    ViewBag.Title = "An error has occurred...";

            
            #line default
            #line hidden
WriteLiteral("\r\n<style");

WriteLiteral(" type=\"text/css\"");

WriteLiteral(@">     
    .flickr {
        position: absolute;
        right: 5px;
        bottom: 2px;
        line-height: 16px;
    }
    .flickr a {
        font-size: 10px;
        background: none;
        vertical-align: middle;
        color: white;
        text-decoration:none;
        font-family: 'Open Sans',Arial,sans-serif;
    }
</style>
<div");

WriteLiteral(" class=\"container\"");

WriteLiteral(" >\r\n    <div");

WriteLiteral(" class=\"row text-center\"");

WriteLiteral(">\r\n        <h2>\r\n            <code>404</code></h2>\r\n        <div");

WriteLiteral(" class=\"notfoundcontainer clearmargin rounded\"");

WriteLiteral(">\r\n            <img");

WriteAttribute("src", Tuple.Create(" src=\"", 664), Tuple.Create("\"", 735)
            
            #line 26 "..\..\Views\Shared\Http404.cshtml"
, Tuple.Create(Tuple.Create("", 670), Tuple.Create<System.Object, System.Int32>(Url.Content(string.Format("~/error/{0}", "contentfile/404.jpg"))
            
            #line default
            #line hidden
, 670), false)
);

WriteLiteral("\r\n                 class=\"notfound rounded img-thumbnail\"");

WriteLiteral(" />\r\n            <div");

WriteLiteral(" class=\'flickr\'");

WriteLiteral(">\r\n                <a");

WriteLiteral(" href=\'http://www.flickr.com/photos/factoryjoe/7785039260\'");

WriteLiteral(">\r\n                    <img");

WriteLiteral(" alt=\'flickr\'");

WriteAttribute("src", Tuple.Create(" src=\"", 948), Tuple.Create("\"", 1024)
            
            #line 30 "..\..\Views\Shared\Http404.cshtml"
, Tuple.Create(Tuple.Create("", 954), Tuple.Create<System.Object, System.Int32>(Url.Content(string.Format("~/error/{0}", "contentfile/flickr16.png"))
            
            #line default
            #line hidden
, 954), false)
);

WriteLiteral("/>\r\n                    factoryjoe/7785039260</a>\r\n            </div>\r\n        </" +
"div>\r\n        <h4>\r\n            Please check the address and try again.</h4>\r\n  " +
"  </div>\r\n</div>");

        }
    }
}
#pragma warning restore 1591
