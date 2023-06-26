using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using sierra.DTO.Product;
using sierra.DTO.Customer;
using sierra.DTO.Order;
using sierra.Middleware;
using sierra.Model.Entities;
using sierra.Repository;
using sierra.Repository.Interfaces;

var configuration = new MapperConfiguration(cfg =>
  {
    cfg.CreateMap<ProductRequest, Product>();
    cfg.CreateMap<Product, ProductResponse>();

    cfg.CreateMap<CustomerRequest, Customer>();
    cfg.CreateMap<Customer, CustomerResponse>();

    cfg.CreateMap<Order, OrderResponse>();
  }
);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton(sp => configuration.CreateMapper());
builder.Services.AddScoped(sp =>  new SierraContext(sp.GetService<IConfiguration>().GetConnectionString("sierra_db")));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services
  .AddHttpContextAccessor()
  .AddAuthorization()
  .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
  }
  );


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
  name: "api",
  pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

// Configure Exception Handler middleware
app.UseMiddleware<ExceptionMiddleware>();

app.Run();
