using Kentico.Kontent.Delivery.Abstractions;
using Kentico.Kontent.Delivery.Builders.DeliveryClient;

namespace DancingGoat.Utils
{
    public class DomainBasedDeliveryClientFactory : IDeliveryClientFactory
    {
        private readonly IDeliveryClientFactory _baseClientFactory;
        private readonly ProjectContext _projectContext;
        private readonly ITypeProvider _typeProvider;
        private readonly IContentLinkUrlResolver _linkResolver;

        public DomainBasedDeliveryClientFactory(
            IDeliveryClientFactory baseClientFactory,
            ProjectContext projectContext,
            ITypeProvider typeProvider,
            IContentLinkUrlResolver linkResolver
        )
        {
            _baseClientFactory = baseClientFactory;
            _projectContext = projectContext;
            _typeProvider = typeProvider;
            _linkResolver = linkResolver;
        }

        public IDeliveryClient Get()
            => DeliveryClientBuilder.WithOptions(builder =>
                    builder
                    .WithProjectId(_projectContext.ProjectGuid)
                    .SelectApi(_projectContext)
                    .Build())
                .WithTypeProvider(_typeProvider)
                .WithContentLinkUrlResolver(_linkResolver)
                .Build();

        public IDeliveryClient Get(string name)
            => _baseClientFactory.Get(name);
    }
}
