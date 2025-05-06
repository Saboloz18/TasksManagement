﻿using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TasksManagement.Application.Services.JwtService;

namespace TasksManagement.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()); });
            services.AddScoped<IJwtTokenService,  JwtTokenService>();   
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }
    }

}