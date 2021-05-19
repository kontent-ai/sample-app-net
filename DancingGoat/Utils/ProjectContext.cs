using System;
using Kentico.Kontent.Delivery;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace DancingGoat.Utils
{
    public class ProjectContext
    {
        private const string AppSettingPreviewApiKey = "PreviewApiKey";
        private const string PreviewApiKeyCookieName = "PreviewApiKey";

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        private HttpContext HttpContext => _httpContextAccessor.HttpContext;

        public string ProjectGuid => GetProjectGuid();

        public bool IsInPreviewMode => GetIsInPreviewMode();

        public string PreviewApiKey => GetPreviewApiKey();

        public ProjectContext(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        private string GetProjectGuid()
        {
            var subdomain = HttpContext.Request.Host.GetSubdomain();
            if (Guid.TryParseExact(subdomain, "N", out Guid decodedGuid))
            {
                return decodedGuid.ToString();
            }

            var configProjectId = _configuration.GetSection(nameof(DeliveryOptions))[nameof(DeliveryOptions.ProjectId)].ToString() ?? _configuration.GetSection(nameof(AppConfiguration)).Get<AppConfiguration>().DefaultProjectId.ToString();
            if (Guid.TryParse(configProjectId, out Guid projectId))
            {
                return projectId.ToString();
            }

            throw new InvalidOperationException($"Invalid project ID, tried subdomain '{subdomain}' and config value `{configProjectId}`.");
        }

        private string GetPreviewApiKey()
        {
            var configPreviewApiKey = _configuration.GetSection(nameof(DeliveryOptions))[nameof(DeliveryOptions.PreviewApiKey)];
            if (!string.IsNullOrEmpty(configPreviewApiKey))
            {
                return configPreviewApiKey;
            }

            var cookies = HttpContext.Request.Cookies;
            if (cookies.TryGetValue(PreviewApiKeyCookieName, out var previewApiKeyCookieValue))
            {
                return previewApiKeyCookieValue;
            }
            return string.Empty;
        }

        private bool GetIsInPreviewMode()
        {
            var configUsePreviewApi = _configuration.GetSection(nameof(DeliveryOptions))[nameof(DeliveryOptions.UsePreviewApi)];

            bool.TryParse(configUsePreviewApi, out var isPreviewFromConfig);
            if (isPreviewFromConfig) return true;

            var isPreviewModeEnabledCookieName = "IsPreviewMode";
            var cookies = HttpContext.Request.Cookies;
            if (cookies.TryGetValue(isPreviewModeEnabledCookieName, out var isInPreviewCookieValue))
            {
                return (isInPreviewCookieValue == "true");
            }
            return false;
        }
    }
}
