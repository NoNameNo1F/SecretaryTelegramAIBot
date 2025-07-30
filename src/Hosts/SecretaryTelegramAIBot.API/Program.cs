using SecretaryTelegramAIBot.CrossCuttingConcern.Logging;
using SecretaryTelegramAIBot.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.WebHost.UseLogger(configuration =>
{
    var appSettings = new AppSettings();
    configuration.Bind(appSettings);
    return appSettings.Logging;
});

builder.Services.AddInfrastructure(opt => 
    configuration.GetSection("GenerativeAI").Bind(opt));

builder.Services.AddTaskManagementInfrastructure(opt => 
    configuration.GetSection("ThirdParties").Bind(opt));

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services
    .AddSwaggerDocumentation()
    .AddVersioning();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClients", policy =>
        policy.WithOrigins("https://localhost:5001", "https://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddAuthentication(configuration);
builder.Services.AddAuthorizationExtension();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

// app.UseExceptionHandler();
app.UseExceptionHandler(_ => { });
app.UseHsts();
app.UseHttpsRedirection();
app.UseCors("AllowClients");

// app.UseDefaultFiles();
// app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();