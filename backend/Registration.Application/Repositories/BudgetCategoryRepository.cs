using Microsoft.EntityFrameworkCore;
using Registration.Application.DTOs;
using Registration.Application.Interfaces;
using Registration.Domain.Entities;
using Registration.Persistence.DbContext;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Registration.Application.Repositories
{
    public class BudgetCategoryRepository : IBudgetCategoryService
    {
        private readonly ApplicationDbContext _context;

        public BudgetCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BudgetCategoryDto>> GetAllAsync()
        {
            return await _context.BudgetCategories
                .Select(b => new BudgetCategoryDto
                {
                    Id = b.Id,
                    CategoryType = b.CategoryType,
                    BudgetAmount = b.BudgetAmount,
                    FinancialYear = b.FinancialYear
                }).ToListAsync();
        }

        public async Task<BudgetCategoryDto> GetByIdAsync(int id)
        {
            var entity = await _context.BudgetCategories.FindAsync(id);
            if (entity == null) return null;

            return new BudgetCategoryDto
            {
                Id = entity.Id,
                CategoryType = entity.CategoryType,
                BudgetAmount = entity.BudgetAmount,
                FinancialYear = entity.FinancialYear
            };
        }

        // ✅ Implemented exactly like interface requires
        public async Task<BudgetCategoryDto> CreateAsync(BudgetCategoryDto dto)
        {
            var budgetCategory = new BudgetCategory
            {
                Id = dto.Id,   // manual ID
                CategoryType = dto.CategoryType,
                BudgetAmount = dto.BudgetAmount,
                FinancialYear = dto.FinancialYear
            };

            _context.BudgetCategories.Add(budgetCategory);
            await _context.SaveChangesAsync();

            return dto;
        }

        public async Task<BudgetCategoryDto> UpdateAsync(int id, BudgetCategoryDto dto)
        {
            var entity = await _context.BudgetCategories.FindAsync(id);
            if (entity == null) return null;

            entity.CategoryType = dto.CategoryType;
            entity.BudgetAmount = dto.BudgetAmount;
            entity.FinancialYear = dto.FinancialYear;

            await _context.SaveChangesAsync();
            return dto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.BudgetCategories.FindAsync(id);
            if (entity == null) return false;

            _context.BudgetCategories.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
