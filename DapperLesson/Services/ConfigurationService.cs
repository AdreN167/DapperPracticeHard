using Microsoft.Extensions.Configuration;

namespace DapperLesson.Services
{
    public static class ConfigurationService
    {
        public static IConfigurationRoot Configuration { get; private set; }

        public static void Init()
        {
            if (Configuration == null)
            {
                var configurationBuilder = new ConfigurationBuilder();
                Configuration = configurationBuilder.AddJsonFile("appSettings.json").Build();
            }
        }
    }
}
