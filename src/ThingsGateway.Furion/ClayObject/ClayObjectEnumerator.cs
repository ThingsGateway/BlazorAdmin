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

using System.Xml.Linq;

namespace ThingsGateway.ClayObject;

/// <summary>
/// 粘土对象对象类型枚举器实现类
/// </summary>
[SuppressSniffer]
public sealed class ClayObjectEnumerator : IEnumerator
{
    /// <summary>
    /// 粘土对象
    /// </summary>
    public dynamic _clay;

    /// <summary>
    /// 当前索引
    /// </summary>
    private int position = -1;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="clay">粘土对象</param>
    public ClayObjectEnumerator(dynamic clay)
    {
        _clay = clay;
    }

    /// <summary>
    /// 推进（获取）下一个元素
    /// </summary>
    /// <returns></returns>
    public bool MoveNext()
    {
        position++;
        return (position < _clay.Length);
    }

    /// <summary>
    /// 将元素索引恢复初始值
    /// </summary>
    public void Reset()
    {
        position = -1;
    }

    /// <summary>
    /// 当前元素
    /// </summary>
    public KeyValuePair<string, dynamic> Current
    {
        get
        {
            try
            {
                var xElement = ((XElement)_clay.XmlElement).Elements().ElementAtOrDefault(position);

                // 获取节点真实标签名
                var localName = xElement.Name == "{item}item"
                    ? xElement.Attribute("item").Value
                    : xElement.Name.LocalName;

                return new(localName, _clay[localName]);
            }
            catch (IndexOutOfRangeException)
            {
                throw new InvalidOperationException();
            }
        }
    }

    /// <summary>
    /// 当前元素（内部）
    /// </summary>
    object IEnumerator.Current => Current;
}