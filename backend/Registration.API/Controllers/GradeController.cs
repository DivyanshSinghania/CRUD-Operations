using Microsoft.AspNetCore.Mvc;
using Registration.Domain.Entities;
using Registration.Application.DTOs;
using Registration.Application.Services;
using System.Threading.Tasks;

namespace Registration.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeController : ControllerBase
    {
        private readonly IGradeService _gradeService;

        public GradeController(IGradeService gradeService)
        {
            _gradeService = gradeService;
        }

        // GET: api/Grade
        [HttpGet]
        public async Task<IActionResult> GetAllGrades()
        {
            var grades = await _gradeService.GetAllGradesAsync();
            return Ok(grades);
        }

        // GET: api/Grade/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGradeById(int id)
        {
            var grade = await _gradeService.GetGradeByIdAsync(id);
            if (grade == null)
                return NotFound();
            return Ok(grade);
        }

        // POST: api/Grade
        [HttpPost]
        public async Task<IActionResult> CreateGrade([FromBody] GradeDTO gradeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdGrade = await _gradeService.AddGradeAsync(gradeDto);
            return CreatedAtAction(nameof(GetGradeById), new { id = createdGrade.GradeId }, createdGrade);
        }

        // PUT: api/Grade/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGrade(int id, [FromBody] GradeDTO gradeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedGrade = await _gradeService.UpdateGradeAsync(id, gradeDto);
            if (updatedGrade == null)
                return NotFound();

            return Ok(updatedGrade);
        }

        // DELETE: api/Grade/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGrade(int id)
        {
            var isDeleted = await _gradeService.DeleteGradeAsync(id);
            if (!isDeleted)
                return NotFound();

            return NoContent();
        }
    }
}
