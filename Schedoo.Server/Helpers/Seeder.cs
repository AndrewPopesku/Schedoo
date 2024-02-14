using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Schedoo.Server.Models;

namespace Schedoo.Server.Helpers;

public static class Seeder
{
    public static void SeedRoles(this ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<IdentityRole>()
            .HasData(new List<IdentityRole>
            {
                new()
                {
                    Id = "c1fcf1c5-e085-4e04-804c-e51eab84d5d6",
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                },
                new()
                {
                    Id = "eb966f18-13f0-4590-971f-8c0ace478171",
                    Name = "Group Leader",
                    NormalizedName = "GROUP LEADER"
                },
                new()
                {
                    Id = "2ee57d9c-7d76-419a-95b4-716c8acb3c52",
                    Name = "Student",
                    NormalizedName = "STUDENT"
                },
            });
    }

    public static void SeedUsers(this ModelBuilder modelBuilder)
    {
        var hasher = new PasswordHasher<User>();

        modelBuilder
            .Entity<User>()
            .HasData(new List<User>
            {
                new()
                {
                    Id = "9f4a48ff-1244-49ef-80ea-c70eb70d27e0",
                    Name = "Admin",
                    SurName = "Admin",
                    Patronymic = "Admin",
                    UserName = "admin@test.com",
                    NormalizedUserName = "ADMIN@TEST.COM",
                    Email = "admin@test.com",
                    NormalizedEmail = "ADMIN@TEST.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Admin@123"),
                },
                new()
                {
                    Id = "6cc6af6a-edc3-4715-9b09-f7638543fe68",
                    Name = "Vasya",
                    SurName = "Hrosu",
                    Patronymic = "Georgiyovich",
                    UserName = "groupleader@test.com",
                    NormalizedUserName = "GROUPLEADER@TEST.COM",
                    Email = "groupleader@test.com",
                    NormalizedEmail = "GROUPLEADER@TEST.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Leader@123"),
                },
                new()
                {
                    Id = "98020454-c731-45e3-980c-1d1cec0ba390",
                    Name = "Misha",
                    SurName = "Paranchich",
                    Patronymic = "Yuriyovich",
                    UserName = "student@test.com",
                    NormalizedUserName = "STUDENT@TEST.COM",
                    Email = "student@test.com",
                    NormalizedEmail = "STUDENT@TEST.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Student@123"),
                },
            });
    }

    public static void SeedUserRoles(this ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<IdentityUserRole<string>>()
            .HasData(new List<IdentityUserRole<string>>
            {
                new()
                {
                    UserId = "9f4a48ff-1244-49ef-80ea-c70eb70d27e0",
                    RoleId = "c1fcf1c5-e085-4e04-804c-e51eab84d5d6",
                },
                new()
                {
                    UserId = "6cc6af6a-edc3-4715-9b09-f7638543fe68",
                    RoleId = "eb966f18-13f0-4590-971f-8c0ace478171",
                },
                new()
                {
                    UserId = "98020454-c731-45e3-980c-1d1cec0ba390",
                    RoleId = "2ee57d9c-7d76-419a-95b4-716c8acb3c52",
                }
            });
    }
}