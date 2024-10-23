

using BootstrapBlazor.Components;

namespace ThingsGateway.Admin.Application;

/// <summary>
/// 机构服务
/// </summary>
public interface ISysOrgService
{
    /// <summary>
    /// 复制组织
    /// </summary>
    /// <param name="input">机构复制参数</param>
    /// <returns></returns>
    Task Copy(SysOrgCopyInput input);
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
    Task<List<SysOrg>> GetAllAsync();
}
