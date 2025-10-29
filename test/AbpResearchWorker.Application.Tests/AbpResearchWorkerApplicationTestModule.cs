using Volo.Abp.Modularity;

namespace AbpResearchWorker;

[DependsOn(
    typeof(AbpResearchWorkerApplicationModule),
    typeof(AbpResearchWorkerDomainTestModule)
)]
public class AbpResearchWorkerApplicationTestModule : AbpModule
{

}
