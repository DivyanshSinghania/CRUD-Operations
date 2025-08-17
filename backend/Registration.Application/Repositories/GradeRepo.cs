using Registration.Application.DTOs;
using Registration.Application.Services; 
using Registration.Domain.Entities;
using Registration.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Registration.Application.Repositories
{
    public class GradeRepo : IGradeService
    {
        private readonly ApplicationDbContext _context;

        public GradeRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Grade>> GetAllGradesAsync()
        {
            return await _context.Grades.ToListAsync();
        }

        public async Task<Grade?> GetGradeByIdAsync(int gradeId)
        {
            return await _context.Grades.FirstOrDefaultAsync(g => g.GradeId == gradeId);
        }

        public async Task<Grade> AddGradeAsync(GradeDTO gradeDto)
{
    var grade = new Grade
    {
        GradeId = gradeDto.GradeId,        // Assign the manual ID from DTO
        GradeLevel = gradeDto.GradeLevel,
        GradeDescription = gradeDto.GradeDescription
    };

    _context.Grades.Add(grade);
    await _context.SaveChangesAsync();

    return grade;
}


        public async Task<Grade?> UpdateGradeAsync(int gradeId, GradeDTO gradeDto)
        {
            var existingGrade = await _context.Grades.FirstOrDefaultAsync(g => g.GradeId == gradeId);
            if (existingGrade == null) return null;

            existingGrade.GradeLevel = gradeDto.GradeLevel;
            existingGrade.GradeDescription = gradeDto.GradeDescription;

            _context.Grades.Update(existingGrade);
            await _context.SaveChangesAsync();

            return existingGrade;
        }

        public async Task<bool> DeleteGradeAsync(int gradeId)
        {
            var grade = await _context.Grades.FirstOrDefaultAsync(g => g.GradeId == gradeId);
            if (grade == null) return false;

            _context.Grades.Remove(grade);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
