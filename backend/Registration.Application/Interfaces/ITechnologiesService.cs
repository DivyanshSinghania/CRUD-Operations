using Registration.Application.DTOs;

namespace Registration.Application.Interfaces
{
    public interface ITechnologiesService
    {
        Task<IEnumerable<TechnologiesDto>> GetAllAsync();
        Task<TechnologiesDto> GetByIdAsync(int id);
        Task<TechnologiesDto> AddAsync(TechnologiesDto dto);
        Task<TechnologiesDto> UpdateAsync(int id, TechnologiesDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
