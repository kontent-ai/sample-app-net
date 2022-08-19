using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;

namespace AspNetCore.Mvc.Routing.Localization
{
    public class LocalizedRoutesController : Controller
    {
        private readonly ILocalizedRoutingProvider _localizedRoutingProvider;

        public LocalizedRoutesController(ILocalizedRoutingProvider localizedRoutingProvider)
        {
            _localizedRoutingProvider = localizedRoutingProvider;
        }

        public string Index()
        {
            var routes = _localizedRoutingProvider.Routes
                    .GroupBy(s => new { s.Original.Controller, s.Original.Action })
                    .Select(s => new
                    {
                        Controller = s.Key.Controller,
                        Action = s.Key.Action,
                        Routes = s.Select(s => $"{s.Culture}/{s.Translated.Controller}/{s.Translated.Action}")
                    });

            return JsonConvert.SerializeObject(routes);
        }

    }
}
