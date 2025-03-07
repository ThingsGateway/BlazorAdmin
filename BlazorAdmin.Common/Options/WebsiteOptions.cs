﻿//------------------------------------------------------------------------------
//  此代码版权声明为全文件覆盖，如有原作者特别声明，会在下方手动补充
//  此代码版权（除特别声明外的代码）归作者本人Diego所有
//  源代码使用协议遵循本仓库的开源协议及附加协议
//  Gitee源代码仓库：https://gitee.com/diego2098/BlazorAdmin
//  Github源代码仓库：https://github.com/kimdiego2098/BlazorAdmin
//  使用文档：https://thingsgateway.cn/
//  QQ群：605534569
//------------------------------------------------------------------------------

using Furion.ConfigurableOptions;

namespace BlazorAdmin.Common;

/// <summary>
/// 网站配置
/// </summary>
public class WebsiteOptions : IConfigurableOptions
{
    /// <summary>
    /// Copyright
    /// </summary>
    public string Copyright { get; set; } = "版权所有 © 2023-present Diego";

    /// <summary>
    /// 是否Demo
    /// </summary>
    public bool Demo { get; set; }

    /// <summary>
    /// 是否显示关于页面
    /// </summary>
    public bool IsShowAbout { get; set; } = true;

    /// <summary>
    /// QQ群链接地址
    /// </summary>
    public string? QQGroup1Link { get; set; } = "http://qm.qq.com/cgi-bin/qm/qr?_wv=1027&k=NnBjPO-8kcNFzo_RzSbdICflb97u2O1i&authKey=V1MI3iJtpDMHc08myszP262kDykbx2Yev6ebE4Me0elTe0P0IFAmtU5l7Sy5w0jx&noverify=0&group_code=605534569";

    /// <summary>
    /// QQ群链接地址
    /// </summary>
    public string? QQGroup1Number { get; set; } = "605534569";

    /// <summary>
    /// 开源地址
    /// </summary>
    public string SourceUrl { get; set; } = "https://gitee.com/diego2098/BlazorAdmin";

    /// <summary>
    /// 标题
    /// </summary>
    public string? Title { get; set; } = "BlazorAdmin";

    /// <summary>
    /// 文档地址
    /// </summary>
    public string WikiUrl { get; set; } = "https://thingsgateway.cn/";


    /// <summary>
    /// 显示授权
    /// </summary>
    public bool ShowAuthorize { get; set; } = true;

}
