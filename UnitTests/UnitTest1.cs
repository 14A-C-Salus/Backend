using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests
{
    //https://stackoverflow.com/questions/44336489/moq-iserviceprovider-iservicescope
    public class UnitTest1
    {

        [Fact]
        public void Test1()
        {
            var serviceCollection = new ServiceCollection();

            // Create the ServiceProvider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // serviceScopeMock will contain my ServiceProvider
            var serviceScopeMock = new Mock<IServiceScope>();
            serviceScopeMock.SetupGet<IServiceProvider>(s => s.ServiceProvider)
                .Returns(serviceProvider);

            // serviceScopeFactoryMock will contain my serviceScopeMock
            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
            serviceScopeFactoryMock.Setup(s => s.CreateScope())
                .Returns(serviceScopeMock.Object);
        }
    }
}