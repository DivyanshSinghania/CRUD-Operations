using Microsoft.AspNetCore.Mvc;
using Registration.Application.Interfaces;
using Registration.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Registration.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechnologiesController : ControllerBase
    {
        private readonly ITechnologiesService _service;

        public TechnologiesController(ITechnologiesService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var technologies = await _service.GetAllAsync();
            return Ok(technologies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var technology = await _service.GetByIdAsync(id);
            if (technology == null)
                return NotFound();
            return Ok(technology);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] TechnologiesDto dto)
        {
            var created = await _service.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TechnologiesDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
                return NotFound();
            return Ok();
        }
    }
}
