
namespace ThingsGateway.Admin.Application;

/// <summary>
/// 职位服务
/// </summary>
public interface ISysPositionService
{
    #region 查询

    /// <summary>
    /// 获取职位列表
    /// </summary>
    /// <returns>职位列表</returns>
    Task<List<SysPosition>> GetAllAsync();

    /// <summary>
    /// 获取职位信息
    /// </summary>
    /// <param name="id">职位ID</param>
    /// <returns>职位信息</returns>
    Task<SysPosition> GetSysPositionById(long id);



    #endregion

}
