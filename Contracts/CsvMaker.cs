namespace Contracts
{
    public class MyMessage : IQuoteSubmitted, IQuoteFormatted
    {
        public string Name { get; set; }
        public string Timestamp { get; set; }
    }
}
