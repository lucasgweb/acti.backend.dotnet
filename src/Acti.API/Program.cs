using System.Net;
using System.Net.Mail;
using System.Text;
using Acti.API.ViewModels;
using Acti.Application.Dtos;
using Acti.Application.Interfaces;
using Acti.Application.Services;
using Acti.Domain.Entities;
using Acti.Domain.Repositories;
using Acti.Infra.Context;
using Acti.Infra.Email;
using Acti.Infra.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

#region AutoMapper

builder.Services.AddAutoMapper(config =>
{
    config.CreateMap<CreateUserViewModel, UserDTO>().ReverseMap();
    config.CreateMap<User, UserDTO>().ReverseMap();
}, typeof(Program));

#endregion

#region DI

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IAuthServices, AuthServices>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddSingleton(builder.Configuration);

builder.Services.AddControllers();

#endregion

#region Jwt

var jwtSettings = builder.Configuration.GetSection("JwtSettings");

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings["SecretKey"])),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

#endregion

var emailSettings = builder.Configuration.GetSection("EmailSettings");

builder.Services.AddSingleton<SmtpClient>(s =>
{
    var smtpClient = new SmtpClient(emailSettings["Server"], int.Parse(emailSettings["Port"]));
    smtpClient.UseDefaultCredentials = false;
    smtpClient.Credentials = new NetworkCredential(emailSettings["Username"], emailSettings["Password"]);
    smtpClient.EnableSsl = bool.Parse(emailSettings["UseSSL"]);
    return smtpClient;
});

builder.Services.AddTransient<IEmailSender, EmailSender>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    };

    c.AddSecurityRequirement(securityRequirement);
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();
// Configure the HTTP request pipeline.


app.UseCors(builder => builder
    .AllowAnyHeader()
    .AllowAnyMethod()
    .SetIsOriginAllowed(host => true)
    .AllowCredentials()
);

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();