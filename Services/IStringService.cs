using MinimalApi.Models;

namespace MinimalApi.Services
{
    public interface IStringService
    {
        Task<List<StringItem>> GetAllAsync();
        Task<StringItem?> GetByIdAsync(string id);
        Task<StringItem> CreateAsync(string valor);
        Task<bool> DeleteAsync(string id);
    }
}
