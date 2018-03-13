using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;
using DancingGoat.Models;

namespace DancingGoat.Helpers
{
    public static class LanguageSwitchHelper
    {
        public static object GetLangugageSwitchParameters(RouteValueDictionary values, string langauge, IDetailItem model)
        {
            if (values == null)
            {
                return new { };
            }

            return new
            {
                originalController = values["controller"].ToString(),
                language = langauge,
                originalAction = values["action"].ToString(),
                type = model?.Type,
                id = model?.Id
            };
        }
    }
}
