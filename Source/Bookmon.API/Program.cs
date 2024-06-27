using Bookmon.API.Extensions;
using Bookmon.API.Middleware;
using Bookmon.API.Models.Requests;
using Bookmon.API.Validators;
using Bookmon.Domain;
using Bookmon.Domain.Entities;
using Bookmon.Domain.Validators;
using Bookmon.Infrastructure.EntityFramework;
using Bookmon.Infrastructure.IoC;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

AddVersioning(builder.Services);
AddMvc(builder.Services);
AddSwagger(builder.Services);
AddAuthentication(builder.Services);
AddCache(builder.Services, builder.Configuration);
AddServices(builder.Services);
AddRepositories(builder.Services, builder.Configuration);
AddControllerEndpoints(builder.Services);
AddCors(builder);

var app = builder.Build();
UseCors(app);

UserSwagger(app);
SeedData(app);

app.MapIdentityApi<User>();

UseMiddleware(app);
UseEndpoints(app, app.Environment);

app.MapControllers();

app.Run();

static void AddVersioning(IServiceCollection services)
{
    services.AddApiVersioning(o =>
    {
        o.ReportApiVersions = true;
        o.AssumeDefaultVersionWhenUnspecified = true;
        o.DefaultApiVersion = new ApiVersion(1, 0);
    });

    services.AddVersionedApiExplorer(o =>
    {
        o.SubstituteApiVersionInUrl = true;
    });
}

static void AddMvc(IServiceCollection services)
{
    services.AddHttpContextAccessor();

    AddValidators(services);

    services.AddRouting(opt => opt.LowercaseUrls = true);
}

static void AddSwagger(IServiceCollection services)
{
    services.AddSwaggerGen(c =>
    {
        c.DescribeAllParametersInCamelCase();
        c.SwaggerDoc(new ApiVersion(1, 0).ToString(), new OpenApiInfo { Title = "Bookmon.API", Version = new ApiVersion(1, 0).ToString() });
        //Add more versions if we need more versions

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter token",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });
    });
}

static void AddControllerEndpoints(IServiceCollection services)
{
    services.AddIdentityCore<User>()
     .AddEntityFrameworkStores<AuthDbContext>()
     .AddApiEndpoints();

    services.AddControllers();
    services.AddEndpointsApiExplorer();
}

static void AddCache(IServiceCollection services, IConfiguration configuration)
{
    services.AddCache(configuration);
}

static void AddServices(IServiceCollection services)
{
    services.AddDomainServices();
}

static void AddRepositories(IServiceCollection services, IConfiguration configuration)
{
    services.ConfigureAuthDatabase();
    services.AddRepositories(configuration);
}

static void AddValidators(IServiceCollection services)
{
    services.AddScoped<IValidator<OrderRequest>, OrderRequestValidator>();
    services.AddScoped<IValidator<Order>, OrderValidator>();
}

static void AddAuthentication(IServiceCollection services)
{
    services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
    services.AddAuthorizationBuilder();
}

static void AddCors(WebApplicationBuilder builder)
{
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(
            policy => policy
            .WithOrigins(
                "http://localhost:4200",
                "https://localhost:4200")
            .SetIsOriginAllowedToAllowWildcardSubdomains()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(origin => true));
    });
}

static void UseEndpoints(IApplicationBuilder app, IWebHostEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseHttpsRedirection();
    }

    app.UseRouting();
    app.UseAuthorization();
}

static void UseMiddleware(IApplicationBuilder app)
{
    app.UseMiddleware<ExceptionMiddleware>();
}

static void UseCors(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseCors();
}

static void UserSwagger(WebApplication app)
{
    app.UseSwagger();
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    app.UseSwaggerUI(options =>
    {
        foreach (var groupName in provider.ApiVersionDescriptions.Select(x => x.GroupName))
        {
            options.SwaggerEndpoint($"/swagger/{groupName}/swagger.json", groupName.ToUpperInvariant());
        }

        //add login identity endpoints to swagger
    });
}

static void SeedData(IApplicationBuilder app)
{
    app.SeedAuthDatabase();
    app.SeedCosmosDatabase();
}