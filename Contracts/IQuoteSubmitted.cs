namespace Contracts
{
  using System;
  public interface IQuoteSubmitted
  {
    public Guid CorrelationId { get; set; }
    public string Name { get; set; }
    public string Timestamp { get; set; }
  }


  public interface IQuoteFormatted
  {
    public Guid CorrelationId { get; set; }
    public string Name { get; set; }
    public string Timestamp { get; set; }
  }

}
