using LibraryManagement.Application.Common;
using LibraryManagement.Infrastructure.Data;
using LibraryManagement.Infrastructure.Data.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

        services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();

        return services;
    }
}
