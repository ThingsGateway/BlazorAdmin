﻿//------------------------------------------------------------------------------
//  此代码版权声明为全文件覆盖，如有原作者特别声明，会在下方手动补充
//  此代码版权（除特别声明外的代码）归作者本人Diego所有
//  源代码使用协议遵循本仓库的开源协议及附加协议
//  Gitee源代码仓库：https://gitee.com/diego2098/BlazorAdmin
//  Github源代码仓库：https://github.com/kimdiego2098/BlazorAdmin
//  使用文档：https://thingsgateway.cn/
//  QQ群：605534569
//------------------------------------------------------------------------------

using BootstrapBlazor.Components;

using NewLife;

using SqlSugar;

using System.Data;

namespace BlazorAdmin.Application;

internal sealed class SysOperateLogService : BaseService<SysOperateLog>, ISysOperateLogService
{

    #region 查询

    /// <summary>
    /// 最新十条
    /// </summary>
    /// <param name="account">操作人</param>
    public async Task<List<OperateLogIndexOutput>> GetNewLog(string account)
    {
        using var db = GetDB();
        var data = await db.Queryable<SysOperateLog>().Select(a => new OperateLogIndexOutput { OpTime = a.OpTime, Name = a.Name, OpAccount = a.OpAccount, OpBrowser = a.OpBrowser, OpIp = a.OpIp }).Where(a => a.OpAccount == account).OrderByDescending(a => a.OpTime).Take(10).ToListAsync().ConfigureAwait(false);
        return data;
    }

    /// <summary>
    /// 表格查询
    /// </summary>
    /// <param name="option">查询条件</param>
    public Task<QueryData<SysOperateLog>> PageAsync(QueryPageOptions option)
    {
        return QueryAsync(option, query => query.WhereIF(!option.SearchText.IsNullOrWhiteSpace(), a => a.Name.Contains(option.SearchText)));
    }

    /// <summary>
    /// 获取n天的统计信息
    /// </summary>
    /// <param name="day">天</param>
    /// <returns>统计信息</returns>
    public async Task<List<OperateLogDayStatisticsOutput>> StatisticsByDayAsync(int day)
    {
        using var db = GetDB();
        //取最近七天
        var dayArray = Enumerable.Range(0, day).Select(it => DateTime.Now.Date.AddDays(it * -1)).ToList();
        //生成时间表
        var queryableLeft = db.Reportable(dayArray).ToQueryable<DateTime>();
        //ReportableDateType.MonthsInLast1yea 表式近一年月份 并且queryable之后还能在where过滤
        var queryableRight = db.Queryable<SysOperateLog>(); //声名表
        //报表查询
        var list = await db.Queryable(queryableLeft, queryableRight, JoinType.Left, (x1, x2)
            => x2.OpTime.ToString("yyyy-MM-dd") == x1.ColumnName.ToString("yyyy-MM-dd"))
        .GroupBy((x1, x2) => x1.ColumnName)//根据时间分组
        .OrderBy((x1, x2) => x1.ColumnName)//根据时间升序排序
        .Select((x1, x2) => new OperateLogDayStatisticsOutput
        {
            OperateCount = SqlFunc.AggregateSum(SqlFunc.IIF(x2.Category == LogCateGoryEnum.Operate, 1, 0)), //null的数据要为0所以不能用count
            ExceptionCount = SqlFunc.AggregateSum(SqlFunc.IIF(x2.Category == LogCateGoryEnum.Exception, 1, 0)), //null的数据要为0所以不能用count
            LoginCount = SqlFunc.AggregateSum(SqlFunc.IIF(x2.Category == LogCateGoryEnum.Login, 1, 0)), //null的数据要为0所以不能用count
            LogoutCount = SqlFunc.AggregateSum(SqlFunc.IIF(x2.Category == LogCateGoryEnum.Logout, 1, 0)), //null的数据要为0所以不能用count
            Date = x1.ColumnName.ToString("yyyy-MM-dd")
        }
        ).ToListAsync().ConfigureAwait(false);

        return list;
    }

    #endregion 查询

    #region 删除

    /// <summary>
    /// 删除日志
    /// </summary>
    /// <param name="category">分类</param>
    [OperDesc("DeleteOperLog", isRecordPar: false)]
    public async Task DeleteAsync(LogCateGoryEnum category)
    {
        using var db = GetDB();
        await db.Deleteable<SysOperateLog>(it => it.Category == category).ExecuteCommandAsync().ConfigureAwait(false);
    }

    #endregion 删除



}
