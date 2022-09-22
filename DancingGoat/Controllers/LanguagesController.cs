using System;
using System.Linq;
using System.Threading.Tasks;
using DancingGoat.Models;
using Microsoft.AspNetCore.Mvc;
using Kontent.Ai.Delivery.Abstractions;
using Kontent.Ai.Urls.Delivery.QueryParameters;
using Kontent.Ai.Urls.Delivery.QueryParameters.Filters;

namespace DancingGoat.Controllers
{
    public class LanguagesController : ControllerBase
    {

        public LanguagesController(IDeliveryClientFactory deliveryClientFactory) : base(deliveryClientFactory)
        { }

        public async Task<ActionResult> Index([FromQuery]Guid itemId, [FromQuery]string originalAction, [FromQuery]string itemType, [FromQuery]string originalController, [FromQuery]string language)
        {
            var referer = Request.Headers["Referer"].ToString();

            var urlParts = referer.Split('/');
            urlParts[3] = language;
            var newUrl = String.Join('/', urlParts);
            
            return Redirect(newUrl);
        }
    }
}