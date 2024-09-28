// ------------------------------------------------------------------------
// 版权信息
// 版权归百小僧及百签科技（广东）有限公司所有。
// 所有权利保留。
// 官方网站：https://baiqian.com
//
// 许可证信息
// 项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。
// 许可证的完整文本可以在源代码树根目录中的 LICENSE-APACHE 和 LICENSE-MIT 文件中找到。
// ------------------------------------------------------------------------

using System.Text.Json;
using System.Text.Json.Serialization;

using ThingsGateway.ClayObject;

namespace ThingsGateway.JsonSerialization;

/// <summary>
/// Clay 类型序列化
/// </summary>
[SuppressSniffer]
public class SystemTextJsonClayJsonConverter : JsonConverter<Clay>
{
    /// <summary>
    /// 默认构造函数
    /// </summary>
    public SystemTextJsonClayJsonConverter()
    {
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="typeToConvert"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public override Clay Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return Clay.Parse(reader.GetString());
    }

    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    public override void Write(Utf8JsonWriter writer, Clay value, JsonSerializerOptions options)
    {
        writer.WriteRawValue(value.ToString());
    }
}