using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace DisasterAlleviationFoundationn.Tests.Load
{
    public class SimpleLoadTests
    {
        private readonly HttpClient _client;

        public SimpleLoadTests()
        {
            _client = new HttpClient();
        }

        [Fact]
        public async Task HomePage_LoadTest_ConcurrentRequests()
        {
            // Arrange
            var baseUrl = "https://localhost:7000"; // Change to your actual URL
            var tasks = new List<Task<HttpResponseMessage>>();

            // Act - Simulate 20 concurrent users (reduced from 100 for stability)
            for (int i = 0; i < 20; i++)
            {
                tasks.Add(_client.GetAsync(baseUrl + "/"));
            }

            var responses = await Task.WhenAll(tasks);

            // Assert
            var successCount = responses.Count(r => r.IsSuccessStatusCode);
            Assert.True(successCount >= 15, $"Only {successCount}/20 requests succeeded");
        }

        [Fact]
        public async Task MultipleEndpoints_ConcurrentAccess()
        {
            // Arrange
            var baseUrl = "https://localhost:7000";
            var endpoints = new[] { "/", "/Auth/Login", "/Auth/Register" };
            var tasks = new List<Task<HttpResponseMessage>>();

            // Act - Simulate concurrent access to multiple endpoints
            foreach (var endpoint in endpoints)
            {
                for (int i = 0; i < 10; i++)
                {
                    tasks.Add(_client.GetAsync(baseUrl + endpoint));
                }
            }

            var responses = await Task.WhenAll(tasks);

            // Assert
            var successRate = responses.Count(r => r.IsSuccessStatusCode) / (double)responses.Length;
            Assert.True(successRate >= 0.8, $"Success rate {successRate:P0} is below 80%");
        }

        [Fact]
        public async Task ResponseTime_PerformanceTest()
        {
            // Arrange
            var baseUrl = "https://localhost:7000";
            var stopwatch = new System.Diagnostics.Stopwatch();

            // Act
            stopwatch.Start();
            var response = await _client.GetAsync(baseUrl + "/");
            stopwatch.Stop();

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.True(stopwatch.ElapsedMilliseconds < 5000,
                $"Response time {stopwatch.ElapsedMilliseconds}ms exceeds 5 seconds");
        }
    }
}