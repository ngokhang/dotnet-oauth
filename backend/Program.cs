using System.Net;
using System.Text;
using System.Text.Json;
using backend.Models;
using backend.Models.DTOs;
using LearnOAuth.Context;
using LearnOAuth.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddAutoMapper(typeof(Program).Assembly);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowSpecificOrigin", builder =>
  {
    builder.WithOrigins("http://localhost:5173") // Specify the allowed origin(s)
      .AllowAnyHeader()
      .AllowAnyMethod();
  });
});
builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
      IConfigurationSection googleAuthNSection =
      config.GetSection("Authentication:Google");
      options.ClientId = googleAuthNSection["ClientId"];
      options.ClientSecret = googleAuthNSection["ClientSecret"];
    });

builder.Services.Configure<AuthenticationDbSettting>(
  builder.Configuration.GetSection("AuthenticationDB")
);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
  options.TokenValidationParameters = new TokenValidationParameters
  {
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = builder.Configuration["Jwt:Issuer"],
    ValidAudience = builder.Configuration["Jwt:Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
  };
});

builder.Services.AddAuthorization(options =>
  {

    options.AddPolicy("Roles=user",
        authBuilder =>
        {
          authBuilder.RequireRole("user");
        });

  });

builder.Services.AddSingleton<HttpServices>();
builder.Services.AddSingleton<UserServices>();
builder.Services.AddSingleton<JwtServices>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
  app.UseExceptionHandler("/error-development");
}
else
{
  app.UseExceptionHandler("/error");
}

app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler(appError =>
{
  appError.Run(async context =>
  {
    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
    context.Response.ContentType = "application/json";
    var contextFuture = context.Features.Get<IExceptionHandlerFeature>();

    if (contextFuture != null)
    {
      var loggerFactory = context.RequestServices.GetService<ILoggerFactory>();
      var logger = loggerFactory.CreateLogger("GlobalExceptionHandler");
      APIResponseDTO<object> response = null;

      if (contextFuture.Error is BaseBusinessException be)
      {
        context.Response.StatusCode = be.StatusCode ?? (int)HttpStatusCode.InternalServerError;
        response = new APIResponseDTO<object>
        {
          ErrorCode = be.StatusCode,
          Message = be.Message
        };
      }
      else
      {
        response = new APIResponseDTO<object>
        {
          ErrorCode = ErrorCodeEnum.Unexcepted,
          Message = contextFuture.Error.Message
        };
      }
      var errorLog = JsonSerializer.Serialize(response, new JsonSerializerOptions
      {
        WriteIndented = true,
      });

      //Technical Exception for troubleshooting
      logger.LogError("Error {0}", errorLog);

      //Business exception - exit gracefully
      await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
  });
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
