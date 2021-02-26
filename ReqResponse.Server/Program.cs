using ReqResponse.Models;
using ReqResponse.Services.Network;
using System;

internal class MethodServer
{
    private static void Main()
    {
        Options options = new Options();
        Console.WriteLine($"Starting ReqResponse.Server on Port: {options.Port}");
        Server.NewServer(options.Port);
    }
}