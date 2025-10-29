using Volo.Abp.Settings;

namespace AbpResearchWorker.Settings;

public class AbpResearchWorkerSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(AbpResearchWorkerSettings.MySetting1));
    }
}
