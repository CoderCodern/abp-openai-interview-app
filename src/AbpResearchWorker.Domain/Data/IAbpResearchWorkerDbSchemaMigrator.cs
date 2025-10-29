using System.Threading.Tasks;

namespace AbpResearchWorker.Data;

public interface IAbpResearchWorkerDbSchemaMigrator
{
    Task MigrateAsync();
}
