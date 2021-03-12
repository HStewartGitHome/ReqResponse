using Microsoft.Extensions.Configuration;
using ReqResponse.Models;
using ReqResponse.Services.Network;
using ReqResponse.Support;
using System;

internal class Program
{
    private static void Main()
    {
        EmailConfiguration emailConfig;
        ServerConfiguration serverConfig;
        IConfiguration configuration = ConfigHelper.CreateConfiguration("appsettings.json",
                                                    out emailConfig,
                                                    out serverConfig);

        Options options = new Options();
        options.SetServer(serverConfig, true);
        ConfigFactory.SetConfiguration(configuration);
        Console.WriteLine($"Starting ReqResponse.Server on Port: {options.Port} UsePrimary: {options.UsePrimary}");
        Server.NewServer(options.Port);
    }
}