using Xunit;
using Moq;
using SensitiveWordsAPI.Controllers;
using SensitiveWordsAPI.DTOs;
using SensitiveWordsAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SensitiveWordsAPI.Controllers.External;

namespace SensitiveWordsAPI.Tests
{
    public class SanitizeControllerTests
    {
        [Fact]
        public async Task Sanitize_ReturnsSanitizedResponse()
        {
            // Arrange
            var mockService = new Mock<IWordFilterService>();
            mockService.Setup(s => s.SanitizeMessageAsync("test"))
                .ReturnsAsync("***");

            var controller = new SanitizeController(mockService.Object);
            var request = new SanitizeRequestDto { Message = "test" };

            // Act
            var result = await controller.Sanitize(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<SanitizeResponseDto>(okResult.Value);
            Assert.Equal("***", response.SanitizedMessage);
        }
    }
}