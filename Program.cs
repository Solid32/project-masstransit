
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
                      cfg.ReceiveEndpoint("Contracts:IQuoteSubmitted", ep =>
                             {
                                ep.ClearSerialization();
                                ep.UseRawJsonSerializer(RawSerializerOptions.AddTransportHeaders | RawSerializerOptions.CopyHeaders);
                                ep.ConfigureConsumer<MsgConsumer>(context);
                                ep.ConfigureConsumeTopology = false;
                              });
                      //cfg.ReceiveEndpoint("Contracts:IQuoteFormatted", ep =>
                      //       {
                      //          ep.ClearSerialization();
                      //          ep.UseRawJsonSerializer();
                      //          ep.UseRawJsonDeserializer();
                      //          ep.ConfigureConsumeTopology = true;
                      //        });
                      cfg.ReceiveEndpoint("Contracts:IQuoteFormattedQuotes", ep =>
                             {
                                ep.ClearSerialization();
                                ep.ConfigureConsumer<CsvConsumerQuotes>(context);
                                ep.UseRawJsonSerializer();
                                ep.UseRawJsonDeserializer();
                                ep.ConfigureConsumeTopology = true;
                              });
                      cfg.ReceiveEndpoint("Contracts:IQuoteFormattedLog", ep =>
                             {
                                ep.ClearSerialization();
                                ep.ConfigureConsumer<CsvConsumerLog>(context);
                                ep.UseRawJsonSerializer();
                                ep.UseRawJsonDeserializer();
                                ep.ConfigureConsumeTopology = true;
                              });
                      cfg.ReceiveEndpoint("Contracts:IQuoteFormattedCount", ep =>
                             {
                                ep.ClearSerialization();
                                ep.ConfigureConsumer<CsvConsumerCount>(context);
                                ep.UseRawJsonSerializer();
                                ep.UseRawJsonDeserializer();
                                ep.ConfigureConsumeTopology = true;
                              });
                      });



                  x.AddConsumer<MsgConsumer>();
                  x.AddConsumer<CsvConsumerQuotes>();
                  x.AddConsumer<CsvConsumerLog>();
                  x.AddConsumer<CsvConsumerCount>();
                });

              services.AddHostedService<Worker>();
            });
  }
}
