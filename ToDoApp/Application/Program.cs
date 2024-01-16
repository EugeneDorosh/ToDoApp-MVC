using Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Service.Interfaces;
using Service.Repositories;
using Service.Interface.IValidation;
using System.Text;
using ToDoApp.Data;
using ToDoApp.Models;
using ToDoApp.Repositories;
using ToDoApp.Validation;
using ToDoApp.Validators;
using Service.TokenHandlers;
using Service.Interface.ITokenHandler;
using Infrastructure.Seeds;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ToDoContext>(options => 
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), 
                x => x.MigrationsAssembly("Infrastructure")
                ))
    ;

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

builder.Services.AddScoped<IUserValidationToDoApp, UserValidation>();
builder.Services.AddScoped<INoteValidationToDoApp, NoteValidation>();
builder.Services.AddScoped<ITaskValidationToDoApp, TaskValidation>();

builder.Services.AddScoped<IUserAuthenticationRepository, UserAuthenticationRepository>();
builder.Services.AddScoped<ITokenHandler, JwtTokenHandler>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//----------------------------------------------------------------------------
//Identity

builder.Services.AddIdentity<User, IdentityRole<Guid>>(o =>
{
    o.Password.RequireNonAlphanumeric = false;
    o.Password.RequireDigit = false;
    o.Password.RequireLowercase = false;
    o.Password.RequireUppercase= false;
    o.Password.RequiredLength = 8;
    o.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<ToDoContext>()
    .AddDefaultTokenProviders();

var jwtConfig = builder.Configuration.GetSection("JwtConfig");
string secretKey = jwtConfig["secret"];

if (string.IsNullOrEmpty(secretKey))
    throw new ArgumentNullException("secret key cannot be null or empty");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(o =>
    {
        o.SaveToken = true;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfig["validIssuer"],
            ValidAudience = jwtConfig["validAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

// username: string, password: string123, id: 42ec4429-49d4-44e0-6f11-08db2d459b02
// undo change in auth repository 

//----------------------------------------------------------------------------

builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "ToDoApp Api", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

// that is for seeding roles
//builder.Services.AddScoped<IDbContextSeed, SeedRole>();

builder.Services.AddScoped<RoleManager<IdentityRole<Guid>>>();

var app = builder.Build();

//using var scope = app.Services.CreateScope();
//var dbContext = scope.ServiceProvider.GetRequiredService<ToDoContext>();
//await dbContext.Database.MigrateAsync();

//// Seed the database with roles
//var dbContextSeed = scope.ServiceProvider.GetRequiredService<IDbContextSeed>();
//await dbContextSeed.SeedAsync(dbContext);

app.UseAuthentication();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
