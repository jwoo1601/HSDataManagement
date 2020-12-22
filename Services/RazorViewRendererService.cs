using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using Syncfusion.Pdf.HtmlToPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Services
{
    public class ViewRendererOptions
    {
        public static readonly string Name = "Views";

        public string TemplateSearchPath { get; set; } = "Templates";
    }

    public class ViewPdfRendererOptions
    {
        public static readonly string Name = "Views:Pdf";

        public string WebkitPath { get; set; }
        // Delay in milliseconds for loading external resources
        public int ResourceDelay { get; set; }
        // Indicates whether or not to generate bookmarks
        public bool GenerateBookmarks { get; set; }
        // Indicates whether or not to generate Table of Contents page
        public bool GenerateToc { get; set; }
        public bool EnableHyperlinks { get; set; }
        public bool EnableJavascript { get; set; }
        public string TempPath { get; set; }
    }

    public class RazorViewRendererService : IViewRendererService
    {
        IRazorViewEngine ViewEngine { get; }
        IHttpContextAccessor ContextAccessor { get; }
        IServiceProvider ServiceProvider { get; }
        ITempDataProvider TempDataProvider { get; }
        IWebHostEnvironment HostEnvironment { get; }
        IOptions<ViewRendererOptions> Options { get; }
        IOptions<ViewPdfRendererOptions> PdfOptions { get; }


        public RazorViewRendererService(
            IRazorViewEngine viewEngine,
            IHttpContextAccessor contextAccessor,
            IServiceProvider serviceProvider,
            ITempDataProvider tempDataProvider,
            IWebHostEnvironment hostEnvironment,
            IOptions<ViewRendererOptions> options,
            IOptions<ViewPdfRendererOptions> pdfOptions
        )
        {
            ViewEngine = viewEngine;
            ContextAccessor = contextAccessor;
            ServiceProvider = serviceProvider;
            TempDataProvider = tempDataProvider;
            HostEnvironment = hostEnvironment;
            Options = options;
            PdfOptions = pdfOptions;
        }

        public async Task<string> RenderViewToStringAsync<TModel>(
            string viewName,
            TModel model = default
        )
        {
            //using (var scope = ServiceProvider.CreateScope())
            //{
            //var httpContext = new DefaultHttpContext
            //{
            //    RequestServices = scope.ServiceProvider
            //};
            var actionContext = new ActionContext(
                ContextAccessor.HttpContext,
                ContextAccessor.HttpContext.GetRouteData(),
                new ActionDescriptor()
            );

            using (var writer = new StringWriter())
            {
                var foundView = FindViewInternal(actionContext, viewName);
                var viewData = new ViewDataDictionary(
                    new EmptyModelMetadataProvider(),
                    new ModelStateDictionary()
                )
                {
                    Model = model
                };
                var viewContext = new ViewContext(
                    actionContext,
                    foundView,
                    viewData,
                    new TempDataDictionary(actionContext.HttpContext, TempDataProvider),
                    writer,
                    new HtmlHelperOptions()
                );

                await foundView.RenderAsync(viewContext);
                return writer.ToString();
            }
            //}
        }

        public async Task<string> RenderTemplateToStringAsync<TModel>(
            string templateName,
            TModel model = default
        )
        {
            return await RenderViewToStringAsync(
                Path.Combine(Options.Value.TemplateSearchPath, templateName),
                model
            );
        }

        public async Task<Stream> RenderViewAsPdfAsync<TModel>(
            string viewName,
            ViewPdfRendererSettings pdfSettings,
            TModel model = default
        )
        {
            if (pdfSettings == null)
            {
                pdfSettings = new ViewPdfRendererSettings();
            }

            var options = PdfOptions.Value;
            var htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.WebKit);
            var settings = new WebKitConverterSettings
            {
                WebKitPath = Path.Combine(
                    HostEnvironment.ContentRootPath,
                    options.WebkitPath
                ),
                AdditionalDelay = options.ResourceDelay,
                EnableBookmarks = options.GenerateBookmarks,
                EnableToc = options.GenerateToc,
                EnableHyperLink = options.EnableHyperlinks,
                EnableJavaScript = options.EnableJavascript,
                TempPath = options.TempPath ?? Path.GetTempPath(),
                EnableOfflineMode = true,
                SplitTextLines = false,
                SplitImages = false
            };

            #region pdf settings conversion
            switch (pdfSettings.MediaType)
            {
                case PdfRenderingMediaType.Print:
                    settings.MediaType = MediaType.Print;
                    break;
                case PdfRenderingMediaType.Screen:
                    settings.MediaType = MediaType.Screen;
                    break;
            }
            switch (pdfSettings.Orientation)
            {
                case PdfRenderingOrientation.Portrait:
                    settings.Orientation = PdfPageOrientation.Portrait;
                    break;
                case PdfRenderingOrientation.Landscape:
                    settings.Orientation = PdfPageOrientation.Landscape;
                    break;
            }
            switch (pdfSettings.SinglePageOption)
            {
                case PdfRenderingSinglePageOption.FixWidth:
                    settings.SinglePageLayout = SinglePageLayout.FitWidth;
                    break;
                case PdfRenderingSinglePageOption.FixHeight:
                    settings.SinglePageLayout = SinglePageLayout.FitHeight;
                    break;
                case PdfRenderingSinglePageOption.None:
                    settings.SinglePageLayout = SinglePageLayout.None;
                    break;
            }
            #endregion

            htmlConverter.ConverterSettings = settings;

            var viewString = await RenderViewToStringAsync(viewName, model);
            var document = htmlConverter.Convert(viewString, HostEnvironment.ContentRootPath);
            var stream = new MemoryStream();
            document.Save(stream);

            stream.Position = 0;
            document.Close(true);

            return stream;
        }

        private IView FindViewInternal(ActionContext actionContext, string viewName)
        {
            var getViewResult = ViewEngine.GetView(executingFilePath: null, viewPath: viewName, isMainPage: true);
            if (getViewResult.Success)
            {
                return getViewResult.View;
            }

            var findViewResult = ViewEngine.FindView(actionContext, viewName, isMainPage: true);
            if (findViewResult.Success)
            {
                return findViewResult.View;
            }

            var searchedLocations = getViewResult.SearchedLocations.Concat(findViewResult.SearchedLocations);
            var errorMessage = string.Join(
                Environment.NewLine,
                new[] { $"Unable to find view '{viewName}'. The following locations were searched:" }.Concat(searchedLocations));

            throw new InvalidOperationException(errorMessage);
        }
    }
}
