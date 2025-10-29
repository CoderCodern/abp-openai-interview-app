using AbpResearchWorker.Localization;
using Volo.Abp.Application.Services;

namespace AbpResearchWorker;

/* Inherit your application services from this class.
 */
public abstract class AbpResearchWorkerAppService : ApplicationService
{
    protected AbpResearchWorkerAppService()
    {
        LocalizationResource = typeof(AbpResearchWorkerResource);
    }
}
