using College_Information_and_Reporting_System;
using College_Information_and_Reporting_System.Data;
using College_Information_and_Reporting_System.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.OpenApi;
using System.Text.Json.Serialization;

// dotnet ef migrations remove
//dotnet ef migrations add InitialCreate
//dotnet ef database update
//for creating tables from the models (migration folder)

var builder = WebApplication.CreateBuilder(args);

// Registers controllers
//builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
    //.AddJsonOptions(o=>o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())); //for enum dropdowns
builder.Services.AddScoped<IStudentService, StudentService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "College Information and Reporting System API",
        Version = "v1",
        Description = "API for managing students, courses, departments, and attendance"
    });
});

// Configure EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));

//seed data
builder.Services.AddHostedService<DatabaseSeeder>();



var app = builder.Build();

//app.UseDeveloperExceptionPage();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStatusCodePages(context =>
{
    if (context.HttpContext.Response.StatusCode == 404)
    {
        context.HttpContext.Response.ContentType = "application/json";
        return context.HttpContext.Response.WriteAsync(
            "{\"error\":\"Resource not found\"}"
        );
    }

    return Task.CompletedTask;
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=WebApp}/{action=Index}");

app.Run();
