﻿using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace ThingsGateway.NewLife.Data;

/// <summary>数据行</summary>
/// <remarks>
/// 文档 https://newlifex.com/core/dbtable
/// </remarks>
/// <remarks>构造数据行</remarks>
/// <param name="table"></param>
/// <param name="index"></param>
public readonly struct DbRow(DbTable table, Int32 index) : IModel
{
    #region 属性
    /// <summary>数据表</summary>
    [XmlIgnore, IgnoreDataMember]
    public readonly DbTable Table { get; } = table;

    /// <summary>行索引</summary>
    public readonly Int32 Index { get; } = index;

    #endregion

    #region 索引器
    /// <summary>基于列索引访问</summary>
    /// <param name="column"></param>
    /// <returns></returns>
    public readonly Object? this[Int32 column]
    {
        get => Table.Rows?[Index][column];
        set
        {
            var rows = Table.Rows;
            if (rows != null) rows[Index][column] = value;
        }
    }

    /// <summary>基于列名访问</summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Object? this[String name] { get => this[Table.GetColumn(name)]; set => this[Table.GetColumn(name)] = value; }
    #endregion

    #region 高级扩展
    /// <summary>读取指定行的字段值</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public readonly T? Get<T>(String name) => Table.Get<T>(Index, name);
    #endregion
}