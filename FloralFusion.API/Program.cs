using FloralFusion.Domain.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using FloralFusion.Persistanse.Reflections;
using FloralFusion.Domain.Interfaces.Admin;
using FloralFusion.Infrastructure.Repositories.AdminRepositories;
using FloralFusion.Domain.Interfaces.DeliveryManagement;
using FloralFusion.Infrastructure.Repositories.DeliveryManagementRepositories;
using FloralFusion.Domain.Interfaces.Flowers_Management;
using FloralFusion.Infrastructure.Repositories.FlowersManagementRepositories;
using FloralFusion.Domain.Interfaces.Notifications;
using FloralFusion.Infrastructure.Repositories.NotificationRepositories;
using FloralFusion.Domain.Interfaces.Order_Management;
using FloralFusion.Infrastructure.Repositories.OrderManagementRepositories;
using FloralFusion.Domain.Interfaces.Payment_Processing;
using FloralFusion.Infrastructure.Repositories.PaymentMethodRepositories;
using FloralFusion.Domain.Interfaces.Reports_and_Analytics;
using FloralFusion.Infrastructure.Repositories.ReportsAndAnalyticsRepositories;
using FloralFusion.Domain.Interfaces.Review_and_Rating_System;
using FloralFusion.Infrastructure.Repositories.ReviewAndRatingRepositories;
using FloralFusion.Domain.Interfaces;
using FloralFusion.Infrastructure.Repositories;
using FloralFusion.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using FloralFusion.Persistanse.OuterServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using FloralFusion.API.CustomMiddlwares;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Swagger config
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "FlorallFusion",
        Description = "Online flower's shop",
        TermsOfService = new Uri("https://github.com/RaisaBadal"),
        Contact = new OpenApiContact
        {
            Name = "Contact Me",
            Url = new Uri("https://www.linkedin.com/in/raisa-badal-567488245/")
        },
        License = new OpenApiLicense
        {
            Name = "License, Source Code",
            Url = new Uri("https://github.com/RaisaBadal")
        }
    });


    opt.AddSecurityDefinition("auth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        Name = "Authorization",
        In = ParameterLocation.Header,
        Description = "Enter token here"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "auth"
                }
            },
            Array.Empty<string>()
                }
            });
});
#endregion


builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<FloralFusionDb>().
    AddDefaultTokenProviders();

builder.Services.AddDbContext<FloralFusionDb>(
    str =>
    {
        str.UseSqlServer(builder.Configuration.GetConnectionString("FlowerDb"));
    });

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   ValidIssuer = "http://localhost:5169",
                   ValidAudience = "http://localhost:5169",
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("65E255FF-F399-42D4-9C7F-D5D08457FGEC285")),
               };
           });

builder.Services.AddScoped<IManageFlowers, ManageFlowerRepos>();
builder.Services.AddScoped<IManageOrders,ManageOrdersRepos>();
builder.Services.AddScoped<IDeliveryOptions,DeliveryOptionsRepositories>();
builder.Services.AddScoped<IFlower,FlowerRepos>();
builder.Services.AddScoped<IFlowerCatalogManagement,FlowerCatalogManagementRepos>();
builder.Services.AddScoped<INotifications, NotificationsRepos>();
builder.Services.AddScoped<IPlaceOrder, PlaceOrderRepos>();
builder.Services.AddScoped<IShoppingCart, ShoppingCartRepos>();
builder.Services.AddScoped<IPaymentMethods, PaymentRepos>();
builder.Services.AddScoped<ISalesReports, SalesReportsRepos>();
builder.Services.AddScoped<IUserReviewsAndRatings, UserReviewsAndRatingsRepos>();
builder.Services.AddScoped<IUniteOfWork, UniteOfWork>();
builder.Services.AddScoped<UserManager<User>>();
builder.Services.AddScoped<RoleManager<IdentityRole>>();
builder.Services.AddScoped<SignInManager<User>>();
builder.Services.AddSingleton<SmtpService>();
builder.Services.AddMemoryCache();

var domainAssemblyServices = Assembly.Load("FloralFusion.Application");
builder.Services.AddInjectServices(domainAssemblyServices);
builder.Services.AddAutoMapper(typeof(FloralFusion.Application.Mapper.AutoMapper));
builder.Services.AddLogging(opt => opt.AddConsole());


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<RateLimitingMiddleware>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
