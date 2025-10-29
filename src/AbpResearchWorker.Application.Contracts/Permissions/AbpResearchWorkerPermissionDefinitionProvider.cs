using AbpResearchWorker.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace AbpResearchWorker.Permissions;

public class AbpResearchWorkerPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(AbpResearchWorkerPermissions.GroupName);

        var booksPermission = myGroup.AddPermission(AbpResearchWorkerPermissions.Books.Default, L("Permission:Books"));
        booksPermission.AddChild(AbpResearchWorkerPermissions.Books.Create, L("Permission:Books.Create"));
        booksPermission.AddChild(AbpResearchWorkerPermissions.Books.Edit, L("Permission:Books.Edit"));
        booksPermission.AddChild(AbpResearchWorkerPermissions.Books.Delete, L("Permission:Books.Delete"));
        //Define your own permissions here. Example:
        //myGroup.AddPermission(AbpResearchWorkerPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AbpResearchWorkerResource>(name);
    }
}
