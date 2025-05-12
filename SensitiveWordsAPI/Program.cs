using Microsoft.OpenApi.Models;
using SensitiveWordsAPI.Data;
using SensitiveWordsAPI.Middleware;
using SensitiveWordsAPI.Repositories;
using SensitiveWordsAPI.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<ISensitiveWordRepository, SensitiveWordRepository>();
builder.Services.AddScoped<IWordFilterService, WordFilterService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Specify OpenAPI version
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SensitiveWordsAPI",
        Version = "v1",
        Description = "API for handling sensitive words",
        Contact = new OpenApiContact
        {
            Name = "Wouter Human",
            Email = "wouterhuman@gmail.com",
        }
    });

    // Enable Swagger annotations
    options.EnableAnnotations();

    // Include XML comments
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});



var app = builder.Build();

// Configure middleware to serve generated Swagger as a JSON endpoint
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "SensitiveWordsAPI v1");
    });
}

app.UseHttpsRedirection();

app.UseRouting();

// Add custom middleware for exception handling
app.UseMiddleware<ApiKeyMiddleware>();

// Add autorization
app.UseAuthorization();

//Map the controllers
app.MapControllers();

app.Run();
