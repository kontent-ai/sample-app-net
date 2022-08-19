﻿using System.Linq;
using System.Threading.Tasks;
using DancingGoat.Models;
using Microsoft.AspNetCore.Mvc;
using AspNetCore.Mvc.Routing.Localization.Attributes;
using Kontent.Ai.Delivery.Abstractions;
using DancingGoat.Configuration;
using Kontent.Ai.Urls.Delivery.QueryParameters;

namespace DancingGoat.Controllers
{
    [LocalizedRoute(Constants.EnglishCulture, "Home")]
    [LocalizedRoute(Constants.SpanishCulture, "Inicio")]
    public class HomeController : ControllerBase
    {
        public HomeController(IDeliveryClientFactory deliveryClientFactory) : base(deliveryClientFactory)
        {
        }

        [LocalizedRoute(Constants.EnglishCulture, "Index")]
        [LocalizedRoute(Constants.SpanishCulture, "Index")]
        public async Task<ActionResult> Index()
        {
            var response = await _client.GetItemAsync<Home>("home", new LanguageParameter(Language), new DepthParameter(1));

            var viewModel = new HomeViewModel
            {
                ContentItem = response.Item,
                Header = response.Item.HeroUnit.Cast<HeroUnit>().FirstOrDefault(x => x.System.Codename == "home_page_hero_unit"),
            };

            return View(viewModel);
        }
    }
}