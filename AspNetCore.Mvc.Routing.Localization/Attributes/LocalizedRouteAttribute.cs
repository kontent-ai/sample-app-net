using System;

namespace AspNetCore.Mvc.Routing.Localization.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class LocalizedRouteAttribute : Attribute
    {
        public LocalizedRouteAttribute(string culture, string template)
        {
            Culture = culture;
            Template = template;
        }

        public string Culture { get; }

        public string Template { get; }
    }
}
