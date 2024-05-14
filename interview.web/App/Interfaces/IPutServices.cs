namespace interview.web.App.Interfaces
{
    public interface IPutServices<T> where T : class
    {
        Task<T> PutCustomAsync(object body, string url, string token);
    }
}
