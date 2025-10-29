using AbpResearchWorker.Samples;
using Xunit;

namespace AbpResearchWorker.EntityFrameworkCore.Applications;

[Collection(AbpResearchWorkerTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<AbpResearchWorkerEntityFrameworkCoreTestModule>
{

}
