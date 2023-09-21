using System.Text;
using LearnOAuth.Context;
using LearnOAuth.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddControllers();
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
builder.Services.AddSingleton<UserOAuthServices>();
builder.Services.AddSingleton<JwtServices>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
