using Microsoft.Extensions.Configuration;
using ReqResponse.Models;

namespace ReqResponse.Support
{
    public static class ConfigFactory
    {
        private static IConfiguration _configuration = null;

        public static Options GetOptions()
        {
            Options options = new Options();

            EmailConfiguration emailConfig;
            ServerConfiguration serverConfig;
            ConfigHelper. CreateConfigurations(_configuration,
                                                out emailConfig,
                                                out serverConfig);
            options.SetServer(serverConfig, true);
            return options;
        }

        public static void SetConfiguration( IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}