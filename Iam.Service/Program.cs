using Iam.Infrastructure;

using Iam.Application;
using Iam.Infrastructure.Services;
using Iam.Persistence;
using Iam.Service.Middleware;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.Kestrel.Core;

using Serilog;

using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
if (builder.Environment.IsProduction() || builder.Environment.IsEnvironment("Docker"))
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        // Az 8080-as portot a gRPC fogja használni HTTP/2-n
        options.ListenAnyIP(8080, o => o.Protocols = HttpProtocols.Http2);

        // Az 8081-es porton a REST API (Swagger, controller, health, stb.)
        options.ListenAnyIP(8081, o => o.Protocols = HttpProtocols.Http1AndHttp2);
    });
}
builder.Services.AddGrpc();

builder.Host.UseSerilog((context, loggerConfig) => loggerConfig
    .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
    .ReadFrom.Configuration(context.Configuration));

builder.Services.AddHttpContextAccessor();

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddApplicationServices();


builder.Services.AddMemoryCache();

// Add services to the container.
builder.Services.AddProblemDetails();

var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

if (allowedOrigins != null && allowedOrigins.Length > 0)
{
    builder.Services.AddCors(opt =>
    {
        opt.AddPolicy("web", p => p
        .WithOrigins(allowedOrigins)
        .AllowAnyHeader()
        .AllowAnyMethod());
    });
}

builder.Services.AddAppAuthentication(builder.Configuration);

builder.Services.AddAuthorization();

builder.Services.AddControllers();
//builder.Services.AddFastEndpoints();

builder.Services.AddOpenApiDocument(config =>
{
    config.Title = "AttrectoTest API";
    config.Version = "v1";

    // Security Definition (Bearer JWT)
    config.AddSecurity("JWT", new NSwag.OpenApiSecurityScheme
    {
        Type = NSwag.OpenApiSecuritySchemeType.ApiKey,
        Name = "Authorization",
        In = NSwag.OpenApiSecurityApiKeyLocation.Header,
        Description = "Type into the textbox: Bearer {your JWT token}."
    });

    // Security Requirement (apply globally)
    config.OperationProcessors.Add(
        new NSwag.Generation.Processors.Security.AspNetCoreOperationSecurityScopeProcessor("JWT"));
});

var app = builder.Build();


app.UseCors("web");
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
//app.UseExceptionHandler();



if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    // Add OpenAPI 3.0 document serving middleware
    // Available at: http://localhost:<port>/swagger/v1/swagger.json
    app.UseOpenApi();

    // Add web UIs to interact with the document
    // Available at: http://localhost:<port>/swagger
    app.UseSwaggerUi();

    // Add ReDoc UI to interact with the document
    // Available at: http://localhost:<port>/redoc
    app.UseReDoc(options =>
    {
        options.Path = "/redoc";
    });
}
app.MapControllers();
app.MapGet("/", () => Results.Redirect("/swagger"));

app.UseStaticFiles();

app.Services.RunDatabaseMigrations();

app.MapGrpcService<IamGrpcService>();

app.Run();
