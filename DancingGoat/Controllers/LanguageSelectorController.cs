using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DancingGoat.Models;
using Kentico.Kontent.Delivery;

namespace DancingGoat.Controllers
{
    public class LanguageSelectorController : ControllerBase
    {
        public async Task<ActionResult> Index(Guid id, string originalAction, string type, string originalController)
        {
            // Specific item is not selected, url will not be changed after redirect
            if (id == Guid.Empty || string.IsNullOrEmpty(type))
            {
                return RedirectToAction(originalAction, originalController);
            }

            var item = (await client.GetItemsAsync<object>(
                new EqualsFilter("system.type", type),
                new EqualsFilter("system.id", id.ToString()),
                new ElementsParameter("url_pattern"))).Items.FirstOrDefault();

            if (!(item is IDetailItem detaiItem))
            {
                throw new HttpException(404, "Not found");
            }

            return RedirectToAction(originalAction, originalController, new { urlSlug = detaiItem.UrlPattern });
        }
    }
}