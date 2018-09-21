using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

using DancingGoat.Controllers;
using KenticoCloud.Delivery;
using KenticoCloud.Delivery.InlineContentItems;
using Newtonsoft.Json.Linq;

namespace DancingGoat.Helpers
{
    public class SampleDeliveryClient : IDeliveryClient
    {
        private readonly DeliveryClient _deliveryClient;
        private readonly IQueryParameter[] _sampleQueryParameter = { new SampleSiteParameter() };
        private readonly string[] _sampleParameter = { new SampleSiteParameter().GetQueryStringParameter() };

        public SampleDeliveryClient(HttpContext httpContext)
        {
            _deliveryClient = ControllerBase.CreateDeliveryClient(httpContext);
        }

        public Task<JObject> GetItemJsonAsync(string codename, params string[] parameters)
        {
            var p = AddSampleSiteParameter(parameters);
            return _deliveryClient.GetItemJsonAsync(codename, p);
        }

        public Task<JObject> GetItemsJsonAsync(params string[] parameters)
        {
            var p = AddSampleSiteParameter(parameters);
            return _deliveryClient.GetItemsJsonAsync(p);
        }

        public Task<DeliveryItemResponse> GetItemAsync(string codename, params IQueryParameter[] parameters)
        {
            var p = AddSampleSiteParameter(parameters);
            return _deliveryClient.GetItemAsync(codename, p);
        }

        public Task<DeliveryItemResponse<T>> GetItemAsync<T>(string codename, params IQueryParameter[] parameters)
        {
            var p = AddSampleSiteParameter(parameters);
            return _deliveryClient.GetItemAsync<T>(codename, p);
        }

        public Task<DeliveryItemResponse> GetItemAsync(string codename, IEnumerable<IQueryParameter> parameters)
        {
            var p = AddSampleSiteParameter(parameters);
            return _deliveryClient.GetItemAsync(codename, p);
        }

        public Task<DeliveryItemResponse<T>> GetItemAsync<T>(string codename, IEnumerable<IQueryParameter> parameters = null)
        {
            var p = AddSampleSiteParameter(parameters);
            return _deliveryClient.GetItemAsync<T>(codename, p);
        }

        public Task<DeliveryItemListingResponse> GetItemsAsync(params IQueryParameter[] parameters)
        {
            var p = AddSampleSiteParameter(parameters);
            return _deliveryClient.GetItemsAsync(p);
        }

        public Task<DeliveryItemListingResponse> GetItemsAsync(IEnumerable<IQueryParameter> parameters)
        {
            var p = AddSampleSiteParameter(parameters);
            return _deliveryClient.GetItemsAsync(p);
        }

        public Task<DeliveryItemListingResponse<T>> GetItemsAsync<T>(params IQueryParameter[] parameters)
        {
            var p = AddSampleSiteParameter(parameters);
            return _deliveryClient.GetItemsAsync<T>(p);
        }

        public Task<DeliveryItemListingResponse<T>> GetItemsAsync<T>(IEnumerable<IQueryParameter> parameters)
        {
            var p = AddSampleSiteParameter(parameters);
            return _deliveryClient.GetItemsAsync<T>(p);
        }

        public Task<JObject> GetTypeJsonAsync(string codename)
        {
            var eq = new EqualsFilter("system.codename", codename).GetQueryStringParameter();
            var p = AddSampleSiteParameter(eq);
            return _deliveryClient.GetTypesJsonAsync(p);
        }

        public Task<JObject> GetTypesJsonAsync(params string[] parameters)
        {
            var p = AddSampleSiteParameter(parameters);
            return _deliveryClient.GetTypesJsonAsync(p);
        }

        public Task<ContentType> GetTypeAsync(string codename)
        {
            return Task.FromResult(_deliveryClient.GetTypesAsync(_sampleQueryParameter).Result.Types.FirstOrDefault(x=>x.System.Codename == codename));
        }

        public Task<DeliveryTypeListingResponse> GetTypesAsync(params IQueryParameter[] parameters)
        {
            var p = AddSampleSiteParameter(parameters);
            return _deliveryClient.GetTypesAsync(p);
        }

        public Task<DeliveryTypeListingResponse> GetTypesAsync(IEnumerable<IQueryParameter> parameters)
        {
            var p = AddSampleSiteParameter(parameters);
            return _deliveryClient.GetTypesAsync(p);
        }

        public Task<ContentElement> GetContentElementAsync(string contentTypeCodename, string contentElementCodename)
        {
            return _deliveryClient.GetContentElementAsync(contentTypeCodename, contentElementCodename);
        }

        public Task<JObject> GetTaxonomyJsonAsync(string codename)
        {
            var tempArray = AddSampleSiteParameter(codename);
            return _deliveryClient.GetTaxonomiesJsonAsync(tempArray);
        }

        public Task<JObject> GetTaxonomiesJsonAsync(params string[] parameters)
        {
            var p = AddSampleSiteParameter(parameters);
            return _deliveryClient.GetItemsJsonAsync(p);
        }

        public async Task<TaxonomyGroup> GetTaxonomyAsync(string codename)
        {
            var taxonomies = await GetTaxonomiesAsync(_sampleQueryParameter);
            return taxonomies.Taxonomies.FirstOrDefault(x => x.System.Codename == codename);
        }

        public Task<DeliveryTaxonomyListingResponse> GetTaxonomiesAsync(params IQueryParameter[] parameters)
        {
            var p = AddSampleSiteParameter(parameters);
            return _deliveryClient.GetTaxonomiesAsync(p);
        }

        public async Task<DeliveryTaxonomyListingResponse> GetTaxonomiesAsync(IEnumerable<IQueryParameter> parameters)
        {
            var p = AddSampleSiteParameter(parameters);
            return await _deliveryClient.GetTaxonomiesAsync(p);
        }

        public IContentLinkUrlResolver ContentLinkUrlResolver
        {
            get { return _deliveryClient.ContentLinkUrlResolver; }
            set { _deliveryClient.ContentLinkUrlResolver = value; }
        }

        public ICodeFirstModelProvider CodeFirstModelProvider
        {
            get { return _deliveryClient.CodeFirstModelProvider; }
            set { _deliveryClient.CodeFirstModelProvider = value; }
        }

        public IInlineContentItemsProcessor InlineContentItemsProcessor
        {
            get { return _deliveryClient.InlineContentItemsProcessor; }
        }

        private string[] AddSampleSiteParameter(string[] parameters)
        {
            var p = parameters == null ? _sampleParameter : _sampleParameter.Concat(parameters).ToArray();
            return p;
        }

        private string[] AddSampleSiteParameter(string codename)
        {
            var length = _sampleParameter.Length;
            string[] tempArray = new string[length + 1];
            Array.Copy(_sampleParameter, tempArray, length);
            tempArray[length] = codename;
            return tempArray;
        }

        private IEnumerable<IQueryParameter> AddSampleSiteParameter(IEnumerable<IQueryParameter> parameters)
        {
            var p = parameters == null ? _sampleQueryParameter : _sampleQueryParameter.Concat(parameters);
            return p;
        }
    }
}