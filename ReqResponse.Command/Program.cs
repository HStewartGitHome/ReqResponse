using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReqResponse.Command.HostBuilders;
using ReqResponse.Command.Models;
using ReqResponse.Command.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace ReqResponse.Command
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CreateParameters(args);

            if (Parameters.Test == Tests.TestHelp)
            {
                Console.WriteLine("ReqResponse.Command -cmd -m");
                Console.WriteLine("Where -cmd is one of the following:");
                Console.WriteLine("   -l   Perform Local ReqResponse test");
                Console.WriteLine("   -r   Perform Remote ReqResponse test");
                Console.WriteLine("   -c   Perform Connect ReqResponse test");
                Console.WriteLine("   -s   Perform Response Summary test");
                Console.WriteLine("   -e   Perform Response Error Report test");
                Console.WriteLine("   -h   Help show this message");
                Console.WriteLine("Where -m does email if any error reports, will do -e if not provided");
                Console.WriteLine("Example:");
                Console.WriteLine("    -l -c -s -m which does local, connected, summary and then email errors");
            }
            else
            {
                IHost _host = CreateHostBuilder().Build();
                ProcessTestService service = _host.Services.GetRequiredService<ProcessTestService>();
                _host.Start();
                ProcessService(service);
                Stop(_host);
            }
        }

        public static void CreateParameters(string[] args)
        {
            bool forceHelp = false;

            Parameters.Test = Tests.TestHelp;
            Parameters.DoEmail = false;
            Parameters.TestToPerform = new List<Tests>();

            if (args.Length > 0)
            {
                Tests test;
                foreach (string arg in args)
                {
                    test = Tests.TestRequestNone;

                    switch (arg)
                    {
                        case "-l":
                            test = Tests.TestLocalRequest;
                            break;

                        case "-r":
                            test = Tests.TestRemoteRequest;
                            break;

                        case "-c":
                            test = Tests.TestConnectedRequest;
                            break;

                        case "-s":
                            test = Tests.TestSummary;
                            break;

                        case "-e":
                            test = Tests.TestErrors;
                            break;

                        case "-m":
                            Parameters.DoEmail = true;
                            break;

                        case "-h":
                        default:
                            test = Tests.TestHelp;
                            forceHelp = true;
                            break;
                    }

                    if (test != Tests.TestRequestNone)
                        Parameters.TestToPerform.Add(test);
                }
            }

            if ((args.Length == 0) || (forceHelp == true))
            {
                Parameters.Test = Tests.TestHelp;
                Parameters.DoEmail = false;
                Parameters.TestToPerform = new List<Tests>
                {
                    Parameters.Test
                };
            }
            else
            {
                Parameters.Test = Tests.TestRequestNone;
            }
        }

        public static void ProcessService(ProcessTestService service)
        {
            Console.WriteLine($"Processing Service {Parameters.DoEmail}");
            try
            {
                Task.Run(async () =>
                {
                    await service.Process();
                }).Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception Processing Service {e}");
            }
        }

        public static async void Stop(IHost host)
        {
            Console.WriteLine("Stopping Host");
            try
            {
                await host.StopAsync();
                host.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception Stopping Host {e}");
            }
        }

        public static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .AddLogging()
                .AddDataServices()
                .AddAppServices()
                .AddConfigurations()
                .AddServices();
        }
    }
}