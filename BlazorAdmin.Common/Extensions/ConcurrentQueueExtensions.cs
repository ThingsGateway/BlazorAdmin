﻿// ------------------------------------------------------------------------------
// 此代码版权声明为全文件覆盖，如有原作者特别声明，会在下方手动补充
// 此代码版权（除特别声明外的代码）归作者本人Diego所有
// 源代码使用协议遵循本仓库的开源协议及附加协议
// Gitee源代码仓库：https://gitee.com/diego2098/BlazorAdmin
// Github源代码仓库：https://github.com/kimdiego2098/BlazorAdmin
// 使用文档：https://thingsgateway.cn/
// QQ群：605534569
// ------------------------------------------------------------------------------

using System.Collections.Concurrent;

namespace BlazorAdmin.Extension;

/// <inheritdoc/>
public static class ConcurrentQueueExtensions
{
    /// <summary>
    /// 批量出队
    /// </summary>
    public static List<T> ToListWithDequeue<T>(this ConcurrentQueue<T> values, int maxCount = 0)
    {
        if (maxCount <= 0)
        {
            maxCount = values.Count;
        }
        else
        {
            maxCount = Math.Min(maxCount, values.Count);
        }

        var list = new List<T>(maxCount);
        while (maxCount-- > 0 && values.TryDequeue(out var result))
        {
            list.Add(result);
        }
        return list;
    }
}
