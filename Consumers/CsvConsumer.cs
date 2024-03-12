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
        CorrelationId = Guid.NewGuid(),
        Name = context.Message.Name,
        Timestamp = context.Message.Timestamp
      };
      context.Publish<IQuoteFormatted>(formattedQuote);
      return Task.CompletedTask;
    }
  }

  public class CsvConsumerQuotes : IConsumer<QuoteFormattedQuotes>
  {
    static readonly string quoteFile = "data/quotes.csv";

    public Task Consume(ConsumeContext<QuoteFormattedQuotes> context)
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
  public class CsvConsumerCount : IConsumer<QuoteFormattedCount>
  {
    static readonly string countQuotes = "data/count_quotes.csv";
    static readonly string quoteFile = "data/quotes.csv";
    static readonly string separator = ",";

   public Task Consume(ConsumeContext<QuoteFormattedCount> context)
{
    try
    {
        string[] existingLines = File.ReadAllLines(quoteFile);

        int count = existingLines.Length - 1;

        // Lecture des lignes du fichier countQuotes
        string[] existingLines2 = File.ReadAllLines(countQuotes);

        // Vérification si le fichier countQuotes a deux lignes
        if (existingLines2.Length != 2)
        {
            // Redimensionner le tableau existingLines2 à une longueur de 2
            Array.Resize(ref existingLines2, 2);
        }

        // Mise à jour de la première ligne avec le texte "COUNT"
        existingLines2[0] = "COUNT";

        // Mise à jour de la deuxième ligne avec le nombre de lignes du premier fichier
        existingLines2[1] = count.ToString();

        // Écriture des lignes mises à jour dans le fichier countQuotes
        File.WriteAllLines(countQuotes, existingLines2);

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
