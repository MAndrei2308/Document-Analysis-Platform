using ContentCreationTool.Api.Application.Repositories;
using ContentCreationTool.Api.Infrastructure;
using ContentCreationTool.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IContentItemRepository, ContentItemRepository>();
builder.Services.AddScoped<ITextDocumentRepository, TextDocumentRepository>();
builder.Services.AddScoped<IImageDocumentRepository, ImageDocumentRepository>();
builder.Services.AddHttpClient<IOllamaService, OllamaService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:11434");
    //client.DefaultRequestHeaders.Add("Accept", "application/json");
    //client.DefaultRequestHeaders.Add("User-Agent", "ContentCreationTool");
    //client.DefaultRequestHeaders.Add("Authorization", "Bearer YOUR_API_KEY"); // Replace with your actual API key
});
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Content Creation Tool API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/openapi/v1.json", " v1"));
    app.MapOpenApi();
}

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
