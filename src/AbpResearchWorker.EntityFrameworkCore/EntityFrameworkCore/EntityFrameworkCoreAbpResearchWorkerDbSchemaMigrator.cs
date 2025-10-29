using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AbpResearchWorker.Data;
using Volo.Abp.DependencyInjection;

namespace AbpResearchWorker.EntityFrameworkCore;

public class EntityFrameworkCoreAbpResearchWorkerDbSchemaMigrator
    : IAbpResearchWorkerDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreAbpResearchWorkerDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the AbpResearchWorkerDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<AbpResearchWorkerDbContext>()
            .Database
            .MigrateAsync();
    }
}
