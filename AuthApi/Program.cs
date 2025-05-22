using Business.Services;
using Data.Contexts;
using Data.Entities;
using Data.Seeders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<UserEntity, IdentityRole>(options =>
{
  options.SignIn.RequireConfirmedAccount = false;
  options.User.RequireUniqueEmail = true;
  options.Password.RequiredLength = 8;
})
  .AddEntityFrameworkStores<DataContext>()
  .AddDefaultTokenProviders();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(options =>
  {
    options.TokenValidationParameters = new TokenValidationParameters
    {
      ValidateIssuer = true,
      ValidateAudience = true,
      ValidateLifetime = true,
      ValidateIssuerSigningKey = true,
      ValidIssuer = builder.Configuration["Jwt:Issuer"],
      ValidAudience = builder.Configuration["Jwt:Audience"],
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
  });

builder.Services.AddScoped<AuthService>();

var app = builder.Build();

app.UseCors(options =>
{
  options.AllowAnyOrigin()
         .AllowAnyMethod()
         .AllowAnyHeader();
});

await DatabaseSeeder.SeedRolesAndAdminAsync(app.Services);

app.MapOpenApi();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
