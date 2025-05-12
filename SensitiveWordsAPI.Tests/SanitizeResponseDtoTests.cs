using Xunit;
using SensitiveWordsAPI.DTOs;

namespace SensitiveWordsAPI.Tests
{
    public class SanitizeResponseDtoTests
    {
        [Fact]
        public void Constructor_SetsSanitizedMessage()
        {
            var dto = new SanitizeResponseDto { SanitizedMessage = "***" };
            Assert.Equal("***", dto.SanitizedMessage);
        }
    }
}
