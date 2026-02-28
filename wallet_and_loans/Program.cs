using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using wallet_and_loans_api;
using wallet_and_loans_components.Logics;

var builder = WebApplication.CreateBuilder(args);

APIPreparer.RegisterComponents(builder);

builder.Services.AddAutoMapper(typeof(Program));
// Session configuration
builder.Services.AddDistributedMemoryCache();

// Auth
var jwtConfig = builder.Configuration.GetSection("Jwt");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtConfig["Issuer"],
            ValidAudience = jwtConfig["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Key"]!))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    TestStatic.UserTest = new User(1, "Hung", "crazyhung060", "LLL");
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
