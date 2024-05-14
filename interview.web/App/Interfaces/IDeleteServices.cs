namespace interview.web.App.Interfaces
{
    public interface IDeleteServices<T> where T : class
    {
        Task<T> DeleteByIdCustomAsync(string urlPath, string token, string id);
    }
}
