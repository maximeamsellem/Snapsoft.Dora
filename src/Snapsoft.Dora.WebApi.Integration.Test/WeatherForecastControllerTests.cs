using Flurl.Http;
using System.Net;

namespace Snapsoft.Dora.WebApi.Integration.Test
{
    public class WeatherForecastControllerTests
    {
        [Fact]
        public async Task GetWeatherForecast_Returns_Ok()
        {
            // Arrange
            var getWeatherForecastHttpRequest = BuildGetWeatherForecastHttpRequest();

            // Act
            var actual = await getWeatherForecastHttpRequest.GetAsync();

            // Assert
            Assert.Equal(actual?.StatusCode, (int)HttpStatusCode.OK);
        }
                
        [Fact]
        public async Task GetWeatherForecast_Returns_Summaries()
        {
            // Arrange
            var getWeatherForecastHttpRequest = BuildGetWeatherForecastHttpRequest();

            // Act
            var actual = await getWeatherForecastHttpRequest.GetJsonAsync<IEnumerable<WeatherForecast>>();

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(5, actual.Count());
        }

        private static IFlurlRequest BuildGetWeatherForecastHttpRequest()
        {
            return new FlurlClient(ServiceFactory.Instance.CreateClient())
                .Request("WeatherForecast")
                .AllowAnyHttpStatus();
        }
    }
}