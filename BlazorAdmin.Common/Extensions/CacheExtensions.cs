//------------------------------------------------------------------------------
//  此代码版权声明为全文件覆盖，如有原作者特别声明，会在下方手动补充
//  此代码版权（除特别声明外的代码）归作者本人Diego所有
//  源代码使用协议遵循本仓库的开源协议及附加协议
//  Gitee源代码仓库：https://gitee.com/diego2098/BlazorAdmin
//  Github源代码仓库：https://github.com/kimdiego2098/BlazorAdmin
//  使用文档：https://thingsgateway.cn/
//  QQ群：605534569
//------------------------------------------------------------------------------

using NewLife.Caching;

namespace BlazorAdmin.Json.Extension;

public static class CacheExtensions
{
    #region 集合
    /// <inheritdoc/>
    public static void HashAdd<T>(this ICache cache, string key, string hashKey, T value)
    {
        lock (cache)
        {
            //获取字典
            var exist = cache.GetDictionary<T>(key);
            if (exist.ContainsKey(hashKey))//如果包含Key
                exist[hashKey] = value;//重新赋值
            else exist.TryAdd(hashKey, value);//加上新的值
            cache.Set(key, exist);
        }
    }

    /// <inheritdoc/>
    public static bool HashSet<T>(this ICache cache, string key, Dictionary<string, T> dic)
    {
        lock (cache)
        {
            //获取字典
            var exist = cache.GetDictionary<T>(key);
            foreach (var it in dic)
            {
                if (exist.ContainsKey(it.Key))//如果包含Key
                    exist[it.Key] = it.Value;//重新赋值
                else exist.Add(it.Key, it.Value);//加上新的值
            }
            return true;
        }
    }

    /// <inheritdoc/>
    public static int HashDel<T>(this ICache cache, string key, params string[] fields)
    {
        var result = 0;
        //获取字典
        var exist = cache.GetDictionary<T>(key);
        foreach (var field in fields)
        {
            if (field != null && exist.ContainsKey(field))//如果包含Key
            {
                exist.Remove(field);//删除
                result++;
            }
        }
        return result;
    }

    /// <inheritdoc/>
    public static List<T> HashGet<T>(this ICache cache, string key, params string[] fields)
    {
        var list = new List<T>();
        //获取字典
        var exist = cache.GetDictionary<T>(key);
        foreach (var field in fields)
        {
            if (exist.TryGetValue(field, out var data))//如果包含Key
            {
                list.Add(data);
            }
            else { list.Add(default); }
        }
        return list;
    }

    /// <inheritdoc/>
    public static T HashGetOne<T>(this ICache cache, string key, string field)
    {
        //获取字典
        var exist = cache.GetDictionary<T>(key);
        exist.TryGetValue(field, out var result);
        return result;
    }

    /// <inheritdoc/>
    public static IDictionary<string, T> HashGetAll<T>(this ICache cache, string key)
    {
        var data = cache.GetDictionary<T>(key);
        return data;
    }
    /// <inheritdoc/>
    public static void DelByPattern(this ICache cache, string pattern)
    {
        var keys = cache.Keys;//获取所有key
        foreach (var item in keys.ToList())
        {
            if (item.StartsWith(pattern))//如果匹配
                cache.Remove(item);
        }
    }
    #endregion

}
