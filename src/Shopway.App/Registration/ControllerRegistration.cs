﻿using Shopway.Infrastructure.Resolvers;
using Newtonsoft.Json.Converters;
using ApiBehaviorOptions = Shopway.App.Options.ApiBehaviorOptions;
using static Newtonsoft.Json.ReferenceLoopHandling;
using static Newtonsoft.Json.Formatting;

namespace Microsoft.Extensions.DependencyInjection;

public static class ControllerRegistration
{
    public static IServiceCollection RegisterControllers(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddApplicationPart(Shopway.Presentation.AssemblyReference.Assembly)
            .ConfigureApiBehaviorOptions(options =>
                options.InvalidModelStateResponseFactory = ApiBehaviorOptions.InvalidModelStateResponse)
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new RequiredPropertiesCamelCaseContractResolver();
                options.SerializerSettings.Formatting = Indented;
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.ReferenceLoopHandling = Ignore;
            });

        return services;
    }
}
