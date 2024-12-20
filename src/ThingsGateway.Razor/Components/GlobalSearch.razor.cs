﻿//------------------------------------------------------------------------------
//  此代码版权声明为全文件覆盖，如有原作者特别声明，会在下方手动补充
//  此代码版权（除特别声明外的代码）归作者本人Diego所有
//  源代码使用协议遵循本仓库的开源协议及附加协议
//  Gitee源代码仓库：https://gitee.com/diego2098/ThingsGateway
//  Github源代码仓库：https://github.com/kimdiego2098/ThingsGateway
//  使用文档：https://thingsgateway.cn/
//  QQ群：605534569
//------------------------------------------------------------------------------

namespace ThingsGateway.Razor;

/// <inheritdoc/>
public partial class GlobalSearch
{
    /// <inheritdoc/>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public IEnumerable<MenuItem>? Menus { get; set; }

    [NotNull]
    private IEnumerable<string?>? ComponentItems => Menus?.Select(i => i.Text);

    [Inject]
    [NotNull]
    private IStringLocalizer<GlobalSearch>? Localizer { get; set; }

    [Inject]
    [NotNull]
    private NavigationManager? NavigationManager { get; set; }

    [NotNull]
    private string? SearchText { get; set; }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        SearchText = Localizer[nameof(SearchText)];
    }

    [Inject]
    IServiceProvider ServiceProvider { get; set; }

    private Task OnSearch(string searchText)
    {

        if (!string.IsNullOrEmpty(searchText))
        {
            var item = Menus?.FirstOrDefault(i => i.Text?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false);
            if (item != null && !string.IsNullOrEmpty(item.Url))
            {
                NavigationManager.NavigateTo(ServiceProvider, item.Url, item.Text, item.Icon);
            }
        }
        return Task.CompletedTask;
    }

    private Task OnSelectedItemChanged(string searchText) => OnSearch(searchText);
}
