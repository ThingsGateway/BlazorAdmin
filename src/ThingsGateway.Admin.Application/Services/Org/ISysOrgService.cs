

using BootstrapBlazor.Components;

namespace ThingsGateway.Admin.Application;

/// <summary>
/// 机构服务
/// </summary>
public interface ISysOrgService
{

    /// <summary>
    /// 保存机构
    /// </summary>
    /// <param name="input">机构</param>
    /// <param name="type">保存类型</param>
    Task<bool> SaveOrgAsync(SysOrg input, ItemChangedType type);

    /// <summary>
    /// 删除机构
    /// </summary>
    /// <param name="ids">id列表</param>
    Task<bool> DeleteOrgAsync(IEnumerable<long> ids);

    /// <summary>
    /// 报表查询
    /// </summary>
    /// <param name="option">查询条件</param>
    Task<QueryData<SysOrg>> PageAsync(QueryPageOptions option);

    /// <summary>
    /// 机构树结构
    /// </summary>
    /// <param name="input">机构选择器</param>
    /// <returns></returns>
    Task<List<SysOrg>> Tree(SysOrgTreeInput input = null);

    /// <summary>
    /// 机构详情
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<SysOrg> Detail(BaseIdInput input);
}
