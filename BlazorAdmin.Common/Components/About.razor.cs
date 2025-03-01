﻿//------------------------------------------------------------------------------
//  此代码版权声明为全文件覆盖，如有原作者特别声明，会在下方手动补充
//  此代码版权（除特别声明外的代码）归作者本人Diego所有
//  源代码使用协议遵循本仓库的开源协议及附加协议
//  Gitee源代码仓库：https://gitee.com/diego2098/BlazorAdmin
//  Github源代码仓库：https://github.com/kimdiego2098/BlazorAdmin
//  使用文档：https://thingsgateway.cn/
//  QQ群：605534569
//------------------------------------------------------------------------------

namespace BlazorAdmin.Common;

/// <inheritdoc/>
public partial class About
{
    [Inject]
    [NotNull]
    private IStringLocalizer<About>? Localizer { get; set; }

    [Inject]
    [NotNull]
    private IOptions<WebsiteOptions>? WebsiteOption { get; set; }
    [Inject]
    [NotNull]
    private IRegisterService? RegisterService { get; set; }
    private string Password { get; set; }
    [Inject]
    ToastService ToastService { get; set; }
    private async Task Register()
    {
        var result = RegisterService.Register(Password);
        if (result)
            await ToastService.Default();
        else
            await ToastService.Default(false);
        await InvokeAsync(StateHasChanged);
    }
}
