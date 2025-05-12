using Microsoft.AspNetCore.Mvc;
using SensitiveWordsAPI.Models;
using SensitiveWordsAPI.Repositories;

namespace SensitiveWordsAPI.Controllers.Internal
{
    /// <summary>
    /// Controller for managing sensitive words.
    /// </summary>
    [ApiController]
    [Route("api/internal/[controller]")]
    public class SensitiveWordController : ControllerBase
    {
        private readonly ISensitiveWordRepository _repository;

        public SensitiveWordController(ISensitiveWordRepository repository) => _repository = repository;

        /// <summary>
        /// Retrieves all sensitive words.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _repository.GetAllAsync());

        /// <summary>
        /// Retrieves a sensitive word by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var word = await _repository.GetByIdAsync(id);
            return word == null ? NotFound() : Ok(word);
        }

        /// <summary>
        /// Creates a new sensitive word.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SensitiveWord word)
        {
            var id = await _repository.CreateAsync(word);
            word.Id = id;
            return CreatedAtAction(nameof(Get), new { id = word.Id }, word);
        }

        /// <summary>
        /// Updates an existing sensitive word.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SensitiveWord word)
        {
            word.Id = id;
            var result = await _repository.UpdateAsync(word);
            return result ? NoContent() : NotFound();
        }

        /// <summary>
        /// Deletes a sensitive word by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _repository.DeleteAsync(id);
            return result ? NoContent() : NotFound();
        }
    }

}
