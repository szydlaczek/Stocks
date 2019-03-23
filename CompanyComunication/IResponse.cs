namespace CompanyComunication
{
    public interface IResponse
    {
        string JSONResult { get; }
        Error Error { get; }
    }
}