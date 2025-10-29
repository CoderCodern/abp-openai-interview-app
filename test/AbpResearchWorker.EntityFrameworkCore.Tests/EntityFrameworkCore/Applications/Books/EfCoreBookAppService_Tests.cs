using AbpResearchWorker.Books;
using Xunit;

namespace AbpResearchWorker.EntityFrameworkCore.Applications.Books;

[Collection(AbpResearchWorkerTestConsts.CollectionDefinitionName)]
public class EfCoreBookAppService_Tests : BookAppService_Tests<AbpResearchWorkerEntityFrameworkCoreTestModule>
{

}