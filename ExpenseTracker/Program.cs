using ExpenseTracker.Core;
using ExpenseTracker.Data;
using ExpenseTracker.Filters.CategoryFilters;
using ExpenseTracker.Filters.ExceptionFilters;
using ExpenseTracker.Filters.OperationFilter;
using ExpenseTracker.Filters.TransactionFilters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Expense Tracker API v1", Version = "Version 1" });
    //c.SwaggerDoc("v2", new OpenApiInfo { Title = "Expense Tracker API v2", Version = "Version 2" });

    c.OperationFilter<AuthorizationHeaderOperationFilter>();
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });
});


//Dependency Injection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<Category_ValidateCategoryIdFilterAttribute>();
builder.Services.AddScoped<Transaction_ValidateTransactionIdFilterAttribute>();
builder.Services.AddScoped<Transaction_HandleUpdateExceptionsFilterAttribute>();
builder.Services.AddScoped<Category_HandleUpdateExceptionsFilterAttribute>();

// API versioning
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "ExpenseTrackerAPI v1");
        //options.SwaggerEndpoint("/swagger/v2/swagger.json", "ExpenseTrackerAPI v2");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

