using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RK7Die.CashServer;
using RK7Die.CashServer.Query;
using Serilog;

namespace RK7Die.Example
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = Host
                .CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOptions();
                    services.Configure<RK7Die.CashServer.ClientOptions>(hostContext.Configuration.GetSection("RK7Client"));
                    services.AddSingleton<RK7Die.CashServer.Client>();
                })
                .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                    .ReadFrom.Configuration(hostingContext.Configuration))
                .UseConsoleLifetime()
                .Build();

            host.RunAsync();

            var client = host.Services.GetService<RK7Die.CashServer.Client>();

            GetOrderList(client);

            Console.ReadKey();
        }

        static private void GetOrderList(Client client)
        {
            GetOrderList getOrderList = new GetOrderList
            {
                OnlyOpened = true
            };

            string result = client.SendQuery(getOrderList);
        }
    }
}
