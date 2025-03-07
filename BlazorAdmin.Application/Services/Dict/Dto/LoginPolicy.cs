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

public class LoginPolicy
{
    /// <summary>
    /// 登录错误次数锁定阈值
    /// </summary>
    [MinValue(3)]
    public int ErrorCount { get; set; }

    /// <summary>
    /// 登录错误锁定时间(分)
    /// </summary>
    [MinValue(1)]
    public int ErrorLockTime { get; set; }

    /// <summary>
    ///  登录错误次数过期时间(分)
    /// </summary>
    [MinValue(1)]
    public int ErrorResetTime { get; set; }

    /// <summary>
    /// 单用户登录开关
    /// </summary>
    public bool SingleOpen { get; set; }

    /// <summary>
    /// 登录过期时间(分)
    /// </summary>
    [MinValue(1)]
    public int VerificatExpireTime { get; set; }
}
