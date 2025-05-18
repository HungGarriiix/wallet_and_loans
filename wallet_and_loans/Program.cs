using wallet_and_loans_api;
using wallet_and_loans_components.Logics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
APIPreparer.RegisterServices(builder);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddSwaggerGen();

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
