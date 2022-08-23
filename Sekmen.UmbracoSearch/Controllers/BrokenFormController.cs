using Microsoft.AspNetCore.Mvc;
using TheUmbracoSite.Models;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace TheUmbracoSite.Controllers
{
    public class BrokenController : SurfaceController
    {
        public BrokenController(
            IUmbracoContextAccessor umbracoContextAccessor,
            IUmbracoDatabaseFactory databaseFactory,
            ServiceContext services,
            AppCaches appCaches,
            IProfilingLogger profilingLogger,
            IPublishedUrlProvider publishedUrlProvider)
            : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
        }

        public IActionResult Submit(BrokenFormModel model)
        {

            //In cases the model does not validate
            if (ModelState.IsValid == false)
                return CurrentUmbracoPage();

            //this will explode when = 0
            var result = 1000 / model.ANumber;
            return RedirectToCurrentUmbracoPage();
        }
    }
}
