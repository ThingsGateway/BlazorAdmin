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

public partial class VerificatListDialog
{
    [Parameter]
    public long UserId { get; set; }

    [Parameter]
    public List<VerificatInfo> VerificatInfos { get; set; }

    private VerificatInfo? SearchModel { get; set; } = new();

    [Inject]
    [NotNull]
    private ISessionService? SessionService { get; set; }

    #region 查询

    private Task<QueryData<VerificatInfo>> OnQueryAsync(QueryPageOptions options)
    {
        var data = VerificatInfos.GetQueryData(options);
        return Task.FromResult(data);
    }

    #endregion 查询
}
