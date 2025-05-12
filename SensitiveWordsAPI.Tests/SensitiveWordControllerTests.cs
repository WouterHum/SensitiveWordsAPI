using Microsoft.AspNetCore.Mvc;
using Moq;
using SensitiveWordsAPI.Controllers.Internal;
using SensitiveWordsAPI.Models;
using SensitiveWordsAPI.Repositories;

namespace SensitiveWordsAPI.Tests
{
    public class SensitiveWordControllerTests
    {
        private readonly Mock<ISensitiveWordRepository> _mockRepo;
        private readonly SensitiveWordController _controller;

        public SensitiveWordControllerTests()
        {
            _mockRepo = new Mock<ISensitiveWordRepository>();
            _controller = new SensitiveWordController(_mockRepo.Object);
        }

        [Fact]
        public async Task Get_ReturnsAllWords()
        {
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<SensitiveWord>());
            var result = await _controller.Get();
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Get_ById_ReturnsWord_WhenFound()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new SensitiveWord { Id = 1, Word = "test" });
            var result = await _controller.Get(1);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Get_ById_ReturnsNotFound_WhenMissing()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((SensitiveWord?)null);
            var result = await _controller.Get(1);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedWord()
        {
            var word = new SensitiveWord { Word = "bad" };
            _mockRepo.Setup(r => r.CreateAsync(word)).ReturnsAsync(1);

            var result = await _controller.Create(word);

            var createdAt = Assert.IsType<CreatedAtActionResult>(result);
            var returnedWord = Assert.IsType<SensitiveWord>(createdAt.Value);
            Assert.Equal(1, returnedWord.Id);
        }

        [Fact]
        public async Task Update_ReturnsNoContent_WhenSuccessful()
        {
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<SensitiveWord>())).ReturnsAsync(true);

            var result = await _controller.Update(1, new SensitiveWord { Word = "changed" });

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenFailed()
        {
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<SensitiveWord>())).ReturnsAsync(false);

            var result = await _controller.Update(1, new SensitiveWord());

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenSuccessful()
        {
            _mockRepo.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);
            var result = await _controller.Delete(1);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenFailed()
        {
            _mockRepo.Setup(r => r.DeleteAsync(1)).ReturnsAsync(false);
            var result = await _controller.Delete(1);
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
