using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dtos;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private static readonly List<StudentDto> Students = new List<StudentDto>
        {
            new StudentDto { Id = 1, Fullname = "John Doe", SeriaNo = "AA121212", Age = 33, Score = 100 },
            new StudentDto { Id = 2, Fullname = "Jane Smith", SeriaNo = "BB123456", Age = 28, Score = 90 }
        };

        // GET: api/student
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(Students);
        }

        // GET api/student/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var student = Students.FirstOrDefault(s => s.Id == id);
            if (student == null) return NotFound();
            return Ok(student);
        }

        // POST api/student
        [HttpPost]
        public IActionResult Post([FromBody] StudentDto dto)
        {
            if (dto == null) return BadRequest("Student data is null");

            // Add logic to generate a new ID or handle any other required logic
            dto.Id = Students.Max(s => s.Id) + 1;
            Students.Add(dto);
            return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
        }

        // PUT api/student/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] StudentDto dto)
        {
            if (dto == null) return BadRequest("Student data is null");

            var student = Students.FirstOrDefault(s => s.Id == id);
            if (student == null) return NotFound();

            // Update student details
            student.Fullname = dto.Fullname;
            student.SeriaNo = dto.SeriaNo;
            student.Age = dto.Age;
            student.Score = dto.Score;

            return NoContent();
        }

        // DELETE api/student/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var student = Students.FirstOrDefault(s => s.Id == id);
            if (student == null) return NotFound();

            Students.Remove(student);
            return NoContent();
        }
    }
}
