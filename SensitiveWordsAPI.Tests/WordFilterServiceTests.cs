using Moq;
using SensitiveWordsAPI.Models;
using SensitiveWordsAPI.Repositories;
using SensitiveWordsAPI.Services;

namespace SensitiveWordsAPI.Tests
{
    public class WordFilterServiceTests
    {
        private readonly Mock<ISensitiveWordRepository> _mockRepository;
        private readonly WordFilterService _service;

        public WordFilterServiceTests()
        {
            _mockRepository = new Mock<ISensitiveWordRepository>();
            _service = new WordFilterService(_mockRepository.Object);
        }

        [Fact]
        public async Task SanitizeMessageAsync_ShouldReplaceSensitiveWordsWithAsterisks()
        {
            // Arrange
            var sensitiveWords = new List<SensitiveWord>
            {
                new SensitiveWord { Id = 1, Word = "SELECT" },
                new SensitiveWord { Id = 2, Word = "DROP" }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync())
                           .ReturnsAsync(sensitiveWords);

            var input = "SELECT * FROM users WHERE id = 1; DROP TABLE users;";
            var expected = "****** * FROM users WHERE id = 1; **** TABLE users;";

            // Act
            var result = await _service.SanitizeMessageAsync(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task SanitizeMessageAsync_ShouldReturnOriginal_WhenNoSensitiveWordsConfigured()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAllAsync())
                           .ReturnsAsync(new List<SensitiveWord>());

            var input = "This is a safe message.";
            var expected = input;

            // Act
            var result = await _service.SanitizeMessageAsync(input);

            // Assert
            Assert.Equal(expected, result);
        }


        [Fact]
        public async Task SanitizeMessageAsync_ReplacesSensitiveWords()
        {
            var mockRepo = new Mock<ISensitiveWordRepository>();
            mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new[]
            {
            new SensitiveWord { Word = "SELECT" },
            new SensitiveWord { Word = "DROP" }
        });

            var service = new WordFilterService(mockRepo.Object);
            var input = "SELECT * FROM TABLE WHERE ID = 1; DROP TABLE USERS;";
            var result = await service.SanitizeMessageAsync(input);

            Assert.DoesNotContain("SELECT", result, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("DROP", result, StringComparison.OrdinalIgnoreCase);
        }
    }
}