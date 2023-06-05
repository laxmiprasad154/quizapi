
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using quizapi.Data_Access_Layer.context;
using quizapi.Data_Access_Layer.Entities;

namespace quizapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly Quizdbcontext _context;

        public QuestionController(Quizdbcontext context)
        {
            _context = context;
        }

        // GET: api/Question
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Question>>> GetQuestions()
        {
            if (_context.Questions == null)
            {
                return NotFound();
            }
            var random5Qns = await (_context.Questions
                .Select(x => new
                {
                    QnId = x.QnId,
                    QnInWords = x.QnInWords,
                    
                    Options = new string[] { x.Option1, x.Option2, x.Option3, x.Option4 }
                })
                .OrderBy(y => Guid.NewGuid())
                .Take(5)
                ).ToListAsync();
            return Ok(random5Qns);
        }

        // GET: api/Question/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Question>> GetQuestion(int id)
        {
            if (_context.Questions == null)
            {
                return NotFound();
            }
            var question = await _context.Questions.FindAsync(id);

            if (question == null)
            {
                return NotFound();
            }

            return question;
        }

        // PUT: api/Question/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuestion(int id, Question question)
        {
            if (id != question.QnId)
            {
                return BadRequest();
            }

            _context.Entry(question).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Question
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public IActionResult AddQuestion([FromBody] Question question)
        {
            if (question == null)
            {
                return BadRequest();
            }
            try
            {
                _context.Questions.Add(question);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest($"error saving the message:{ex.InnerException.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occured :{ex.Message}");
            }


            return Ok();

        }

        // DELETE: api/Question/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            if (_context.Questions == null)
            {
                return NotFound();
            }
            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QuestionExists(int id)
        {
            return (_context.Questions?.Any(e => e.QnId == id)).GetValueOrDefault();
        }

        [HttpPost]
        [Route("GetAnswers")]
        public async Task<ActionResult<Question>> RetrieveAnswers(int[] qnIds)
        {
            var answers = await (_context.Questions
                .Where(x => qnIds.Contains(x.QnId))
                .Select(y => new
                {
                    QnId = y.QnId,
                    QnInWords = y.QnInWords,
                    
                    Options = new string[] { y.Option1, y.Option2, y.Option3, y.Option4 },
                    Answer = y.Answer
                })).ToListAsync();
            return Ok(answers);
        }
    }
}
