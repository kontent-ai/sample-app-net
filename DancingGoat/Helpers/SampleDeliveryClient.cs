using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DancingGoat.Controllers;
using DancingGoat.Models;
using DancingGoat.Utils;
using KenticoCloud.Delivery;
using KenticoCloud.Delivery.InlineContentItems;
using Newtonsoft.Json.Linq;

namespace DancingGoat.Helpers
{
    public class SampleDeliveryClient : IDeliveryClient
    {
        private DeliveryClient _deliveryClient;
        private readonly List<SampleSiteFilter> _sampleSiteFilter = new List<SampleSiteFilter> { new SampleSiteFilter() };

        public SampleDeliveryClient()
        {
            // Use the provider to get environment variables.
            var provider = new ConfigurationManagerProvider();

            // Build DeliveryOptions with default or explicit values.
            var options = provider.GetDeliveryOptions();

            options.ProjectId = ProjectUtils.GetProjectId();

            _deliveryClient = new DeliveryClient(options);
            _deliveryClient.CodeFirstModelProvider.TypeProvider = new CustomTypeProvider();
            _deliveryClient.ContentLinkUrlResolver = new CustomContentLinkUrlResolver();
        }

        public Task<JObject> GetItemJsonAsync(string codename, params string[] parameters)
        {
            parameters.ToList().Add(_sampleSiteFilter.FirstOrDefault().GetQueryStringParameter());
            return _deliveryClient.GetItemJsonAsync(codename, parameters);
        }

        public Task<JObject> GetItemsJsonAsync(params string[] parameters)
        {
            parameters.ToList().Add(_sampleSiteFilter.FirstOrDefault().GetQueryStringParameter());
            return _deliveryClient.GetItemsJsonAsync(parameters);
        }

        public Task<DeliveryItemResponse> GetItemAsync(string codename, params IQueryParameter[] parameters)
        {
            var p = parameters.Concat(_sampleSiteFilter);
            return _deliveryClient.GetItemAsync(codename, p);
        }

        public Task<DeliveryItemResponse<T>> GetItemAsync<T>(string codename, params IQueryParameter[] parameters)
        {
            var p = parameters.Concat(_sampleSiteFilter);
            return _deliveryClient.GetItemAsync<T>(codename, p);
        }

        public Task<DeliveryItemResponse> GetItemAsync(string codename, IEnumerable<IQueryParameter> parameters)
        {
            parameters.ToList().Add(_sampleSiteFilter.FirstOrDefault());
            return _deliveryClient.GetItemAsync(codename, parameters);
        }

        public Task<DeliveryItemResponse<T>> GetItemAsync<T>(string codename, IEnumerable<IQueryParameter> parameters = null)
        {
            var p = parameters == null ? _sampleSiteFilter : parameters.Concat(_sampleSiteFilter);
            return _deliveryClient.GetItemAsync<T>(codename, p);
        }

        public Task<DeliveryItemListingResponse> GetItemsAsync(params IQueryParameter[] parameters)
        {
            var p = parameters.Concat(_sampleSiteFilter);
            return _deliveryClient.GetItemsAsync(p);
        }

        public Task<DeliveryItemListingResponse> GetItemsAsync(IEnumerable<IQueryParameter> parameters)
        {
            var p = parameters.Concat(_sampleSiteFilter);
            return _deliveryClient.GetItemsAsync(p);
        }

        public Task<DeliveryItemListingResponse<T>> GetItemsAsync<T>(params IQueryParameter[] parameters)
        {
            var p = parameters.Concat(_sampleSiteFilter);
            return _deliveryClient.GetItemsAsync<T>(p);
        }

        public Task<DeliveryItemListingResponse<T>> GetItemsAsync<T>(IEnumerable<IQueryParameter> parameters)
        {
            var p = parameters.Concat(_sampleSiteFilter);
            return _deliveryClient.GetItemsAsync<T>(p);
        }

        public Task<JObject> GetTypeJsonAsync(string codename)
        {
            throw new NotImplementedException();
        }

        public Task<JObject> GetTypesJsonAsync(params string[] parameters)
        {
            parameters.ToList().Add(_sampleSiteFilter.FirstOrDefault().GetQueryStringParameter());
            return _deliveryClient.GetTypesJsonAsync(parameters);
        }

        public Task<ContentType> GetTypeAsync(string codename)
        {
            throw new NotImplementedException();
        }

        public Task<DeliveryTypeListingResponse> GetTypesAsync(params IQueryParameter[] parameters)
        {
            var p = parameters.Concat(_sampleSiteFilter);
            return _deliveryClient.GetTypesAsync(p);
        }

        public Task<DeliveryTypeListingResponse> GetTypesAsync(IEnumerable<IQueryParameter> parameters)
        {
            var p = parameters.Concat(_sampleSiteFilter);
            return _deliveryClient.GetTypesAsync(p);
        }

        public Task<ContentElement> GetContentElementAsync(string contentTypeCodename, string contentElementCodename)
        {
            throw new NotImplementedException();
        }

        public Task<JObject> GetTaxonomyJsonAsync(string codename)
        {
            throw new NotImplementedException();
        }

        public Task<JObject> GetTaxonomiesJsonAsync(params string[] parameters)
        {
            parameters.ToList().Add(_sampleSiteFilter.FirstOrDefault().GetQueryStringParameter());
            return _deliveryClient.GetItemsJsonAsync(parameters);
        }

        public async Task<TaxonomyGroup> GetTaxonomyAsync(string codename)
        {
            var taxonomies = await GetTaxonomiesAsync(_sampleSiteFilter);
            return taxonomies.Taxonomies.FirstOrDefault(x => x.System.Codename == Brewer.ProductStatusCodename);
        }

        public Task<DeliveryTaxonomyListingResponse> GetTaxonomiesAsync(params IQueryParameter[] parameters)
        {
            var p = parameters.Concat(_sampleSiteFilter);
            return _deliveryClient.GetTaxonomiesAsync(p);
        }

        public async Task<DeliveryTaxonomyListingResponse> GetTaxonomiesAsync(IEnumerable<IQueryParameter> parameters)
        {
            var p = parameters.Concat(_sampleSiteFilter);
            return await _deliveryClient.GetTaxonomiesAsync(p);
        }

        public IContentLinkUrlResolver ContentLinkUrlResolver { get; set; }
        public ICodeFirstModelProvider CodeFirstModelProvider { get; set; }
        public IInlineContentItemsProcessor InlineContentItemsProcessor { get; }
    }
}