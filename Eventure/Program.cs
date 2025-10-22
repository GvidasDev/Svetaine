using Eventure.Data;
using Eventure.Services;
using Eventure.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ---------------------
// CORS: leidžiame React dev server
// ---------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactDev", policy =>
    {
        policy.WithOrigins("https://localhost:44405")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ---------------------
// SQLite DbContext
// ---------------------
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// ---------------------
// Services
// ---------------------
builder.Services.AddScoped<IEventService, EventService>();

// ---------------------
// Controllers & Swagger
// ---------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ---------------------
// Build app
// ---------------------
var app = builder.Build();

// ---------------------
// Automatinis DB sukūrimas
// ---------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    db.Database.EnsureCreated();
}

// ---------------------
// Middleware
// ---------------------
app.UseCors("AllowReactDev");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// ---------------------
// Map controllers + fallback
// ---------------------
app.MapControllers();

app.MapFallbackToFile("index.html");

// ---------------------
// Swagger (development)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
