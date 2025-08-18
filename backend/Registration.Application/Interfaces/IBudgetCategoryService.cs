using Registration.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Registration.Application.Interfaces
{
    public interface IBudgetCategoryService
    {
        Task<IEnumerable<BudgetCategoryDto>> GetAllAsync();
        Task<BudgetCategoryDto> GetByIdAsync(int id);

        // Accepts manual ID from BudgetCategoryDto
        Task<BudgetCategoryDto> CreateAsync(BudgetCategoryDto dto);

        Task<BudgetCategoryDto> UpdateAsync(int id, BudgetCategoryDto dto);
        Task<bool> DeleteAsync(int id);
    }
}

