using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using SAS.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SAS.Services
{
    public class TemplateHelper : ITemplateHelper
    {
        private IRazorViewEngine razorViewEngine;
        private IServiceProvider serviceProvider;
        private ITempDataProvider tempDataProvider;

        public TemplateHelper(
            IRazorViewEngine engine,
            IServiceProvider serviceProvider,
            ITempDataProvider tempDataProvider)
        {
            this.razorViewEngine = engine;
            this.serviceProvider = serviceProvider;
            this.tempDataProvider = tempDataProvider;
        }

        public async Task<string> GetTemplateHtmlAsStringAsync<T>(
                string viewName, T model)
        {

            var httpContext = new DefaultHttpContext()
            {
                RequestServices = serviceProvider
            };

            var actionContext = new ActionContext(
                    httpContext, new RouteData(), new ActionDescriptor());

            using (StringWriter sw = new StringWriter())
            {
                var viewResult = razorViewEngine.FindView(
                        actionContext, 
                        viewName, 
                        true);

                if (viewResult.View == null)
                {
                    return string.Empty;
                }

                var viewDataDictionary = new ViewDataDictionary(
                    new EmptyModelMetadataProvider(),
                    new ModelStateDictionary() )
                {
                    Model = model
                };

                var viewContext = new ViewContext(
                    actionContext,
                    viewResult.View,
                    viewDataDictionary,
                    new TempDataDictionary(
                        actionContext.HttpContext, tempDataProvider),
                    sw,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);
                return sw.ToString();
            }
        }


    }
}
