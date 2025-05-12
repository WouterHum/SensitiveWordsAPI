namespace SensitiveWordsAPI.Models
{
    /// <summary>
    /// Model representing a sensitive word.
    /// </summary>
    public class SensitiveWord
    {
        public int Id { get; set; }
        public string Word { get; set; } = string.Empty;
    }
}
