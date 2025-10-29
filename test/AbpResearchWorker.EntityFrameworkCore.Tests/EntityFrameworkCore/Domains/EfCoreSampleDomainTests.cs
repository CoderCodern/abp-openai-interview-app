using AbpResearchWorker.Samples;
using Xunit;

namespace AbpResearchWorker.EntityFrameworkCore.Domains;

[Collection(AbpResearchWorkerTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<AbpResearchWorkerEntityFrameworkCoreTestModule>
{

}
