using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Contracts;
using Consumers;

namespace GettingStarted
{
    public class Worker : BackgroundService
    {
        readonly IBus _bus;

        public Worker(IBus bus)
        {
            _bus = bus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //Console.WriteLine("Publishing HelloMessage with name 'World'");
                //await _bus.Publish(new IHelloMessage { Name = "World" }, stoppingToken);

                await Task.Delay(1000, stoppingToken);
            }
        }
    }

}
