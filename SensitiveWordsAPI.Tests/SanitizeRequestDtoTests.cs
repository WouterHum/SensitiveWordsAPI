using SensitiveWordsAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensitiveWordsAPI.Tests
{
    public class SanitizeRequestDtoTests
    {
        [Fact]
        public void Constructor_SetsMessage()
        {
            var dto = new SanitizeRequestDto { Message = "hello" };
            Assert.Equal("hello", dto.Message);
        }
    }
}
