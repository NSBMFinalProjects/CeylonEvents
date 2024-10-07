using System;
using System.Net.Mail;
using System.Text;
using EventHandler.Data;
using EventHandler.Helper;
using EventHandler.Models.Entities;
using EventHandler.Services.EmailService;
using EventHandler.Services.EventService;
using EventHandler.Services.RoleService;
using MailKit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RestApiwithDB.Helper;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var JWTSetting = builder.Configuration.GetSection("JWTSetting");

builder.Services.AddDbContext<EventDbContext>(options => 
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"));
});

builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IEventService, Eventservice>();

builder.Services.AddTransient<UploadHandler>();
builder.Services.AddScoped<RoleService>();

builder.Services.AddSingleton<SmtpClient>();
builder.Services.AddSingleton<IEmailService, EmailService>();

StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];


builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
    options.Password.RequiredUniqueChars = 1;
})
.AddEntityFrameworkStores<EventDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication(opt => {
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt => {
    opt.SaveToken = true;
    opt.RequireHttpsMetadata = false;
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = JWTSetting["ValidAudience"],
        ValidIssuer = JWTSetting["ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTSetting.GetSection("securityKey").Value!))

    };
});

builder.Services.AddSwaggerGen(c => {
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization Example : 'Bearer token validator",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement(){
        {
            new OpenApiSecurityScheme{
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "outh2",
                Name="Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });

});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
    options.Cookie.HttpOnly = true; 
    options.Cookie.IsEssential = true; 
});

builder.Services.AddDistributedMemoryCache();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(typeof(MappingProfiles));

var app = builder.Build();

app.Lifetime.ApplicationStopping.Register(() =>
{
    var smtpClient = app.Services.GetRequiredService<MailKit.Net.Smtp.SmtpClient>();
    smtpClient.Disconnect(true);
    smtpClient.Dispose();
});


app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Uploads")),
    RequestPath = "/Uploads"
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options =>
{
    options.AllowAnyHeader();
    options.AllowAnyMethod();
    options.AllowAnyOrigin();
});

app.UseHttpsRedirection();

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
