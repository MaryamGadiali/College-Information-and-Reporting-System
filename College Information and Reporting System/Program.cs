using College_Information_and_Reporting_System;
using College_Information_and_Reporting_System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

// dotnet ef migrations remove
//dotnet ef migrations add InitialCreate
//dotnet ef database update
//for creating tables from the models (migration folder)

var builder = WebApplication.CreateBuilder(args);

// Registers controllers
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
