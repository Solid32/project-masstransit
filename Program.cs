
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
        //Création et configuration de la plateforme RabbitMQ
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

                      cfg.ClearSerialization(); // Reset la serialisation
                      cfg.UseRawJsonSerializer(); // Serialise en Json Raw


                      cfg.ConfigureEndpoints(context); // Configure tout les endpoints pour créer la tuyauterie entre les handlers et les queues
                      });

                  x.AddSagaStateMachine<QuotesStateMachine, QuoteFormattedState>()
                  .InMemoryRepository(); // Utilise la mémoire interne pour les états de la saga
                  x.AddConsumer<MsgConsumer>();
                  x.AddConsumer<CsvConsumerQuotes>();
                  x.AddConsumer<CsvConsumerLog>();
                  x.AddConsumer<CsvConsumerCount>();
                });
              services.AddHostedService<Worker>();
            });
  }
}
