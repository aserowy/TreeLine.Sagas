using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TreeLine.Sagas.DependencyInjection;

namespace TreeLine.Sagas.Examples.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host
                .CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();

                    services
                        .AddLogging()
                        .AddSagas();
                });
        }
    }
}
