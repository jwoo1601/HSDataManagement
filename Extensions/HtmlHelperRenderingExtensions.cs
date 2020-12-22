using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;

namespace HyosungManagement.Extensions
{
    public static class HtmlHelperRenderingExtensions
    {
        private static string BuildPartialSectionName(string sectionName)
        {
            return $"__PartialSection-{sectionName}";
        }

        public static HtmlString PartialSection(
            this IHtmlHelper helper,
            string sectionName,
            Func<HtmlString, HelperResult> htmlContent,
            string environment = "Development"
        )
        {
            var env = helper.ViewContext.HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
            if (htmlContent == null || (environment != null && !env.IsEnvironment(environment)))
            {
                return HtmlString.Empty;
            }

            var decoratedName = BuildPartialSectionName(sectionName);
            var viewData = helper.ViewContext.HttpContext.Items;
            if (viewData.ContainsKey(decoratedName))
            {
                var sectionData = viewData[decoratedName] as IList<Func<HtmlString, HelperResult>>;
                if (sectionData == null)
                {
                    Trace.WriteLine($"Invalid partial section data for {sectionName}");
                }
                else
                {
                    sectionData.Add(htmlContent);
                }
            }
            else
            {
                var sectionData = new List<Func<HtmlString, HelperResult>>();
                sectionData.Add(htmlContent);
                viewData[decoratedName] = sectionData;
            }

            return HtmlString.Empty;
        }

        public static HtmlString RenderPartialSection(this IHtmlHelper helper, string sectionName)
        {
            var decoratedName = BuildPartialSectionName(sectionName);
            var viewData = helper.ViewContext.HttpContext.Items;
            if (viewData.ContainsKey(decoratedName))
            {
                var sectionData = viewData[decoratedName] as IList<Func<HtmlString, HelperResult>>;
                if (sectionData != null)
                {
                    var sb = new StringBuilder();
                    using (var writer = new StringWriter(sb))
                    {
                        foreach (var htmlTag in sectionData)
                        {
                            htmlTag(HtmlString.Empty).WriteTo(writer, HtmlEncoder.Default);
                        }
                    }

                    return new HtmlString(sb.ToString());
                }
            }

            return HtmlString.Empty;
        }
    }
}
