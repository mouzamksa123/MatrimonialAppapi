

using matrimonial.Extensions;
using matrimonial.Filters;
using matrimonial.Models;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;
var configurationbuilder = builder.Configuration;
configurationbuilder.SetBasePath(env.ContentRootPath);
configurationbuilder.AddJsonFile($"appsettings.{env.EnvironmentName}.json", false, true);
configurationbuilder.AddEnvironmentVariables();
// Add services to the container.

// Named Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CorsPolicy",
        builder =>
        {
            string[] originArr = configurationbuilder["CORS:origin"].ToString().Split(",");
            string[] headersArr = configurationbuilder["CORS:headers"].ToString().Split(",");
            string[] methodsArr = configurationbuilder["CORS:methods"].ToString().Split(",");
            builder.WithOrigins(originArr).WithHeaders(headersArr).WithMethods(methodsArr);
        });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Matrimonial App API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
builder.Services.Configure<AppSettingsModel>(configurationbuilder);
builder.Services.AddJWTTokenServices(configurationbuilder);
builder.Services.AddMvc(
                config =>
                {
                    config.Filters.Add(typeof(CustomExceptionHandler));
                }
            );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseCors("CorsPolicy");
app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();

