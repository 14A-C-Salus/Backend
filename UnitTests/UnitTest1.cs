using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Salus.Data;
using Salus.Services;
using Salus.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;
//https://stackoverflow.com/questions/44336489/moq-iserviceprovider-iservicescope
public class ApplicationControllerTests : IClassFixture<DietService>
{
    private readonly HttpClient _client;
    private readonly DataContext _db;
    private readonly DietService _dietService;

    public ApplicationControllerTests(
         DietService dietService)
    {
        _dietService = dietService;
        _client = _dietService.CreateClient();
        var scope = _dietService.GetRequiredService<IServiceScopeFactory>().CreateScope();
        _db = scope.ServiceProvider.GetRequiredService<DataContext>();
    }

    [Fact]
    public async Task CreateApplication_WhenCalled_ShouldReturnSuccess()
    {
        var request = new CreateApplicationRequest
        {
            Name = "App1"
        }



        var response = await _client.PostAsync("/api/v1/Application/CreateApplicationForLive", request.ToJsonStringContent());

        response.EnsureSuccess();

        var applicationId = await response.Deserialize<Guid>();

        var application = _db.Set<Application>().Find(applicationId);
        Assert.Equal(request.Name, application.Name);
    }
}