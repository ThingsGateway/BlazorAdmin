//------------------------------------------------------------------------------
//  此代码版权声明为全文件覆盖，如有原作者特别声明，会在下方手动补充
//  此代码版权（除特别声明外的代码）归作者本人Diego所有
//  源代码使用协议遵循本仓库的开源协议及附加协议
//  Gitee源代码仓库：https://gitee.com/diego2098/BlazorAdmin
//  Github源代码仓库：https://github.com/kimdiego2098/BlazorAdmin
//  使用文档：https://thingsgateway.cn/
//  QQ群：605534569
//------------------------------------------------------------------------------

using BlazorAdmin.Application;

namespace BlazorAdmin.Razor;

public partial class SysUserEdit
{

    private List<SelectedItem> ModuleSelectedItems { get; set; }
    [Inject]
    private IStringLocalizer<BlazorAdmin.Razor._Imports>? AdminLocalizer { get; set; }
    [Inject]
    private ISysPositionService? SysPositionService { get; set; }


    [Parameter]
    [NotNull]
    public SysUser? Model { get; set; }

    private List<CascaderItem> Items { get; set; }
    private List<SelectedItem> BoolItems;
    [Inject]
    private ISysResourceService SysResourceService { get; set; }
    protected override async Task OnInitializedAsync()
    {
        BoolItems = LocalizerUtil.GetBoolItems(Model.GetType(), nameof(Model.Status));
        var items = await SysPositionService.SelectorAsync(new PositionSelectorInput() { });
        Items = PositionUtil.BuildCascaderItemList(items);
        ModuleSelectedItems = ResourceUtil.BuildModuleSelectList((await SysResourceService.GetAllAsync())).ToList();
        await base.OnInitializedAsync();
    }

    private Task OnSelectedItemChanged(CascaderItem[] items)
    {
        Model.OrgId = items.LastOrDefault()?.Parent?.Value?.ToLong() ?? 0;
        return Task.CompletedTask;
    }
}
