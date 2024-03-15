using System;
using System.Threading.Tasks;
using MassTransit;
using Contracts;
using System.IO;
using System.Collections.Generic;

namespace Consumers
{
  // Premier consumer formattage du message, crétion de l'enveloppe MT.
  public class MsgConsumer : IConsumer<IQuoteSubmitted>
  {
    public Task Consume(ConsumeContext<IQuoteSubmitted> context)
    {
      // Crée un nouveau message MyMessage avec le contenant provenant de l'UI et nouveau ID
      var formattedQuote = new MyMessage
      {
        CorrelationId = Guid.NewGuid(),
        Name = context.Message.Name,
        Timestamp = context.Message.Timestamp
      };
      // Publie le message + enveloppe
      context.Publish<IQuoteFormatted>(formattedQuote);
      return Task.CompletedTask;
    }
  }

  // Je consume les messages pour ajout au .csv citation.
  public class CsvConsumerQuotes : IConsumer<QuoteFormattedQuotes>
  {
    static readonly string quoteFile = "data/quotes.csv";

    public Task Consume(ConsumeContext<QuoteFormattedQuotes> context)
    {
      string messagecsv1 = context.Message.Name;
      // ajoute à la fin la commande pour passer à la ligne
      string lineToWrite1 = messagecsv1 + Environment.NewLine;
        // Ajoute les lignes à la suite du csv
        File.AppendAllText(quoteFile, lineToWrite1);
        Console.WriteLine("Quote ajoutée au csv!");

      return Task.CompletedTask;
    }
  }
  // Je consume les messages pour ajout aux .csv logs.
  public class CsvConsumerLog : IConsumer<IQuoteFormatted>
  {
    static readonly string logFile = "data/log_quotes.csv";
    static readonly string separator = ",";
    public Task Consume(ConsumeContext<IQuoteFormatted> context)
    {
      // Formate en string le GUID
      var messageid = context.MessageId.ToString();
      // Création d'une liste d'items pour ajout par colonne
      string[] messagecsv2 = { messageid, context.Message.Timestamp };

      string lineToWrite = string.Join(separator, messagecsv2) + Environment.NewLine;

        // Ajoute à la ligne
        File.AppendAllText(logFile, lineToWrite);
        Console.WriteLine("Logs ajoutés au csv! ");

      return Task.CompletedTask;
    }
  }
  // Je consume les messages pour ajout au .csv count.
  public class CsvConsumerCount : IConsumer<QuoteFormattedCount>
  {
    static readonly string countQuotes = "data/count_quotes.csv";
    static readonly string quoteFile = "data/quotes.csv";

   public Task Consume(ConsumeContext<QuoteFormattedCount> context)
{


        // Ouvre et lis le fichier Quotes
        string[] existingLines = File.ReadAllLines(quoteFile);

        // Enleve le titre des colonnes au compte
        int count = existingLines.Length - 1;

        // Ouvre et lis le fichier Count
        string[] existingLines2 = {"COUNT", count.ToString()};


        // Écriture des lignes mises à jour dans le fichier Quotes
        File.WriteAllLines(countQuotes, existingLines2);

        Console.WriteLine("Count mis à jour!");


    Console.WriteLine($"{context.Message.Name}, {context.MessageId}");
    return Task.CompletedTask;
}
  }
}
