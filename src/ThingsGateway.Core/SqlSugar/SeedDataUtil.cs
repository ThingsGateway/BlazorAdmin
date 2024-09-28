//------------------------------------------------------------------------------
//  此代码版权声明为全文件覆盖，如有原作者特别声明，会在下方手动补充
//  此代码版权（除特别声明外的代码）归作者本人Diego所有
//  源代码使用协议遵循本仓库的开源协议及附加协议
//  Gitee源代码仓库：https://gitee.com/diego2098/ThingsGateway
//  Github源代码仓库：https://github.com/kimdiego2098/ThingsGateway
//  使用文档：https://thingsgateway.cn/
//  QQ群：605534569
//------------------------------------------------------------------------------

using Newtonsoft.Json;

using System.Reflection;
using System.Text.RegularExpressions;

namespace ThingsGateway;
/// <summary>
/// 种子数据工具类
/// </summary>
[ThingsGateway.DependencyInjection.SuppressSniffer]
public class SeedDataUtil
{
    /// <summary>
    /// 获取List列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="jsonName"></param>
    /// <returns></returns>
    public static List<T> GetSeedData<T>(string jsonName)
    {
        var basePath = AppContext.BaseDirectory;//获取项目目录
        jsonName = basePath.CombinePathWithOs(jsonName);//获取文件路径
        var dataString = FileUtil.ReadFile(jsonName);//读取文件
        return GetSeedDataByJson<T>(dataString);
    }

    public static string GetManifestResourceStream(Assembly assembly, string path)
    {
        var name = $"{assembly.GetName().Name}.{path}";
        using var readStream = assembly.GetManifestResourceStream(name);
        return readStream?.ToStr();
    }

    public static List<T> GetSeedDataByJson<T>(string json)
    {
        var seedData = new List<T>();//种子数据结果
        if (!string.IsNullOrEmpty(json))//如果有内容
        {
            //字段没有数据的替换成null
            json = Regex.Replace(json, "\\\"[^\"]+?\\\": \\\"\\\"", match => match.Value.Replace("\"\"", "null"));
            //将json字符串转为实体，这里extjson可以正常转换为字符串
            var seedDataRecord = (SeedDataRecords<T>)Newtonsoft.Json.JsonConvert.DeserializeObject(json, typeof(SeedDataRecords<T>), new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,// 使用缩进格式化输出
                NullValueHandling = NullValueHandling.Ignore, // 忽略空值属性
                Converters = new List<JsonConverter> { new ZeroAsFalseConverter() }
            });

            //种子数据赋值
            seedData = seedDataRecord.Records;
        }

        return seedData;
    }
}

/// <summary>
/// 种子数据格式实体类,遵循Navicat导出json格式
/// </summary>
/// <typeparam name="T"></typeparam>
public class SeedDataRecords<T>
{
    /// <summary>
    /// 数据
    /// </summary>
    public List<T> Records { get; set; }
}

internal class ZeroAsFalseConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(bool);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var value = reader.Value?.ToString()?.ToBoolean();
        return value;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        writer.WriteValue(value?.ToString());
    }
}
