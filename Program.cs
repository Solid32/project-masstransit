
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Consumers;
using Company.StateMachines;

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

                      cfg.ClearSerialization();
                      cfg.UseRawJsonSerializer();


                      cfg.ConfigureEndpoints(context);
                      });

                  x.AddSagaStateMachine<StateMachStateMachine, QuoteFormattedState>()
                  .InMemoryRepository();
                  x.AddConsumer<MsgConsumer>();
                  x.AddConsumer<CsvConsumerQuotes>();
                  x.AddConsumer<CsvConsumerLog>();
                  x.AddConsumer<CsvConsumerCount>();
                });
              services.AddHostedService<Worker>();
            });
  }
}
