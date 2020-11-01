using Microsoft.Extensions.DependencyInjection;
using ZipCo.Data.Entities;
using ZipCo.Data.Repositories;
using ZipCo.Data.Repositories.Interfaces;

namespace ZipCo.Data
{
    public static class RepositoriesDependencies
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<ZipCoContext>();
            services.AddScoped<IRepository<Account>, AccountRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}