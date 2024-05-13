namespace interview.web.App.Interfaces
{
    public interface IGetServices<T> where T : class
    {
        Task<T> GetCustomAsync(string urlPath, string token);
        Task<T> GetCustomQueryIdAsync(string urlPath, string token, Dictionary<string, object> queryStringParameters);
    }
}