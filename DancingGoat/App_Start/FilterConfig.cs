using System.Web.Mvc;
using DancingGoat.ErrorHandler;


namespace DancingGoat
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new AiHandleErrorAttribute());
        }
    }
}