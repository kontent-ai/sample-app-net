using System;
using System.Globalization;
using System.Threading.Tasks;
using DancingGoat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Kontent.Ai.Delivery.Abstractions;
using Kontent.Ai.Urls.Delivery.QueryParameters;
using Kontent.Ai.Urls.Delivery.QueryParameters.Filters;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;

namespace DancingGoat.Controllers
{
    public class ArticlesController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ArticlesController(IConfiguration configuration, IDeliveryClientFactory deliveryClientFactory) : base(deliveryClientFactory)
        {
            _configuration = configuration;
        }
        
        public async Task<ActionResult> Index()
        {
            var response = await _client.GetItemsAsync<Article>(
                new OrderParameter("elements.post_date", SortOrder.Descending),
                new ElementsParameter(Article.TeaserImageCodename, Article.PostDateCodename, Article.SummaryCodename, Article.UrlPatternCodename, Article.TitleCodename),
                new LanguageParameter(Language)
            );

            return View(response.Items);
        }
        
        public async Task<ActionResult> Show(string urlSlug)
        {
            var response = await _client.GetItemsAsync<Article>(
                new EqualsFilter($"elements.{Article.UrlPatternCodename}", urlSlug),
                new DepthParameter(1),
                new LanguageParameter(Language)
            );

            if (response.Items[0].System.Language != Language)
            {
                var urlParts = Request.GetDisplayUrl().Split('/');
                urlParts[3] = response.Items[0].System.Language;
                var newUrl = String.Join('/', urlParts);
            
                return Redirect(newUrl);
            }

            if (response.Items.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return View(response.Items[0]);
            }
        }
    }
}