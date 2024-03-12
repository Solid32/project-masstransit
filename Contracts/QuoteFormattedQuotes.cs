namespace Contracts
{
    using System;

    public record QuoteFormattedQuotes
    {
        public Guid CorrelationId { get; init; }
        public string Name { get; init; }
        public string Timestamp { get; init; }
    }
}
