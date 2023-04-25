using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Salus.Models.Requests;
using Salus.Services.Interfaces;

namespace UnitTests
{
    //https://stackoverflow.com/questions/44336489/moq-iserviceprovider-iservicescope
    public class UnitTest1
    {

        [Fact]
        public async void Test1()
        {
            var service = new Mock<IDietService>();
            var req = new CreateDietRequest() { description = "Its a new diet"};
            var description = service.Object.Create(req).description;
            Assert.Equal(description, req.description);
        }
        Mock<IServiceProvider> CreateScopedServicesProvider(params (Type @interface, Object service)[] services)
        {
            var scopedServiceProvider = new Mock<IServiceProvider>();

            foreach (var (@interfcae, service) in services)
            {
                scopedServiceProvider
                    .Setup(s => s.GetService(@interfcae))
                    .Returns(service);
            }

            var scope = new Mock<IServiceScope>();
            scope
                .SetupGet(s => s.ServiceProvider)
                .Returns(scopedServiceProvider.Object);

            var serviceScopeFactory = new Mock<IServiceScopeFactory>();
            serviceScopeFactory
                .Setup(x => x.CreateScope())
                .Returns(scope.Object);

            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(s => s.GetService(typeof(IServiceScopeFactory)))
                .Returns(serviceScopeFactory.Object);

            return serviceProvider;
        }
    }
}