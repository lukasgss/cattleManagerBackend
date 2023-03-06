global using Microsoft.EntityFrameworkCore;
using System.Text;
using AutoMapper;
using CattleManager.Application.Application.Common.Interfaces.Authentication;
using CattleManager.Application.Application.Common.Interfaces.Entities.Users;
using CattleManager.Application.Application.Common.Marker;
using CattleManager.Application.Application.Middlewares;
using CattleManager.Application.Application.Services.Authentication;
using CattleManager.Application.Application.Services.Entities;
using CattleManager.Application.Infrastructure.Persistence;
using CattleManager.Application.Infrastructure.Persistence.DataContext;
using CattleManager.Application.Infrastructure.Persistence.Mappings;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;

var mappingConfig = new MapperConfiguration(mc => mc.AddProfile(new MappingProfile()));
IMapper mapper = mappingConfig.CreateMapper();

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    // Add services to the container.
    builder.Services.AddSwaggerGen();

    builder.Services.AddSingleton(mapper);

    builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IPasswordService, PasswordService>();
    builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>(_ => new JwtTokenGenerator(
        builder.Configuration["AppSettings:Issuer"],
        builder.Configuration["AppSettings:Audience"],
        builder.Configuration["AppSettings:SecretKey"]));

    builder.Services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();

    builder.Services.AddAuthentication("Bearer")
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["AppSettings:Issuer"],
                ValidAudience = builder.Configuration["AppSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:SecretKey"]))
            };
        }
    );
}

var app = builder.Build();

app.UseMiddleware<ErrorHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
