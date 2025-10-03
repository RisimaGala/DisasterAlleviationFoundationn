using Xunit;
using System.Net.Http;
using System.Threading.Tasks;

namespace DisasterAlleviationFoundationn.Tests.UI
{
    public class HttpTests
    {
        private readonly HttpClient _client;

        public HttpTests()
        {
            _client = new HttpClient();
        }

        [Fact]
        public async Task Website_IsAccessible()
        {
            // Note: This test requires your website to be running
            // Arrange
            var url = "https://localhost:7000/"; // Change to your actual URL

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            Assert.True(response.IsSuccessStatusCode, $"Website returned {response.StatusCode}");
        }
    }
}
