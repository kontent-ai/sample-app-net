using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kentico.Kontent.Delivery;
using Newtonsoft.Json.Linq;

namespace DancingGoat.Localization
{
    public class LanguageClient : IDeliveryClient {
        public const string DEFAULT_LANGUAGE = "en-us";
        public const string SpanishLanguge = "es-es";
        private readonly IDeliveryClient _client;
        private readonly string _language;

        public LanguageClient(IDeliveryClient client, string language)
        {
            _client = client;
            _language = language;
        }

        public Task<DeliveryItemResponse<T>> GetItemAsync<T>(string codename, params IQueryParameter[] parameters)
        {
            return _client.GetItemAsync<T>(codename, new[] {new LanguageParameter(_language)}.Concat(parameters));
        }

        public Task<DeliveryItemResponse> GetItemAsync(string codename, IEnumerable<IQueryParameter> parameters)
        {
            return _client.GetItemAsync(codename, new[] { new LanguageParameter(_language) }.Concat(parameters));
        }

        public Task<DeliveryItemResponse<T>> GetItemAsync<T>(string codename, IEnumerable<IQueryParameter> parameters = null)
        {
            var parametersWithLanguage = parameters != null ? new[] { new LanguageParameter(_language) }.Concat(parameters) :
            new[] { new LanguageParameter(_language)};
            return _client.GetItemAsync<T>(codename, parametersWithLanguage);
        }

        public Task<DeliveryItemListingResponse> GetItemsAsync(params IQueryParameter[] parameters)
        {
            return _client.GetItemsAsync(new[] { new LanguageParameter(_language) }.Concat(parameters));
        }

        public Task<DeliveryItemListingResponse> GetItemsAsync(IEnumerable<IQueryParameter> parameters)
        {
            return _client.GetItemsAsync(new[] { new LanguageParameter(_language) }.Concat(parameters));
        }

        public Task<DeliveryItemListingResponse<T>> GetItemsAsync<T>(params IQueryParameter[] parameters)
        {
            return _client.GetItemsAsync<T>(new[] { new LanguageParameter(_language) }.Concat(parameters));
        }

        public Task<DeliveryItemListingResponse<T>> GetItemsAsync<T>(IEnumerable<IQueryParameter> parameters)
        {
            return _client.GetItemsAsync<T>(new[] { new LanguageParameter(_language) }.Concat(parameters));
        }

        public Task<JObject> GetTypeJsonAsync(string codename)
        {
            return _client.GetTypeJsonAsync(codename);
        }

        public Task<JObject> GetTypesJsonAsync(params string[] parameters)
        {
            return _client.GetTypesJsonAsync(new [] {new LanguageParameter(_language).GetQueryStringParameter()}.Concat(parameters).ToArray());
        }

        public Task<DeliveryTypeResponse> GetTypeAsync(string codename)
        {
            return _client.GetTypeAsync(codename);
        }

        public Task<DeliveryTypeListingResponse> GetTypesAsync(params IQueryParameter[] parameters)
        {
            return _client.GetTypesAsync(new[] { new LanguageParameter(_language) }.Concat(parameters));
        }

        public Task<DeliveryTypeListingResponse> GetTypesAsync(IEnumerable<IQueryParameter> parameters)
        {
            return _client.GetTypesAsync(new[] { new LanguageParameter(_language) }.Concat(parameters));
        }

        public Task<DeliveryElementResponse> GetContentElementAsync(string contentTypeCodename, string contentElementCodename)
        {

            return _client.GetContentElementAsync(contentTypeCodename, contentElementCodename);
        }

        public Task<JObject> GetTaxonomyJsonAsync(string codename)
        {
            return _client.GetTaxonomyJsonAsync(codename);
        }

        public Task<JObject> GetTaxonomiesJsonAsync(params string[] parameters)
        {
            return _client.GetTaxonomiesJsonAsync(parameters);
        }

        public Task<DeliveryTaxonomyResponse> GetTaxonomyAsync(string codename)
        {
            return _client.GetTaxonomyAsync(codename);
        }

        public Task<DeliveryTaxonomyListingResponse> GetTaxonomiesAsync(params IQueryParameter[] parameters)
        {
            return _client.GetTaxonomiesAsync(parameters);
        }

        public Task<DeliveryTaxonomyListingResponse> GetTaxonomiesAsync(IEnumerable<IQueryParameter> parameters)
        {
            return _client.GetTaxonomiesAsync(parameters);
        }

        public Task<JObject> GetItemJsonAsync(string codename, params string[] parameters)
        {
            return _client.GetItemJsonAsync(codename, new[] {new LanguageParameter(_language).GetQueryStringParameter()}.Concat(parameters).ToArray());
        }

        public Task<JObject> GetItemsJsonAsync(params string[] parameters)
        {
            return _client.GetItemsJsonAsync(new[] { new LanguageParameter(_language).GetQueryStringParameter() }.Concat(parameters).ToArray());
        }

        public Task<DeliveryItemResponse> GetItemAsync(string codename, params IQueryParameter[] parameters)
        {
            return _client.GetItemAsync(codename, new[] { new LanguageParameter(_language)}.Concat(parameters));
        }

        public IDeliveryItemsFeed GetItemsFeed(params IQueryParameter[] parameters)
        {
            return _client.GetItemsFeed(parameters);
        }

        public IDeliveryItemsFeed GetItemsFeed(IEnumerable<IQueryParameter> parameters)
        {
            return _client.GetItemsFeed(parameters);
        }

        public IDeliveryItemsFeed<T> GetItemsFeed<T>(params IQueryParameter[] parameters)
        {
            return _client.GetItemsFeed<T>(parameters);
        }

        public IDeliveryItemsFeed<T> GetItemsFeed<T>(IEnumerable<IQueryParameter> parameters)
        {
            return _client.GetItemsFeed<T>(parameters);
        }
    }
}