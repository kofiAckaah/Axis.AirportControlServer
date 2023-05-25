using AircraftAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);
//builder.Configuration
//    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
var config = builder.Configuration;
builder.Services.ConfigureServices(config);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
