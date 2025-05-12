using Microsoft.AspNetCore.Mvc;
using SensitiveWordsAPI.DTOs;
using SensitiveWordsAPI.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace SensitiveWordsAPI.Controllers.External
{
    /// <summary>
    /// Controller for sanitizing messages by censoring sensitive words.
    /// </summary>
    [ApiController]
    [Route("api/external/[controller]")]
    [SwaggerTag("Sanitizes messages by censoring sensitive words")]
    public class SanitizeController : ControllerBase
    {
        private readonly IWordFilterService _service;

        public SanitizeController(IWordFilterService service) => _service = service;

        /// <summary>
        /// Sanitizes a message by replacing sensitive words with asterisks.
        /// </summary>
        /// <param name="request">Request object containing the message.</param>
        /// <returns>A Sanatized message</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Sanitize a message", Description = "Replaces all sensitive words with asterisks")]
        [ProducesResponseType(typeof(SanitizeResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Sanitize([FromBody] SanitizeRequestDto request)
        {
            var result = await _service.SanitizeMessageAsync(request.Message);
            return Ok(new SanitizeResponseDto { SanitizedMessage = result });
        }
    }

}
