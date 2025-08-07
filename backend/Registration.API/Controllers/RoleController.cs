using Microsoft.AspNetCore.Mvc;
using Registration.Application.DTOs;
using Registration.Application.Interfaces;

namespace Registration.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        // GET: api/Role
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }

        // GET: api/Role/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null)
                return NotFound();

            return Ok(role);
        }

        // POST: api/Role
        [HttpPost]
        public async Task<IActionResult> Create(RoleDto roleDto)
        {
            await _roleService.CreateRoleAsync(roleDto);
            return CreatedAtAction(nameof(GetById), new { id = roleDto.Role_Id }, roleDto);
        }

        // PUT: api/Role/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RoleDto roleDto)
        {
            if (id != roleDto.Role_Id)
                return BadRequest("ID mismatch");

            var updated = await _roleService.UpdateRoleAsync(id, roleDto);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/Role/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _roleService.DeleteRoleAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
