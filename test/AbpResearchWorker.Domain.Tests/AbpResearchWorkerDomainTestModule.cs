using Volo.Abp.Modularity;

namespace AbpResearchWorker;

[DependsOn(
    typeof(AbpResearchWorkerDomainModule),
    typeof(AbpResearchWorkerTestBaseModule)
)]
public class AbpResearchWorkerDomainTestModule : AbpModule
{

}
