

namespace ThingsGateway.Admin.Application;

/// <summary>
/// 职位服务
/// </summary>
public class SysPositionService : BaseService<SysPosition>, ISysPositionService
{
    public async Task<List<SysPosition>> GetAllAsync()
    {
        var key = $"{CacheConst.Cache_SysPosition}";//系统配置key
        var sysPositions = App.CacheService.Get<List<SysPosition>>(key);
        if (sysPositions.Count == 0)
        {
            using var db = GetDB();
            sysPositions = (await db.Queryable<SysPosition>().ToListAsync().ConfigureAwait(false));
            App.CacheService.Set(key, sysPositions);
        }

        return sysPositions;
    }

    public async Task<SysPosition> GetSysPositionById(long id)
    {
        var list = await GetAllAsync();
        return list.FirstOrDefault(x => x.Id == id);
    }
}
