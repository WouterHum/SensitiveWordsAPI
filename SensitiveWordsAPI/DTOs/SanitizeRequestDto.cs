using System.ComponentModel.DataAnnotations;

namespace SensitiveWordsAPI.DTOs
{
    /// <summary>
    /// Data Transfer Object for sanitizing requests.
    /// </summary>
    public class SanitizeRequestDto
    {
        [Required]
        public string Message { get; set; } = string.Empty;
    }
}
