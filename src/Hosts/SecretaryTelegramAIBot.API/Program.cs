var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddInfrastructure(opt => configuration.GetSection("ThirdParties").Bind(opt));

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();