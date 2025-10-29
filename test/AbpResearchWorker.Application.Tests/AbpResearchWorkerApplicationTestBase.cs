using Volo.Abp.Modularity;

namespace AbpResearchWorker;

public abstract class AbpResearchWorkerApplicationTestBase<TStartupModule> : AbpResearchWorkerTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
