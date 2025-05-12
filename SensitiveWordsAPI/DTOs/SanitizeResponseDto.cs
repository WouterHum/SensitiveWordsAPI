namespace SensitiveWordsAPI.DTOs
{
    /// <summary>
    ///  Data Transfer Object for sanitizing responses.
    /// </summary>
    public class SanitizeResponseDto
    {
        public string SanitizedMessage { get; set; } = string.Empty;
    }
}
