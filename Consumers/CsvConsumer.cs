using System;
using System.Threading.Tasks;
using MassTransit;
using Contracts;
using System.IO;
using System.Linq;

namespace Consumers
{
    public class CsvConsumer : IConsumer<IHelloMessage>
    {
          static readonly string quoteFile = "data/quotes.csv";
          static readonly string countQuotes = "data/count_quotes.csv";
          static readonly string logFile = "data/log_quotes.csv";
          static readonly string separator = ",";
        public Task Consume(ConsumeContext<IHelloMessage> context)
        {
            string messagecsv1  = context.Message.Name;
            string lineToWrite1 = messagecsv1 + Environment.NewLine;
            string[] messagecsv2 = {context.Message.Id, context.Message.Timestamp};
            string lineToWrite = string.Join(separator, messagecsv2) + Environment.NewLine;
            try
                {
                    File.AppendAllText(quoteFile, lineToWrite1);
                    Console.WriteLine("Quote ajoutée au csv!");
                }
            catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

            try
                {
                    File.AppendAllText(logFile, lineToWrite);
                    Console.WriteLine("Logs ajoutés au csv! ");
                }
            catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            try
                {
                    int count = File.ReadLines(quoteFile).Count();
                    string lineToWriteCountQuotes = (count-1).ToString() + Environment.NewLine;
                    string[] existingLines = File.ReadAllLines(countQuotes);


                    if (existingLines.Length >= 2)
                    {
                        existingLines[1] = lineToWriteCountQuotes;
                    }
                    else if (existingLines.Length == 1)
                    {
                        Array.Resize(ref existingLines, 2);
                        existingLines[1] = lineToWriteCountQuotes;
                    }


                    File.WriteAllLines(countQuotes, existingLines);
                    Console.WriteLine("Count mis à jour!");
                }
            catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

            Console.WriteLine($"Hello world2, {context.MessageId}");
            return Task.CompletedTask;
        }
    }

}
