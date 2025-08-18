using Microsoft.EntityFrameworkCore;
using Registration.Application.DTOs;
using Registration.Application.Interfaces;
using Registration.Domain.Entities;
using Registration.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Registration.Application.Repositories
{
    public class TechnologiesRepository : ITechnologiesService
    {
        private readonly ApplicationDbContext _context;

        public TechnologiesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TechnologiesDto>> GetAllAsync()
        {
            return await _context.Technologies
                .Select(t => new TechnologiesDto
                {
                    Id = t.Id,
                    TechnologyStack = t.TechnologyStack
                })
                .ToListAsync();
        }

        public async Task<TechnologiesDto> GetByIdAsync(int id)
        {
            var tech = await _context.Technologies.FindAsync(id);
            if (tech == null) return null;

            return new TechnologiesDto
            {
                Id = tech.Id,
                TechnologyStack = tech.TechnologyStack
            };
        }

        public async Task<TechnologiesDto> AddAsync(TechnologiesDto dto)
        {
            // Generate Id manually if needed
            var nextId = (_context.Technologies.Any()
                          ? await _context.Technologies.MaxAsync(t => t.Id)
                          : 0) + 1;

            var entity = new Technology
            {
                Id = nextId,
                TechnologyStack = dto.TechnologyStack
            };

            _context.Technologies.Add(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            return dto;
        }

        public async Task<TechnologiesDto> UpdateAsync(int id, TechnologiesDto dto)
        {
            var tech = await _context.Technologies.FindAsync(id);
            if (tech == null) return null;

            tech.TechnologyStack = dto.TechnologyStack;
            await _context.SaveChangesAsync();

            return dto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var tech = await _context.Technologies.FindAsync(id);
            if (tech == null) return false;

            _context.Technologies.Remove(tech);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
