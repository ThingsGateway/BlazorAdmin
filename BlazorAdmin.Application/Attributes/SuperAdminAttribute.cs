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
/// 管理员才能访问
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class SuperAdminAttribute : Attribute
{
}

/// <summary>
/// 忽略超级管理员才能访问特性
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class IgnoreSuperAdminAttribute : Attribute
{
}
