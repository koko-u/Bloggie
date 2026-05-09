using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Bloggie.Web.ServiceCollectionExtensions;

/// <summary>
/// Extension methods for configuring NpgsqlDataSource in the service collection.
/// </summary>
public static class NpgsqlDataSourceExtension
{
    /// <summary>
    /// Add NpgsqlDataSource to the service collection using configuration settings.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddNpgsqlDataSource(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var builder = new NpgsqlConnectionStringBuilder()
        {
            Host = configuration.GetValue<string>("DATABASE_HOST"),
            Port = configuration.GetValue<int>("DATABASE_PORT"),
            Database = configuration.GetValue<string>("DATABASE_NAME"),
            Username = configuration.GetValue<string>("DATABASE_USER"),
            Password = configuration.GetValue<string>("DATABASE_PASSWORD"),
            SslMode = configuration.GetValue<SslMode>("DATABASE_SSLMODE"),
        };
        services.AddNpgsqlDataSource(builder.ConnectionString);

        return services;
    }
}
