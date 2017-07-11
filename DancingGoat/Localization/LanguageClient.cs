using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KenticoCloud.Delivery;
using KenticoCloud.Delivery.InlineContentItems;
using Newtonsoft.Json.Linq;

namespace DancingGoat.Localization
{
    public class LanguageClient : IDeliveryClient {

        private readonly DeliveryClient _client;
        private readonly string _language;

        public LanguageClient(DeliveryClient client, string language)
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
            return parameters != null ? _client.GetItemAsync<T>(codename, new[] { new LanguageParameter(_language) }.Concat(parameters)) : 
                _client.GetItemAsync<T>(codename, new LanguageParameter(_language));
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

        public Task<ContentType> GetTypeAsync(string codename)
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

        public Task<ContentElement> GetContentElementAsync(string contentTypeCodename, string contentElementCodename)
        {

            return _client.GetContentElementAsync(contentTypeCodename, contentElementCodename);
        }

        public IContentLinkUrlResolver ContentLinkUrlResolver
        {
            get { return _client.ContentLinkUrlResolver; }
            set { _client.ContentLinkUrlResolver = value; }
        }

        public ICodeFirstModelProvider CodeFirstModelProvider
        {
            get { return _client.CodeFirstModelProvider; }
            set { _client.CodeFirstModelProvider = value; }
        }
        public InlineContentItemsProcessor InlineContentItemsProcessor => _client.InlineContentItemsProcessor;
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
    }
}