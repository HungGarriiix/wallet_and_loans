using wallet_and_loans_api;
using wallet_and_loans_components.Logics;

var builder = WebApplication.CreateBuilder(args);

APIPreparer.RegisterComponents(builder);

builder.Services.AddAutoMapper(typeof(Program));
// Session configuration
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".Yuuka-chan.Session";
    options.IdleTimeout = new TimeSpan(0, 20, 0);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
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
app.UseSession();
app.MapControllers();

app.Run();
