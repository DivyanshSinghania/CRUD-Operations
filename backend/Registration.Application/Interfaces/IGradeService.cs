using Registration.Application.DTOs;
using Registration.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Registration.Application.Services
{
    public interface IGradeService
{
    Task<IEnumerable<Grade>> GetAllGradesAsync();
    Task<Grade?> GetGradeByIdAsync(int gradeId);       // Allow null
    Task<Grade> AddGradeAsync(GradeDTO gradeDto);
    Task<Grade?> UpdateGradeAsync(int gradeId, GradeDTO gradeDto);  // Allow null
    Task<bool> DeleteGradeAsync(int gradeId);
}

}
