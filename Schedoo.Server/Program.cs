using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Schedoo.Server.Data;
using Schedoo.Server.Models;
using Schedoo.Server.Services;

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        builder =>
        {
            builder.WithOrigins("https://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddSingleton<ScrapperService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SchedooContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SchedooContext")));

builder.Services.AddDbContext<SchedooIdentityContext>(x => 
    x.UseSqlite("DataSource=app.db"));

builder.Services.AddIdentityCore<User>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<SchedooIdentityContext>()
    .AddApiEndpoints();

builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole",
            policy => policy.RequireRole("Administrator"));
    options.AddPolicy("RequireGroupLeaderRole",
        policy => policy.RequireRole("Group Leader"));
    options.AddPolicy("RequireStudentRole",
        policy => policy.RequireRole("Student"));
});


var app = builder.Build();

app.UseDefaultFiles();

app.UseHttpsRedirection();

app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.MapIdentityApi<User>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
