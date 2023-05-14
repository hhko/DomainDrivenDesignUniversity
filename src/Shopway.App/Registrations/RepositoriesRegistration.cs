﻿using Shopway.Domain.Abstractions.Repositories;
using Shopway.Persistence.Outbox;
using Shopway.Persistence.Repositories;

namespace Microsoft.Extensions.DependencyInjection;

public static class RepositoriesRegistration
{
    public static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<IOrderHeaderRepository, OrderHeaderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOutboxRepository, OutboxRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();

        return services;
    }
}
