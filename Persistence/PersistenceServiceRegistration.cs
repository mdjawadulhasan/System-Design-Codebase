using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicketManagement.Application.Contracts.Persistence;
using TicketManagement.Persistence.Repositories;

namespace  Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SystemDesignDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("SystemDesignConnectionString")));

        services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));
       // services.AddScoped<IOrderRepository, OrderRepository>();

        return services;    
    }
}