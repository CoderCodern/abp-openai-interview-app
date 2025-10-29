using AbpResearchWorker.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace AbpResearchWorker.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpResearchWorkerEntityFrameworkCoreModule),
    typeof(AbpResearchWorkerApplicationContractsModule)
)]
public class AbpResearchWorkerDbMigratorModule : AbpModule
{
}
