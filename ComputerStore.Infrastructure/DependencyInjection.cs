using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ComputerStore.Application;
using ComputerStore.Infrastructure.Data;
using Infrastructure.Persistence;
using Application.Repositories;

namespace ComputerStore.Infrastructure

{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var defaultConnectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ComputerStoredbContext>(options =>
               options.UseSqlServer(defaultConnectionString));

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();


            return services;
        }
    }
}