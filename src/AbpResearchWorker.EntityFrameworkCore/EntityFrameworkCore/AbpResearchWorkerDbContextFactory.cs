using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AbpResearchWorker.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class AbpResearchWorkerDbContextFactory : IDesignTimeDbContextFactory<AbpResearchWorkerDbContext>
{
    public AbpResearchWorkerDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        
        AbpResearchWorkerEfCoreEntityExtensionMappings.Configure();

        var builder = new DbContextOptionsBuilder<AbpResearchWorkerDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));
        
        return new AbpResearchWorkerDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../AbpResearchWorker.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
