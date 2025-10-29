using AbpResearchWorker.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace AbpResearchWorker.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class AbpResearchWorkerController : AbpControllerBase
{
    protected AbpResearchWorkerController()
    {
        LocalizationResource = typeof(AbpResearchWorkerResource);
    }
}
