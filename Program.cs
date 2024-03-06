
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Consumers;

namespace GettingStarted
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(x =>
                    {
                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Host("localhost", "/", h =>
                            {
                                h.Username("guest");
                                h.Password("guest");
                            });

                              //cfg.ConfigureEndpoints(context);
                               cfg.ReceiveEndpoint("GettingStarted", ep =>
                                  {
                                       ep.ClearSerialization();
                                       //ep.UseRawJsonDeserializer();
                                       ep.UseRawJsonSerializer(RawSerializerOptions.AddTransportHeaders | RawSerializerOptions.CopyHeaders);
                                       ep.ConfigureConsumer<CsvConsumer>(context);
                                       ep.ConfigureConsumeTopology = false;
                                  });
                        });


                        x.AddConsumer<CsvConsumer>();
                    });

                    services.AddHostedService<Worker>();
                });
    }
}
