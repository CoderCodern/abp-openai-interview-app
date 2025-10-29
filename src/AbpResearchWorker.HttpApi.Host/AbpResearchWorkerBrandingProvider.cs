using Microsoft.Extensions.Localization;
using AbpResearchWorker.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace AbpResearchWorker;

[Dependency(ReplaceServices = true)]
public class AbpResearchWorkerBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<AbpResearchWorkerResource> _localizer;

    public AbpResearchWorkerBrandingProvider(IStringLocalizer<AbpResearchWorkerResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
