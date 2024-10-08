﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Shopway.Domain.Users.Authorization;
using Shopway.Presentation.Authentication;
using Shopway.Presentation.Authentication.ApiKeyAuthentication;
using Shopway.Presentation.Authentication.ApiKeyAuthentication.Handlers;
using Shopway.Presentation.Authentication.GenericProxy;
using Shopway.Presentation.Authentication.OrderHeaders.OrderHeaderCreatedByUser;
using Shopway.Presentation.Authentication.RolePermissionAuthentication;
using Shopway.Presentation.Authentication.RolePermissionAuthentication.Handlers;
using Shopway.Presentation.Authentication.Services;

namespace Microsoft.Extensions.DependencyInjection;

internal static class SecurityRegistration
{
    internal static IServiceCollection RegisterSecurity(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddScheme<AuthenticationSchemeOptions, AnonymousSchema>(AnonymousSchema.Name, null)
            .AddJwtBearer()
            .AddCookie()
            .AddGoogle(options =>
            {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });

        services.AddAuthorization(options =>
        {
            //Fallback policy, so if there is no policy there, the user must be authenticated (commented due to the tutorial purposes)
            //options.FallbackPolicy = new AuthorizationPolicyBuilder()
            //    .RequireAuthenticatedUser()
            //    .Build();

            options.AddPolicy(OrderHeaderCreatedByUserRequirement.PolicyName, policy => policy.Requirements.Add(new OrderHeaderCreatedByUserRequirement()));
            options.AddPolicy(GenericProxyPropertiesRequirement.PolicyName, policy => policy.Requirements.Add(new GenericProxyPropertiesRequirement()));
        });

        services.AddScoped<IAuthorizationHandler, OrderHeaderCreatedByUserRequirementHandler>();
        services.AddScoped<IAuthorizationHandler, GenericProxyPropertiesRequirementHandler>();
        services.AddScoped<IAuthorizationHandler, PermissionRequirementHandler<PermissionName, RoleName>>();
        services.AddScoped<IAuthorizationHandler, RoleRequirementHandler<PermissionName, RoleName>>();
        services.AddScoped<IAuthorizationHandler, ApiKeyRequirementHandler<ApiKeyName>>();
        services.AddScoped<IUserAuthorizationService<PermissionName, RoleName>, UserAuthorizationService<PermissionName, RoleName>>();

        return services;
    }
}
