﻿//------------------------------------------------------------------------------
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

public partial class SysDictPage
{
    private SysDict? SearchModel { get; set; } = new();

    [Inject]
    [NotNull]
    private ISysDictService? SysDictService { get; set; }

    #region 查询


    private async Task<QueryData<SysDict>> OnQueryAsync(QueryPageOptions options)
    {
        var data = await SysDictService.PageAsync(options);
        return data;
    }

    #endregion 查询

    #region 修改

    private async Task<bool> DeleteDictAsync(IEnumerable<long> ids)
    {
        try
        {
            return await SysDictService.DeleteDictAsync(ids);
        }
        catch (Exception ex)
        {
            await ToastService.Warn(ex);
            return false;
        }
    }

    private async Task<bool> SaveDictAsync(SysDict input, ItemChangedType type)
    {
        try
        {
            return await SysDictService.SaveDictAsync(input, type);
        }
        catch (Exception ex)
        {
            await ToastService.Warn(ex);
            return false;
        }
    }

    #endregion 修改
}
