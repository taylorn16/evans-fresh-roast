using System.Threading.Tasks;
using FluentAssertions;
using Refit;
using Xunit;

namespace WebApp.RegressionTests
{
    public sealed class ContactTests : IClassFixture<TestWebApplicationFactory<Startup>>
    {
        private readonly IEvansFreshRoastApi _api;

        public ContactTests(TestWebApplicationFactory<Startup> webApplicationFactory)
        {
            var httpClient = webApplicationFactory.CreateClient();
            _api = RestService.For<IEvansFreshRoastApi>(httpClient);
        }

        [Fact]
        public async Task Post_CreatesNewUnconfirmedContact()
        {
            // Arrange
            var request = new Dto.ContactPostRequest
            {
                Name = "Gangalorp Frildud",
                PhoneNumber = "+12223334444",
            };
            // Act
            var result = await _api.CreateContact(request);
            // Assert
            result.Id.Should().NotBeNull();
            result.Id.Should().NotBeEmpty();
            result.Name.Should().Be(request.Name);
            result.PhoneNumber.Should().Contain("2223334444");
            result.IsActive.Should().BeFalse();
            result.IsConfirmed.Should().BeFalse();
        }
    }
}
