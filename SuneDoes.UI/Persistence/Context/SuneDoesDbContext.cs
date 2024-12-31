using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql.NameTranslation;
using SuneDoes.UI.Configuration;
using SuneDoes.UI.Persistence.Model;

namespace SuneDoes.UI.Persistence.Context;

public class SuneDoesDbContext : DbContext
{

    public DbSet<VerifiableEmailDbo> EmailAddresses { get; set; }

    public SuneDoesDbContext(DbContextOptions<SuneDoesDbContext> opts) : base(opts)
    {


    }


    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.Properties<DateTime?>().HaveColumnType("timestamp without time zone");
        configurationBuilder.Properties<DateTime>().HaveColumnType("timestamp without time zone");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SuneDoesDbContext).Assembly);
    }



}


public static class SuneDoesDbContextExtensions
{
    public static WebApplicationBuilder AddSuneDoesDbContext(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContextFactory<SuneDoesDbContext>(ConfigureDb, lifetime: ServiceLifetime.Singleton);

        return builder;
    }

    private static void ConfigureDb(IServiceProvider services, DbContextOptionsBuilder builder)
    {
        var connectionString = services.GetRequiredService<IOptions<SuneDoesConfiguration>>().Value.Db.ConnectionString;
        builder
            .UseNpgsql(connectionString, opts =>
            {
                //opts.MapEnum<MalarkeyIdentityProviderDbo>("provider_type", nameTranslator: new NpgsqlNullNameTranslator());
                //opts.MapEnum<MalarkeyTokenTypeDbo>("token_type", nameTranslator: new NpgsqlNullNameTranslator());

            })
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging();
    }



}
