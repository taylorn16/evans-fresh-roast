using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Refit;
using WebApp.Dto;
using Xunit;

namespace WebApp.RegressionTests
{
    public sealed class CoffeeRoastingEventTests : IClassFixture<TestWebApplicationFactory<Startup>>
    {
        private readonly IEvansFreshRoastApi _api;

        public CoffeeRoastingEventTests(TestWebApplicationFactory<Startup> webApplicationFactory)
        {
            var httpClient = webApplicationFactory.CreateClient();
            _api = RestService.For<IEvansFreshRoastApi>(httpClient);
        }

        [Fact]
        public void Post_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            var reqest = new CoffeeRoastingEventPostRequest();
            // Act
            Func<Task> act = async () => await _api.CreateEvent(reqest);
            // Assert
            act.Should().Throw<ValidationApiException>().And.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
