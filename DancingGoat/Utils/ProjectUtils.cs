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
            if (Guid.TryParse(ConfigurationManager.AppSettings["ProjectId"], out Guid projectId))
            {
                return projectId.ToString();
            }

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
            if (ConfigurationManager.AppSettings.AllKeys.Contains(AppSettingPreviewApiKey))
            {
                var previewApiKey = ConfigurationManager.AppSettings[AppSettingPreviewApiKey];
                return previewApiKey;
            }

            var cookies = httpContext.Request.Cookies;
            if (cookies.AllKeys.Contains(PreviewApiKeyCookieName) && !string.IsNullOrEmpty(cookies[PreviewApiKeyCookieName].Value))
            {
                var previewApiKeyCookieValue = cookies.Get(PreviewApiKeyCookieName).Value;
                return previewApiKeyCookieValue;
            }
            return string.Empty;
        }

        public static bool IsInPreviewMode(HttpContext httpContext)
        {
            bool.TryParse(ConfigurationManager.AppSettings["UsePreviewApi"], out var isPreviewFromConfig);
            if (isPreviewFromConfig) return true;

            var isPreviewModeEnabledCookieName = "IsPreviewMode";
            var cookies = httpContext.Request.Cookies;
            if (cookies.AllKeys.Contains(isPreviewModeEnabledCookieName) && !string.IsNullOrEmpty(cookies[isPreviewModeEnabledCookieName].Value))
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