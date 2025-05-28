using Microsoft.Extensions.DependencyInjection;

namespace DDD_CQRS.Application;

public static class IoC
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(IoC).Assembly;
        services.AddMediatR(x => 
            x.RegisterServicesFromAssembly(assembly));
        
        return services;
    }
}
