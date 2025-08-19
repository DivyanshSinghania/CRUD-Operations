using Registration.Application.DTOs;
using Registration.Application.Interfaces;
using Registration.Domain.Entities;
using Registration.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Registration.Application.Repositories
{
    public class EmployeeRepo : IEmployeeService
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
        {
            return await _context.Employees
                .Select(e => new EmployeeDto
                {
                    Employee_Id = e.Employee_Id,
                    Name = e.Name,
                    Email = e.Email,
                    Department = e.Department,
                    Designation = e.Designation
                })
                .ToListAsync();
        }

        public async Task<EmployeeDto?> GetEmployeeByIdAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return null;

            return new EmployeeDto
            {
                Employee_Id = employee.Employee_Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                Designation = employee.Designation
            };
        }

        public async Task<EmployeeDto> CreateEmployeeAsync(EmployeeDto employeeDto)
        {
            var employee = new Employee
            {
                Employee_Id = employeeDto.Employee_Id,
                Name = employeeDto.Name,
                Email = employeeDto.Email,
                Department = employeeDto.Department,
                Designation = employeeDto.Designation,
                LastModified = DateTime.UtcNow
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            employeeDto.Employee_Id = employee.Employee_Id;
            return employeeDto;
        }

        public async Task<bool> UpdateEmployeeAsync(int id, EmployeeDto employeeDto)
        {
            var existing = await _context.Employees.FindAsync(id);
            if (existing == null) return false;

            existing.Name = employeeDto.Name;
            existing.Email = employeeDto.Email;
            existing.Department = employeeDto.Department;
            existing.Designation = employeeDto.Designation;
            existing.LastModified = DateTime.UtcNow; // Update LastModified timestamp

            _context.Employees.Update(existing);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return false;

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return true;
        }
        
        public async Task<PagedResult<EmployeeDto>> GetEmployeesPagedAsync(int page, int pageSize)
        {
            var query = _context.Employees.AsQueryable();

            var totalCount = await query.CountAsync();

            var employees = await query
                .OrderBy(e => e.Employee_Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new EmployeeDto
            {
                Employee_Id = e.Employee_Id,
                Name = e.Name,
                Email = e.Email,
                Department = e.Department,
                Designation = e.Designation
            })
            .ToListAsync();

            return new PagedResult<EmployeeDto>
        {
            Items = employees,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
}

    }
}
