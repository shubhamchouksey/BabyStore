#pragma checksum "C:\Users\INS Viraat\source\repos\BabyStore\BabyStore\Views\UsersAdmin\Details.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "aed3525d82a8716c8837b0b1019bb8f8abc31273"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_UsersAdmin_Details), @"mvc.1.0.view", @"/Views/UsersAdmin/Details.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/UsersAdmin/Details.cshtml", typeof(AspNetCore.Views_UsersAdmin_Details))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\Users\INS Viraat\source\repos\BabyStore\BabyStore\Views\_ViewImports.cshtml"
using BabyStore;

#line default
#line hidden
#line 2 "C:\Users\INS Viraat\source\repos\BabyStore\BabyStore\Views\_ViewImports.cshtml"
using BabyStore.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"aed3525d82a8716c8837b0b1019bb8f8abc31273", @"/Views/UsersAdmin/Details.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"600ecb2f3555c616ab5607e5e3ec2d6c1842285a", @"/Views/_ViewImports.cshtml")]
    public class Views_UsersAdmin_Details : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<BabyStore.Data.ApplicationUser>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 2 "C:\Users\INS Viraat\source\repos\BabyStore\BabyStore\Views\UsersAdmin\Details.cshtml"
  
    ViewData["Title"] = "Details";

#line default
#line hidden
            BeginContext(82, 4, true);
            WriteLiteral("<h2>");
            EndContext();
            BeginContext(87, 17, false);
#line 5 "C:\Users\INS Viraat\source\repos\BabyStore\BabyStore\Views\UsersAdmin\Details.cshtml"
Write(ViewData["Title"]);

#line default
#line hidden
            EndContext();
            BeginContext(104, 84, true);
            WriteLiteral("</h2>\r\n<div>\r\n    <hr />\r\n    <dl class=\"dl-horizontal\">\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(189, 45, false);
#line 10 "C:\Users\INS Viraat\source\repos\BabyStore\BabyStore\Views\UsersAdmin\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.FirstName));

#line default
#line hidden
            EndContext();
            BeginContext(234, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(278, 41, false);
#line 13 "C:\Users\INS Viraat\source\repos\BabyStore\BabyStore\Views\UsersAdmin\Details.cshtml"
       Write(Html.DisplayFor(model => model.FirstName));

#line default
#line hidden
            EndContext();
            BeginContext(319, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(363, 44, false);
#line 16 "C:\Users\INS Viraat\source\repos\BabyStore\BabyStore\Views\UsersAdmin\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.LastName));

#line default
#line hidden
            EndContext();
            BeginContext(407, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(451, 40, false);
#line 19 "C:\Users\INS Viraat\source\repos\BabyStore\BabyStore\Views\UsersAdmin\Details.cshtml"
       Write(Html.DisplayFor(model => model.LastName));

#line default
#line hidden
            EndContext();
            BeginContext(491, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(535, 41, false);
#line 22 "C:\Users\INS Viraat\source\repos\BabyStore\BabyStore\Views\UsersAdmin\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.Email));

#line default
#line hidden
            EndContext();
            BeginContext(576, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(620, 37, false);
#line 25 "C:\Users\INS Viraat\source\repos\BabyStore\BabyStore\Views\UsersAdmin\Details.cshtml"
       Write(Html.DisplayFor(model => model.Email));

#line default
#line hidden
            EndContext();
            BeginContext(657, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(701, 47, false);
#line 28 "C:\Users\INS Viraat\source\repos\BabyStore\BabyStore\Views\UsersAdmin\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.DateOfBirth));

#line default
#line hidden
            EndContext();
            BeginContext(748, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(792, 43, false);
#line 31 "C:\Users\INS Viraat\source\repos\BabyStore\BabyStore\Views\UsersAdmin\Details.cshtml"
       Write(Html.DisplayFor(model => model.DateOfBirth));

#line default
#line hidden
            EndContext();
            BeginContext(835, 29, true);
            WriteLiteral("\r\n        </dd>\r\n            ");
            EndContext();
            BeginContext(865, 37, false);
#line 33 "C:\Users\INS Viraat\source\repos\BabyStore\BabyStore\Views\UsersAdmin\Details.cshtml"
       Write(Html.DisplayFor(model=>model.Address));

#line default
#line hidden
            EndContext();
            BeginContext(902, 127, true);
            WriteLiteral("\r\n    </dl>\r\n</div>\r\n<div class=\"container\">\r\n    <div class=\"row\">\r\n        <h4>Roles this user belongs to:</h4>\r\n    </div>\r\n");
            EndContext();
#line 40 "C:\Users\INS Viraat\source\repos\BabyStore\BabyStore\Views\UsersAdmin\Details.cshtml"
     if (ViewBag.RoleNames.Count == 0)
    {

#line default
#line hidden
            BeginContext(1076, 61, true);
            WriteLiteral("        <hr />\r\n        <p>No roles found for this user</p>\r\n");
            EndContext();
#line 44 "C:\Users\INS Viraat\source\repos\BabyStore\BabyStore\Views\UsersAdmin\Details.cshtml"
    }

#line default
#line hidden
            BeginContext(1144, 4, true);
            WriteLiteral("    ");
            EndContext();
#line 45 "C:\Users\INS Viraat\source\repos\BabyStore\BabyStore\Views\UsersAdmin\Details.cshtml"
     foreach (var item in ViewBag.RoleNames)
    {

#line default
#line hidden
            BeginContext(1197, 39, true);
            WriteLiteral("        <div class=\"row\">\r\n            ");
            EndContext();
            BeginContext(1237, 4, false);
#line 48 "C:\Users\INS Viraat\source\repos\BabyStore\BabyStore\Views\UsersAdmin\Details.cshtml"
       Write(item);

#line default
#line hidden
            EndContext();
            BeginContext(1241, 18, true);
            WriteLiteral("\r\n        </div>\r\n");
            EndContext();
#line 50 "C:\Users\INS Viraat\source\repos\BabyStore\BabyStore\Views\UsersAdmin\Details.cshtml"
    }

#line default
#line hidden
            BeginContext(1266, 17, true);
            WriteLiteral("</div>\r\n<p>\r\n    ");
            EndContext();
            BeginContext(1284, 54, false);
#line 53 "C:\Users\INS Viraat\source\repos\BabyStore\BabyStore\Views\UsersAdmin\Details.cshtml"
Write(Html.ActionLink("Edit", "Edit", new { id = Model.Id }));

#line default
#line hidden
            EndContext();
            BeginContext(1338, 8, true);
            WriteLiteral(" |\r\n    ");
            EndContext();
            BeginContext(1347, 40, false);
#line 54 "C:\Users\INS Viraat\source\repos\BabyStore\BabyStore\Views\UsersAdmin\Details.cshtml"
Write(Html.ActionLink("Back to List", "Index"));

#line default
#line hidden
            EndContext();
            BeginContext(1387, 10, true);
            WriteLiteral("\r\n</p>\r\n\r\n");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<BabyStore.Data.ApplicationUser> Html { get; private set; }
    }
}
#pragma warning restore 1591
