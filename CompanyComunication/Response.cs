namespace CompanyComunication
{
    internal class Response : IResponse
    {
        public string JSONResult { get; } = string.Empty;

        public Error Error { get; }

        public Response(string jSONResult)
        {
            JSONResult = jSONResult;
        }

        public Response(Error error)
        {
            Error = error;
        }
    }
}