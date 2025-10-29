using Xunit;

namespace AbpResearchWorker.EntityFrameworkCore;

[CollectionDefinition(AbpResearchWorkerTestConsts.CollectionDefinitionName)]
public class AbpResearchWorkerEntityFrameworkCoreCollection : ICollectionFixture<AbpResearchWorkerEntityFrameworkCoreFixture>
{

}
