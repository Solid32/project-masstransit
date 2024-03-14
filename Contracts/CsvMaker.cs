namespace Contracts
{
  using System;
    public class MyMessage : IQuoteSubmitted, IQuoteFormatted // Essai aux interfaces. Inutile ici (?)
    {
       public Guid CorrelationId { get; set; }
        public string Name { get; set; }
        public string Timestamp { get; set; }
    }
}
