using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Registration.Application.DTOs;
using Registration.Application.Interfaces;
using Registration.Application.Repositories; // RoleRepo lives here
using Registration.Domain.Entities;
using Registration.Persistence.DbContext;
using Xunit;

public class RoleRepo_UnitTests
{
    private static ApplicationDbContext NewInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("RoleDb_" + Guid.NewGuid()) // unique per test
            .Options;

        var ctx = new ApplicationDbContext(options);

        // If your Role entity requires unique index in prod, InMemory won’t enforce it.
        // Your service should check duplicates before insert—these tests verify that.
        return ctx;
    }

    private static IRoleService NewService(ApplicationDbContext ctx)
        => new RoleRepo(ctx); // Your merged service/repo class that implements IRoleService

    [Fact]
    public async Task CreateRole_Should_Save_When_Valid()
    {
        var ctx = NewInMemoryContext();
        var svc = NewService(ctx);

        var dto = new RoleDto { Role_Id = 1, Role_Title = "Developer", Project_Name = "Internal Tools" };
        var created = await svc.CreateRoleAsync(dto);

        var saved = await ctx.Roles.FindAsync(1);
        saved.Should().NotBeNull();
        saved!.Role_Title.Should().Be("Developer");
        saved.Project_Name.Should().Be("Internal Tools");
    }

    [Fact]
    public async Task CreateRole_Should_Fail_When_Duplicate_Id()
    {
        var ctx = NewInMemoryContext();
        ctx.Roles.Add(new Role { Role_Id = 200, Role_Title = "Mgr", Project_Name = "Project AAA" });
        await ctx.SaveChangesAsync();

        var svc = NewService(ctx);
        var dto = new RoleDto { Role_Id = 200, Role_Title = "Mgr2", Project_Name = "Project BBB" };

        // If your service throws, assert for exception. If it returns a result, adjust accordingly.
        var act = async () => await svc.CreateRoleAsync(dto);
        await act.Should().ThrowAsync<Exception>(); // e.g., InvalidOperationException in your code
    }

    [Fact]
    public async Task GetRoleById_Should_Return_Null_When_NotFound()
    {
        var ctx = NewInMemoryContext();
        var svc = NewService(ctx);

        var result = await svc.GetRoleByIdAsync(999);
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateRole_Should_Return_True_When_Exists()
    {
        var ctx = NewInMemoryContext();
        ctx.Roles.Add(new Role { Role_Id = 300, Role_Title = "QA", Project_Name = "Alpha" });
        await ctx.SaveChangesAsync();

        var svc = NewService(ctx);
        var ok = await svc.UpdateRoleAsync(300, new RoleDto { Role_Id = 300, Role_Title = "QA Lead", Project_Name = "Alpha 2" });

        ok.Should().BeTrue();
        var saved = await ctx.Roles.FindAsync(300);
        saved!.Role_Title.Should().Be("QA Lead");
        saved.Project_Name.Should().Be("Alpha 2");
    }

    [Fact]
    public async Task UpdateRole_Should_Return_False_When_NotFound()
    {
        var ctx = NewInMemoryContext();
        var svc = NewService(ctx);

        var ok = await svc.UpdateRoleAsync(404, new RoleDto { Role_Id = 404, Role_Title = "X", Project_Name = "Y" });
        ok.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteRole_Should_Return_True_When_Exists()
    {
        var ctx = NewInMemoryContext();
        ctx.Roles.Add(new Role { Role_Id = 500, Role_Title = "Ops", Project_Name = "OpsProj" });
        await ctx.SaveChangesAsync();

        var svc = NewService(ctx);
        var ok = await svc.DeleteRoleAsync(500);
        ok.Should().BeTrue();

        var gone = await ctx.Roles.FindAsync(500);
        gone.Should().BeNull();
    }

    [Fact]
    public async Task DeleteRole_Should_Return_False_When_NotFound()
    {
        var ctx = NewInMemoryContext();
        var svc = NewService(ctx);

        var ok = await svc.DeleteRoleAsync(12345);
        ok.Should().BeFalse();
    }
}
