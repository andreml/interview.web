namespace interview.web.App.Interfaces
{
    public interface IPostServices<T> where T : class
    {
        Task<T> PostCustomAsync(object body, string url, string token);
    }
}