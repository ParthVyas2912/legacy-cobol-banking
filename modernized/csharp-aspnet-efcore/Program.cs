using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Source.Data;
using StackExchange.Redis;
using Hangfire;
using Hangfire.PostgreSql;

var builder = WebApplication.CreateBuilder(args);

// 1. Configure Logging
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/app_log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();

// 2. Add Services to the Container

// Register ApplicationDbContext with PostgreSQL Provider
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Register Hangfire with PostgreSQL as its storage
builder.Services.AddHangfire(configuration =>
    configuration.UsePostgreSqlStorage(connectionString));
builder.Services.AddHangfireServer();

// Register Caching with Redis
var redisConnection = builder.Configuration.GetConnectionString("RedisConnection");
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnection;
});

// Add Controllers with Razor Views support
builder.Services.AddControllersWithViews();

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Authentication (JWT Bearer)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Add CORS (if needed for cross-origin requests)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// Add IHttpClientFactory support
builder.Services.AddHttpClient();

// 3. Build App
var app = builder.Build();

// 4. Configure Middleware Pipeline

// Developer Exception Page for Development, Global Exception Handling for Production
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// HTTPS Redirection
app.UseHttpsRedirection();

// Serve Static Files
app.UseStaticFiles();

// Enable Routing
app.UseRouting();

// Enable CORS
app.UseCors("AllowAll");

// Authentication and Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Enable Hangfire Dashboard
app.UseHangfireDashboard("/hangfire");

// Map Health Check Endpoint (not specified explicitly but common practice)
app.MapHealthChecks("/health");

// Map Controller Endpoints
app.MapControllers();

// Map Razor Pages or Views
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Run the application
app.Run();