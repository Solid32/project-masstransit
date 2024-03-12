namespace Contracts
{
    using System;

    public record QuoteFormattedCount
    {
        public Guid CorrelationId { get; init; }
        public string Name { get; init; }
    }
}
