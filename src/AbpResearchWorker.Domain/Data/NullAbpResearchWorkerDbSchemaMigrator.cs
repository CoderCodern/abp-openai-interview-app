using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace AbpResearchWorker.Data;

/* This is used if database provider does't define
 * IAbpResearchWorkerDbSchemaMigrator implementation.
 */
public class NullAbpResearchWorkerDbSchemaMigrator : IAbpResearchWorkerDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
