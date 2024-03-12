namespace Contracts
{
  using System;
    public class MyMessage : IQuoteSubmitted, IQuoteFormatted
    {
       public Guid CorrelationId { get; set; }
        public string Name { get; set; }
        public string Timestamp { get; set; }
    }
}
