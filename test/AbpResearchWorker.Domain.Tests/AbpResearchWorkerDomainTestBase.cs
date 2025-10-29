using Volo.Abp.Modularity;

namespace AbpResearchWorker;

/* Inherit from this class for your domain layer tests. */
public abstract class AbpResearchWorkerDomainTestBase<TStartupModule> : AbpResearchWorkerTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
