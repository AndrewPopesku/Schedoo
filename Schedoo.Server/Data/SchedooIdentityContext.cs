using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Schedoo.Server.Helpers;
using Schedoo.Server.Models;

namespace Schedoo.Server.Data;

public class SchedooIdentityContext(DbContextOptions<SchedooIdentityContext> options) : IdentityDbContext<User>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.SeedRoles();
        builder.SeedUsers();
        builder.SeedUserRoles();
    }
}