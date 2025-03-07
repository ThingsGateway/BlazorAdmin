﻿//------------------------------------------------------------------------------
//  此代码版权声明为全文件覆盖，如有原作者特别声明，会在下方手动补充
//  此代码版权（除特别声明外的代码）归作者本人Diego所有
//  源代码使用协议遵循本仓库的开源协议及附加协议
//  Gitee源代码仓库：https://gitee.com/diego2098/BlazorAdmin
//  Github源代码仓库：https://github.com/kimdiego2098/BlazorAdmin
//  使用文档：https://thingsgateway.cn/
//  QQ群：605534569
//------------------------------------------------------------------------------

using Furion;

using Microsoft.Extensions.Hosting;

namespace BlazorAdmin.Common;

/// <inheritdoc/>
[Furion.DependencyInjection.SuppressSniffer]
public static class ToastServiceExtensions
{
    /// <inheritdoc/>
    public static Task Default(this ToastService toastService, bool success = true)
    {
        if (success)
            return toastService.Success("Success");
        else
            return toastService.Warning("Fail");
    }

    /// <inheritdoc/>
    public static Task Warn(this ToastService toastService, Exception ex)
    {
        if (App.HostEnvironment.IsDevelopment())
            return toastService.Warning("Warn", ex.ToString());
        else
            return toastService.Warning("Warn", ex.Message);
    }
}
