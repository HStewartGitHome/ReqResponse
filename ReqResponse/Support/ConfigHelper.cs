using Microsoft.Extensions.Configuration;
using ReqResponse.Models;

namespace ReqResponse.Support
{
    public class ConfigHelper
    {
        public static IConfiguration CreateConfiguration(string fileName,
                                                         out EmailConfiguration emailConfig,
                                                         out ServerConfiguration serverConfig)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(fileName)
                .Build();

            CreateConfigurations(configuration,
                                        out emailConfig,
                                        out serverConfig);
 

            return configuration;
        }

        public static void CreateConfigurations(IConfiguration configuration,
                                                      out EmailConfiguration emailConfig,
                                                      out ServerConfiguration serverConfig)
        {
            
            emailConfig = configuration
                  .GetSection("EmailConfiguration").Get<EmailConfiguration>();


            serverConfig = configuration
                    .GetSection("ServerConfiguration").Get<ServerConfiguration>();
            serverConfig.OnPrimary = true;

        }
    }
}
