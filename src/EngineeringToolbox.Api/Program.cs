using EngineeringToolbox.Api.Config;
using EngineeringToolbox.Api.Extensions;
using EngineeringToolbox.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

builder.Configuration.AddLoggerConfig();

builder.Services.ResolveSettings(builder.Configuration);

builder.Services.AddIdentityConfig(builder.Configuration);

builder.Services.AddAppConfig();

builder.Services.AddDependenciesResolver();

builder.Services.AddSwaggerConfig();

var app = builder.Build();

app.UseSwaggerConfig();

app.UseAppConfig(app.Environment);

app.Run();
