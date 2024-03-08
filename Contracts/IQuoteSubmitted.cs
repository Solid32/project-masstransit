namespace Contracts
{
  public interface IQuoteSubmitted
  {
    public string Name { get; set; }
    public string Timestamp { get; set; }
  }


  public interface IQuoteFormatted
  {
    public string Name { get; set; }
    public string Timestamp { get; set; }
  }

}
