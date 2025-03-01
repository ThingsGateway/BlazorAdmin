﻿//------------------------------------------------------------------------------
//  此代码版权声明为全文件覆盖，如有原作者特别声明，会在下方手动补充
//  此代码版权（除特别声明外的代码）归作者本人Diego所有
//  源代码使用协议遵循本仓库的开源协议及附加协议
//  Gitee源代码仓库：https://gitee.com/diego2098/BlazorAdmin
//  Github源代码仓库：https://github.com/kimdiego2098/BlazorAdmin
//  使用文档：https://thingsgateway.cn/
//  QQ群：605534569
//------------------------------------------------------------------------------

namespace BlazorAdmin.Application;

/// <summary>
/// 授权用户常量
/// </summary>
[Furion.DependencyInjection.SuppressSniffer]
public class ClaimConst
{
    /// <summary>
    /// 账号
    /// </summary>
    public const string Account = "Account";

    /// <summary>
    /// SuperAdmin
    /// </summary>
    public const string SuperAdmin = "SuperAdmin";

    /// <summary>
    /// 用户Id
    /// </summary>
    public const string UserId = "UserId";

    /// <summary>
    /// 验证Id
    /// </summary>
    public const string VerificatId = "VerificatId";

    /// <summary>
    /// 组织Id
    /// </summary>
    public const string OrgId = "OrgId";

    /// <summary>
    /// 租户Id
    /// </summary>
    public const string TenantId = "TenantId";


}
