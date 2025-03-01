//------------------------------------------------------------------------------
//  此代码版权声明为全文件覆盖，如有原作者特别声明，会在下方手动补充
//  此代码版权（除特别声明外的代码）归作者本人Diego所有
//  源代码使用协议遵循本仓库的开源协议及附加协议
//  Gitee源代码仓库：https://gitee.com/diego2098/BlazorAdmin
//  Github源代码仓库：https://github.com/kimdiego2098/BlazorAdmin
//  使用文档：https://thingsgateway.cn/
//  QQ群：605534569
//------------------------------------------------------------------------------

using Furion;

using BlazorAdmin.Application;

namespace BlazorAdmin.Razor;

public class ComponentDefault : ComponentBase
{
    [Inject]
    [NotNull]
    public IStringLocalizer<BlazorAdmin.Common._Imports>? RazorLocalizer { get; set; }

    [Inject]
    [NotNull]
    public IStringLocalizer<BlazorAdmin.Razor._Imports>? AdminLocalizer { get; set; }

    [Inject]
    [NotNull]
    public DialogService? DialogService { get; set; }

    [NotNull]
    public IStringLocalizer? Localizer { get; set; }

    [Inject]
    [NotNull]
    public IStringLocalizer<OperDescAttribute>? OperDescLocalizer { get; set; }

    [Inject]
    [NotNull]
    public ToastService? ToastService { get; set; }

    [Inject]
    [NotNull]
    protected BlazorAppContext? AppContext { get; set; }

    [Inject]
    [NotNull]
    private NavigationManager? NavigationManager { get; set; }

    public string RouteName => NavigationManager.ToBaseRelativePath(NavigationManager.Uri);

    protected bool AuthorizeButton(string operate)
    {
        return AppContext.IsHasButtonWithRole(RouteName, operate);
    }

    protected override void OnInitialized()
    {
        Localizer = App.GetService<IStringLocalizerFactory>().Create(GetType());
        base.OnInitialized();
    }
}
