using Microsoft.AspNetCore.Hosting;
using wallet_and_loans_api.BLO;
using wallet_and_loans_api.IBLO;
using wallet_and_loans_api.IRepositories;
using wallet_and_loans_api.IServices;
using wallet_and_loans_api.Repositories;
using wallet_and_loans_api.Services;

namespace wallet_and_loans_api
{
    public class APIPreparer
    {
        public static void RegisterComponents(WebApplicationBuilder builder)
        {
            RegisterRepositories(builder);
            RegisterBLOs(builder);
            RegisterServices(builder);
            RegisterControllers(builder);
        }

        public static void RegisterControllers(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddSwaggerGen();
        }

        // ToDo: implement DB later
        //public static void RegisterDatabase(WebApplicationBuilder builder)
        //{
        //    builder.Services.AddDbContext<WalletAndLoansContext>(options =>
        //        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        //}

        public static void RegisterServices(WebApplicationBuilder builder)
        {
            // Register your services here
            // Example:
            // builder.Services.AddScoped<IService, Servuce>();
            // Add other services as needed

            builder.Services.AddScoped<IWalletService, WalletService>();
            builder.Services.AddScoped<IBillService, BillService>();

            //var serviceAssembly = typeof(Program).Assembly; // Use a known type instead of Startup

            //var serviceTypes = serviceAssembly.GetTypes()
            //    .Where(t => t.IsClass && !t.IsAbstract &&
            //                t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IService<>)));

            //foreach (var service in serviceTypes)
            //{
            //    var interfaces = service.GetInterfaces()
            //        .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IService<>));

            //    foreach (var iface in interfaces)
            //    {
            //        builder.Services.AddScoped(iface, service);
            //    }
            //}
        }

        public static void RegisterBLOs(WebApplicationBuilder builder)
        {
            // Register your business logic objects (BLOs) here
            // Example:
            // builder.Services.AddScoped<IBLO, BLO>();
            // Add other BLOs as needed

            builder.Services.AddScoped<IWalletBLO, WalletBLO>();
            builder.Services.AddScoped<IBillBLO, BillBLO>();
        }

        public static void RegisterRepositories(WebApplicationBuilder builder)
        {
            // Register your repositories here
            // Example:
            // builder.Services.AddScoped<IRepository, Repository>();
            // Add other repositories as needed

            builder.Services.AddScoped<IWalletRepository, WalletRepository>();
            builder.Services.AddScoped<IBillRepository, BillRepository>();
        }

        public static void RegisterAutoMapper(WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(typeof(Program));
        }
    }
}
