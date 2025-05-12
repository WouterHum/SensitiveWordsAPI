namespace SensitiveWordsAPI.DTOs
{
    /// <summary>
    /// Data Transfer Object for sensitive words.
    /// </summary>
    public class SensitiveWordDto
    {
        public int Id { get; set; }
        public string Word { get; set; } = string.Empty;
    }
}
