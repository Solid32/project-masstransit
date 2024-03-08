using System;
using System.Threading.Tasks;
using MassTransit;
using Contracts;
using System.IO;
using System.Linq;

namespace Consumers
{
  public class MsgConsumer : IConsumer<IQuoteSubmitted>
  {
    public Task Consume(ConsumeContext<IQuoteSubmitted> context)
    {
      var formattedQuote = new MyMessage
      {
        Name = context.Message.Name,
        Timestamp = context.Message.Timestamp
      };
      context.Publish<IQuoteFormatted>(formattedQuote);
      return Task.CompletedTask;
    }
  }

  public class CsvConsumerQuotes : IConsumer<IQuoteFormatted>
  {
    static readonly string quoteFile = "data/quotes.csv";

    public Task Consume(ConsumeContext<IQuoteFormatted> context)
    {
      string messagecsv1 = context.Message.Name;
      string lineToWrite1 = messagecsv1 + Environment.NewLine;
      try
      {
        File.AppendAllText(quoteFile, lineToWrite1);
        Console.WriteLine("Quote ajoutée au csv!");
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error: {ex.Message}");
      }
      return Task.CompletedTask;
    }
  }
  public class CsvConsumerLog : IConsumer<IQuoteFormatted>
  {
    static readonly string logFile = "data/log_quotes.csv";
    static readonly string separator = ",";
    public Task Consume(ConsumeContext<IQuoteFormatted> context)
    {
      var messageid = context.MessageId.ToString();
      string[] messagecsv2 = { messageid, context.Message.Timestamp };
      string lineToWrite = string.Join(separator, messagecsv2) + Environment.NewLine;
      try
      {
        File.AppendAllText(logFile, lineToWrite);
        Console.WriteLine("Logs ajoutés au csv! ");
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error: {ex.Message}");
      }

      return Task.CompletedTask;
    }
  }
  public class CsvConsumerCount : IConsumer<IQuoteFormatted>
  {
    static readonly string countQuotes = "data/count_quotes.csv";
    static readonly string quoteFile = "data/quotes.csv";
    static readonly string separator = ",";
    public Task Consume(ConsumeContext<IQuoteFormatted> context)
    {
      try
      {
        string[] existingLines = File.ReadAllLines(countQuotes);

        if (existingLines.Length >= 2)
        {
          int value = int.Parse(existingLines[1]);
          value++;
          existingLines[1] = value.ToString();
        }
        else if (existingLines.Length == 1)
        {
          existingLines = new string[] { existingLines[0], "1" };
        }
        else
        {
          existingLines = new string[] { "0" };
        }

        File.WriteAllLines(countQuotes, existingLines);
        Console.WriteLine("Count mis à jour!");
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error: {ex.Message}");
      }

      Console.WriteLine($"{context.Message.Name}, {context.MessageId}");
      return Task.CompletedTask;
    }
  }
}
