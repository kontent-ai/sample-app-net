namespace AspNetCore.Mvc.Routing.Localization.Models
{
    public sealed class RouteInformation
    {
        public string Controller { get; set; }

        public string Action { get; set; }

        public static RouteInformation Create(string controller, string action)
            => new RouteInformation
            {
                Controller = controller,
                Action = action
            };
    }
}
