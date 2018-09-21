using System;
using System.Configuration;
using System.Linq;
using System.Web;

namespace DancingGoat.Utils
{
    public class ProjectUtils
    {
        private static string AppSettingPreviewApiKey = "PreviewApiKey";
        private static string PreviewApiKeyCookieName = "PreviewApiKey";

        public static string GetProjectId()
        {
            // check web.config first
            if (Guid.TryParse(ConfigurationManager.AppSettings["ProjectId"], out Guid projectId))
            {
                return projectId.ToString();
            }

            // get projectId from subdomain
            var requestUrl = System.Web.HttpContext.Current.Request.Url;
            var subdomain = requestUrl.GetSubdomain();

            if (!Guid.TryParseExact(subdomain, "N", out Guid decodedGuid))
            {
                throw new InvalidOperationException(nameof(decodedGuid));
            }

            return decodedGuid.ToString();
        }

        public static string GetPreviewApiKey(HttpContext httpContext)
        {
            // check web.config first
            if (ConfigurationManager.AppSettings.AllKeys.Contains(AppSettingPreviewApiKey))
            {
                var previewApiKey = ConfigurationManager.AppSettings[AppSettingPreviewApiKey];

                return previewApiKey;
            }


            // check preview api key from cookie
            var cookies = httpContext.Request.Cookies;

            if (cookies.AllKeys.Contains(PreviewApiKeyCookieName) && !String.IsNullOrEmpty(cookies[PreviewApiKeyCookieName].Value))
            {
                var previewApiKeyCookieValue = cookies.Get(PreviewApiKeyCookieName).Value;
                return previewApiKeyCookieValue;
            }

            return string.Empty;
        }

        public static bool IsInPreviewMode(HttpContext httpContext)
        {
            // check web.config first
            bool.TryParse(ConfigurationManager.AppSettings["UsePreviewApi"], out var isPreviewFromConfig);

            if (isPreviewFromConfig) return true;

            // check preview mode from cookie
            var isPreviewModeEnabledCookieName = "IsPreviewMode";
            var cookies = httpContext.Request.Cookies;

            if (cookies.AllKeys.Contains(isPreviewModeEnabledCookieName) && !String.IsNullOrEmpty(cookies[isPreviewModeEnabledCookieName].Value))
            {
                var isInPreviewCookieValue = cookies.Get(isPreviewModeEnabledCookieName).Value;
                if (isInPreviewCookieValue == "true")
                {
                    return true;
                }
            }

            return false;
        }
    }
}