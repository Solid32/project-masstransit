namespace Consumers
{
//  using MassTransit;
//
//  public class CsvConsumerQuotesDefinition :
//      ConsumerDefinition<CsvConsumerQuotes>
//  {
//      protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<CsvConsumerQuotes> consumerConfigurator)
//      {
//
//          endpointConfigurator.ClearSerialization();
//          endpointConfigurator.UseRawJsonDeserializer();
//          endpointConfigurator.UseRawJsonSerializer(RawSerializerOptions.AddTransportHeaders );
//          endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
//          endpointConfigurator.ConfigureConsumeTopology = false;
//
//          if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rabbit)
//          {
//            rabbit.Bind("Contracts:IQuoteFormattedQuotes");
//          }
//      }
//  }
//
}
