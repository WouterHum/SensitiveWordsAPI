using SensitiveWordsAPI.Models;

namespace SensitiveWordsAPI.Repositories
{
    /// <summary>
    /// Interface for the SensitiveWord repository.
    /// </summary>
    public interface ISensitiveWordRepository
    {
        Task<IEnumerable<SensitiveWord>> GetAllAsync();
        Task<SensitiveWord?> GetByIdAsync(int id);
        Task<int> CreateAsync(SensitiveWord word);
        Task<bool> UpdateAsync(SensitiveWord word);
        Task<bool> DeleteAsync(int id);
    }
}
