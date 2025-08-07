using Registration.Application.DTOs;

namespace Registration.Application.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task<RoleDto?> GetRoleByIdAsync(int id);
        Task<RoleDto> CreateRoleAsync(RoleDto roleDto);
        Task<bool> UpdateRoleAsync(int id, RoleDto roleDto);
        Task<bool> DeleteRoleAsync(int id);
    }
}
