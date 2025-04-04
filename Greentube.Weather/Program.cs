using Greentube.Weather.Application;
using Greentube.Weather.CustomMiddlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddWeatherModule(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "custom",
        policy  =>
        {
            policy
                .WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Value ?? "*")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
});

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".Greentube.Weather";
    options.IdleTimeout = TimeSpan.FromDays(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandler>();

app.UseHttpsRedirection();

app.UseCors("custom");

app.UseAuthorization();

app.UseSession();

app.MapControllers();

app.Run();