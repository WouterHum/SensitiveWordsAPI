using SensitiveWordsAPI.Repositories;
using System.Text.RegularExpressions;

namespace SensitiveWordsAPI.Services
{
    /// <summary>
    /// Service for filtering sensitive words from messages.
    /// </summary>
    public class WordFilterService : IWordFilterService
    {
        /// <summary>
        /// Repository for managing sensitive words.
        /// </summary>
        private readonly ISensitiveWordRepository _repository;

        /// <summary>
        /// Initializes a new instance of the WordFilterService class with the specified repository.
        /// </summary>
        /// <param name="repository"></param>
        public WordFilterService(ISensitiveWordRepository repository) => _repository = repository;

        /// <summary>
        /// Sanitizes a message by replacing sensitive words with asterisks.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>The message with * if there is a senstitve word in it</returns>
        public async Task<string> SanitizeMessageAsync(string message)
        {
            var words = await _repository.GetAllAsync();
            foreach (var word in words)
            {
                var pattern = $@"\b{Regex.Escape(word.Word)}\b";
                message = Regex.Replace(message, pattern, new string('*', word.Word.Length), RegexOptions.IgnoreCase);
            }
            return message;
        }
    }

}
