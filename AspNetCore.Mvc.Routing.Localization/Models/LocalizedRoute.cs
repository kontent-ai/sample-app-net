namespace AspNetCore.Mvc.Routing.Localization.Models
{
    public sealed class LocalizedRoute
    {
        public string Culture { get; set; }

        public RouteInformation Original { get; set; }

        public RouteInformation Translated { get; set; }

        public static LocalizedRoute Create(string culture, RouteInformation original, RouteInformation translated)
            => new LocalizedRoute
            {
                Culture = culture,
                Original = original,
                Translated = translated
            };
    }
}
