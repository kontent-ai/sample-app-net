using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DancingGoat.Infrastructure
{
    public class SelfConfigActionFilterAttribute : ActionFilterAttribute
    {


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            bool selfConfigured;
            bool selfConfiguredParsed = bool.TryParse(ConfigurationManager.AppSettings["SelfConfigured"], out selfConfigured);
            DateTime subscriptionExpiresAt;
            bool subscriptionExpiresAtParsed = DateTime.TryParse(ConfigurationManager.AppSettings["SubscriptionExpiresAt"], out subscriptionExpiresAt);

            if (
                    selfConfiguredParsed &&
                    (
                        !selfConfigured ||
                        (
                            selfConfigured && subscriptionExpiresAtParsed && subscriptionExpiresAt <= DateTime.Now
                        )
                    )
               )
            {
            }

            if (selfConfiguredParsed && subscriptionExpiresAtParsed)
            {
                if (!selfConfigured)
                {
                    filterContext.Result = Helpers.RedirectHelpers.GetSelfConfigRedirectResult();

                }
                else if (subscriptionExpiresAt <= DateTime.Now)
                {
                    filterContext.Result = Helpers.RedirectHelpers.GetSelfConfigRecheckResult();
                }

            }
            else
            {
                // Error message: Could not determine if the app has already configured itself.
            }
        }
    }
}