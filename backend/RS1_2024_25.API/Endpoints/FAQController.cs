using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;
using System.Globalization;

namespace RS1_2024_25.API.Endpoints
{
    [ApiController]
    [Route("api/[controller]")]
    public class FAQController(ApplicationDbContext _db) : ControllerBase
    {
        public class FAQResponse
        {
            public int FAQId { get; set; }
            public string Question { get; set; }
            public string Answer { get; set; }
        }




        [HttpGet("get_10")]
        public async Task<IActionResult> GetTop10FAQs()
        {
            var faqs = await _db.FAQs.Take(10).Select(x => new FAQResponse
            {

                FAQId = x.FAQId,
                Question = x.Question,
                Answer = x.Answer,

            }).ToListAsync();

            return Ok(faqs);
        }

        public class FAQRequest
        {
            public string Question { get; set; }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddFAQ([FromBody] FAQRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Question))
                return BadRequest("Question is required");

            int? userId = null;

            var userIdClaim = User.FindFirst("id")?.Value;
            if (int.TryParse(userIdClaim, out int parsedId))
                userId = parsedId;

            var faq = new FAQ
            {
                Question = request.Question,
                Answer = null,
                UserId = userId
            };

            await _db.FAQs.AddAsync(faq);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Your question has been submitted." });
        }
    }
}
