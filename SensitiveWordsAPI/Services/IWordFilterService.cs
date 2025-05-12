namespace SensitiveWordsAPI.Services
{
    /// <summary>
    /// Interface for the Word Filter Service.
    /// </summary>
    public interface IWordFilterService
    {
        /// <summary>
        /// Sanitizes a message by replacing sensitive words with asterisks.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task<string> SanitizeMessageAsync(string message);
    }
}
