using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Services
{
    public enum PdfRenderingMediaType
    {
        Screen,
        Print
    }

    public enum PdfRenderingOrientation
    {
        Portrait,
        Landscape
    }

    public enum PdfRenderingSinglePageOption
    {
        FixWidth,
        FixHeight,
        None
    }

    public class ViewPdfRendererSettings
    {
        public PdfRenderingMediaType MediaType { get; set; } = PdfRenderingMediaType.Print;
        public PdfRenderingOrientation Orientation { get; set; } = PdfRenderingOrientation.Portrait;
        public PdfRenderingSinglePageOption SinglePageOption { get; set; } = PdfRenderingSinglePageOption.FixWidth;
    }

    public interface IViewRendererService
    {
        Task<string> RenderViewToStringAsync<TModel>(
            string viewName,
            TModel model = default
        );

        Task<string> RenderTemplateToStringAsync<TModel>(
            string templateName,
            TModel model = default
        );

        Task<Stream> RenderViewAsPdfAsync<TModel>(
            string viewName,
            ViewPdfRendererSettings settings,
            TModel model = default
        );
    }
}
