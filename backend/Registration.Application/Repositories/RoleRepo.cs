using Registration.Application.DTOs;
using Registration.Application.Interfaces;
using Registration.Domain.Entities;
using Registration.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Registration.Application.Repositories
{
    public class RoleRepo : IRoleService
    {
        private readonly ApplicationDbContext _context;

        public RoleRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            return await _context.Roles
                .Select(r => new RoleDto
                {
                    Role_Id = r.Role_Id,
                    Role_Title = r.Role_Title,
                    Project_Name = r.Project_Name
                })
                .ToListAsync();
        }

        public async Task<RoleDto?> GetRoleByIdAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return null;

            return new RoleDto
            {
                Role_Id = role.Role_Id,
                Role_Title = role.Role_Title,
                Project_Name = role.Project_Name
            };
        }

        public async Task<RoleDto> CreateRoleAsync(RoleDto roleDto)
        {
            var role = new Role
            {
                Role_Title = roleDto.Role_Title,
                Project_Name = roleDto.Project_Name
            };

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            roleDto.Role_Id = role.Role_Id;
            return roleDto;
        }

        public async Task<bool> UpdateRoleAsync(int id, RoleDto roleDto)
        {
            var existing = await _context.Roles.FindAsync(id);
            if (existing == null) return false;

            existing.Role_Title = roleDto.Role_Title;
            existing.Project_Name = roleDto.Project_Name;

            _context.Roles.Update(existing);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteRoleAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return false;

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
