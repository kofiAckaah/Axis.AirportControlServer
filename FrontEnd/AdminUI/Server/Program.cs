using AdminUI.Server.Extensions;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.AspNetCore.ResponseCompression;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);
var config = builder.Configuration;
builder.Services.ConfigureService(config);

var app = builder.Build();

app.ConfigureApplication();
await app.RunAsync();