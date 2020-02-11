using DancingGoat.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

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
                message?.MessageType
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
                message?.MessageType
            }));
        }
    }
}