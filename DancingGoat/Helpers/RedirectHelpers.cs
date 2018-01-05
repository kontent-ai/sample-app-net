using System.Web.Mvc;
using System.Web.Routing;

using DancingGoat.Areas.Admin.Models;

namespace DancingGoat.Helpers
{
    public static class RedirectHelpers
    {
        public static RedirectToRouteResult GetSelfConfigIndexResult(MessageModel message)
        {
            return new RedirectToRouteResult(new RouteValueDictionary(new
            {
                Action = "Index",
                Controller = "SelfConfig",
                Area = "Admin",
                MessageBody = message?.Message,
                MessageType = message?.MessageType
            }));
        }

        public static RedirectToRouteResult GetHomeRedirectResult(MessageModel message)
        {
            return new RedirectToRouteResult(new RouteValueDictionary(new
            {
                Action = "Index",
                Controller = "Home",
                Area = "",
                MessageBody = message?.Message,
                MessageType = message?.MessageType
            }));
        }
    }
}