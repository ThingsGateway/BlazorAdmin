﻿//------------------------------------------------------------------------------
//  此代码版权声明为全文件覆盖，如有原作者特别声明，会在下方手动补充
//  此代码版权（除特别声明外的代码）归作者本人Diego所有
//  源代码使用协议遵循本仓库的开源协议及附加协议
//  Gitee源代码仓库：https://gitee.com/diego2098/BlazorAdmin
//  Github源代码仓库：https://github.com/kimdiego2098/BlazorAdmin
//  使用文档：https://thingsgateway.cn/
//  QQ群：605534569
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace BlazorAdmin;

/// <summary>
/// 最小值校验
/// </summary>
public sealed class MinValueAttribute : ValidationAttribute
{
    /// <summary>
    /// 最小值
    /// </summary>
    /// <param name="value"></param>
    public MinValueAttribute(UInt64 value)
    {
        MinValue = value;
    }

    private UInt64 MinValue { get; set; }

    /// <summary>
    /// 最小值校验
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return false;
        }

        var input = Convert.ToUInt64(value);
        return input >= MinValue;
    }
}
