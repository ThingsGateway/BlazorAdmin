//------------------------------------------------------------------------------
//  此代码版权声明为全文件覆盖，如有原作者特别声明，会在下方手动补充
//  此代码版权（除特别声明外的代码）归作者本人Diego所有
//  源代码使用协议遵循本仓库的开源协议及附加协议
//  Gitee源代码仓库：https://gitee.com/diego2098/ThingsGateway
//  Github源代码仓库：https://github.com/kimdiego2098/ThingsGateway
//  使用文档：https://thingsgateway.cn/
//  QQ群：605534569
//------------------------------------------------------------------------------

using BootstrapBlazor.Components;

using SqlSugar;

using ThingsGateway.Extension.Generic;
using ThingsGateway.FriendlyException;

namespace ThingsGateway.Admin.Application;

internal class SysOrgService : BaseService<SysOrg>, ISysOrgService
{
    private ISysPositionService _sysPositionService;
    public SysOrgService(ISysPositionService sysPositionService)
    {
        _sysPositionService = sysPositionService;
    }

    [OperDesc("CopyOrg")]
    public async Task Copy(SysOrgCopyInput input)
    {
        var orgList = await GetAllAsync();//获取所有
        var positionList = await _sysPositionService.GetAllAsync();//获取所有职位
        var ids = new HashSet<long>();//定义不重复Id集合
        var addOrgList = new List<SysOrg>();//添加机构列表
        var addPositionList = new List<SysPosition>();//添加职位列表
        var alreadyIds = new HashSet<long>();//定义已经复制过得组织Id
        ids.AddRange(input.Ids);//加到集合
        if (ids.Contains(input.TargetId))
            throw Oops.Bah(Localizer["CanotContainsSelf"]);
        //获取目标组织
        var target = orgList.Where(it => it.Id == input.TargetId).FirstOrDefault();
        if (target != null)
        {
            //需要复制的组织名称列表
            var orgNames = orgList.Where(it => ids.Contains(it.Id)).Select(it => it.Name).ToList();
            //目标组织的一级子组织名称列表
            var targetChildNames = orgList.Where(it => it.ParentId == input.TargetId).Select(it => it.Name).ToList();
            orgNames.ForEach(it =>
            {
                if (targetChildNames.Contains(it)) throw Oops.Bah(Localizer["TargetNameDup", it]);
            });
            foreach (var id in input.Ids)
            {
                var org = orgList.Where(o => o.Id == id).FirstOrDefault();//获取组织
                if (org != null && !alreadyIds.Contains(id))
                {
                    alreadyIds.Add(id);//添加到已复制列表
                    RedirectOrg(org);//生成新的实体
                    org.ParentId = input.TargetId;//父id为目标Id
                    addOrgList.Add(org);
                    //是否包含职位
                    if (input.ContainsPosition)
                    {
                        var positions = positionList.Where(p => p.OrgId == id).ToList();//获取机构下的职位
                        positions.ForEach(p =>
                        {
                            p.OrgId = org.Id;//赋值新的机构ID
                            p.Id = CommonUtils.GetSingleId();//生成新的ID
                            p.Code = Random.Shared.Next().ToString();//生成新的Code
                            addPositionList.Add(p);//添加到职位列表
                        });
                    }
                    //是否包含下级
                    if (input.ContainsChild)
                    {
                        var childIds = await GetOrgChildIds(id, false);//获取下级id列表
                        alreadyIds.AddRange(childIds);//添加到已复制id
                        var childList = orgList.Where(c => childIds.Contains(c.Id)).ToList();//获取下级
                        var sysOrgChildren = CopySysOrgChildren(childList, id, org.Id, input.ContainsPosition,
                            positionList);//赋值下级组织
                        addOrgList.AddRange(sysOrgChildren.Item1);//添加到组织列表
                        addPositionList.AddRange(sysOrgChildren.Item2);//添加到职位列表
                    }
                }
            }
            orgList.AddRange(addOrgList);//要添加的组织添加到组织列表
            //遍历机构重新赋值全称和父Id列表
            addOrgList.ForEach(it =>
            {
                it.Names = it.Name;
                if (it.ParentId != 0)
                {
                    var parentIdList = GetNames(orgList, it.ParentId, it.Name, out var names);
                    it.Names = names;
                    it.ParentIdList = parentIdList;
                }
            });

            using var db = GetDB();
            //事务
            var result = await db.UseTranAsync(async () =>
            {
                await db.Insertable(addOrgList).ExecuteCommandAsync();//插入组织
                if (addPositionList.Count > 0)
                {
                    await db.Insertable(addPositionList).ExecuteCommandAsync();
                }
            }).ConfigureAwait(false);
            if (result.IsSuccess)//如果成功了
            {
                RefreshCache();//刷新缓存
            }
            else
            {
                throw new(result.ErrorMessage, result.ErrorException);
            }


        }
    }

    [OperDesc("DeleteOrg")]
    public async Task<bool> DeleteOrgAsync(IEnumerable<long> ids)
    {
        var result = await base.DeleteAsync(ids).ConfigureAwait(false);
        if (result)
            RefreshCache();
        return result;
    }


    /// <summary>
    /// 从缓存/数据库获取系统配置列表
    /// </summary>
    /// <returns></returns>
    public async Task<List<SysOrg>> GetAllAsync()
    {
        var key = $"{CacheConst.Cache_SysOrg}";//系统配置key
        var sysOrgs = App.CacheService.Get<List<SysOrg>>(key);
        if (sysOrgs == null)
        {
            using var db = GetDB();
            sysOrgs = (await db.Queryable<SysOrg>().ToListAsync().ConfigureAwait(false));
            App.CacheService.Set(key, sysOrgs);
        }

        return sysOrgs;
    }

    /// <summary>
    /// 表格查询
    /// </summary>
    /// <param name="option"></param>
    /// <returns></returns>
    public Task<QueryData<SysOrg>> PageAsync(QueryPageOptions option)
    {
        return QueryAsync(option);
    }

    [OperDesc("SaveOrg")]
    public async Task<bool> SaveOrgAsync(SysOrg input, ItemChangedType type)
    {
        await CheckInput(input).ConfigureAwait(false);//检查参数
        var reuslt = await base.SaveAsync(input, type).ConfigureAwait(false);
        if (reuslt)
            RefreshCache();

        return reuslt;
    }

    #region 方法
    /// <summary>
    /// 赋值组织的所有下级
    /// </summary>
    /// <param name="orgList">组织列表</param>
    /// <param name="parentId">父Id</param>
    /// <param name="newParentId">新父Id</param>
    /// <param name="isCopyPosition"></param>
    /// <param name="positions"></param>
    /// <returns></returns>
    public Tuple<List<SysOrg>, List<SysPosition>> CopySysOrgChildren(List<SysOrg> orgList, long parentId, long newParentId,
        bool isCopyPosition,
        List<SysPosition> positions)
    {
        //找下级组织列表
        var orgInfos = orgList.Where(it => it.ParentId == parentId).ToList();
        if (orgInfos.Count > 0)//如果数量大于0
        {
            var result = new Tuple<List<SysOrg>, List<SysPosition>>(
                new List<SysOrg>(), new List<SysPosition>()
                );
            foreach (var item in orgInfos)//遍历组织
            {
                var oldId = item.Id;//获取旧Id
                RedirectOrg(item);//实体重新赋值
                var children = CopySysOrgChildren(orgList, oldId, item.Id, isCopyPosition,
                    positions);//获取子节点
                item.ParentId = newParentId;//赋值新的父Id
                result.Item1.AddRange(children.Item1);//添加下级组织;
                if (isCopyPosition)//如果包含职位
                {
                    var positionList = positions.Where(it => it.OrgId == oldId).ToList();//获取职位列表
                    positionList.ForEach(it =>
                    {
                        it.OrgId = item.Id;//赋值新的机构ID
                        it.Id = CommonUtils.GetSingleId();//生成新的ID
                        it.Code = Random.Shared.Next().ToString();//生成新的Code
                    });
                    result.Item2.AddRange(positionList);//添加职位列表
                }
            }
            return result;//返回结果
        }
        return new Tuple<List<SysOrg>, List<SysPosition>>(
            new List<SysOrg>(), new List<SysPosition>()
            );
    }

    /// <inheritdoc />
    public async Task<List<long>> GetOrgChildIds(long orgId, bool isContainOneself = true, List<SysOrg> sysOrgList = null)
    {
        var orgIds = new List<long>();//组织列表
        if (orgId > 0)//如果orgId有值
        {
            //获取所有子集
            var childList = await GetChildListById(orgId, isContainOneself, sysOrgList);
            orgIds = childList.Select(x => x.Id).ToList();//提取ID列表
        }
        return orgIds;
    }

    /// <inheritdoc />
    public async Task<List<SysOrg>> GetChildListById(long orgId, bool isContainOneself = true, List<SysOrg> sysOrgList = null)
    {
        //获取所有组织
        sysOrgList ??= await GetAllAsync();
        //查找下级
        var childList = GetSysOrgChildren(sysOrgList, orgId);
        if (isContainOneself)//如果包含自己
        {
            //获取自己的组织信息
            var self = sysOrgList.Where(it => it.Id == orgId).FirstOrDefault();
            if (self != null) childList.Insert(0, self);//如果组织不为空就插到第一个
        }
        return childList;
    }

    /// <summary>
    /// 获取组织所有下级
    /// </summary>
    /// <param name="orgList"></param>
    /// <param name="parentId"></param>
    /// <returns></returns>
    public List<SysOrg> GetSysOrgChildren(List<SysOrg> orgList, long parentId)
    {
        //找下级组织ID列表
        var orgInfos = orgList.Where(it => it.ParentId == parentId).ToList();
        if (orgInfos.Count > 0)//如果数量大于0
        {
            var data = new List<SysOrg>();
            foreach (var item in orgInfos)//遍历组织
            {
                var children = GetSysOrgChildren(orgList, item.Id);//获取子节点
                data.AddRange(children);//添加子节点);
                data.Add(item);//添加到列表
            }
            return data;//返回结果
        }
        return new List<SysOrg>();
    }


    /// <summary>
    /// 重新生成组织实体
    /// </summary>
    /// <param name="org"></param>
    private void RedirectOrg(SysOrg org)
    {
        //重新生成ID并赋值
        var newId = CommonUtils.GetSingleId();
        org.Id = newId;
        org.Code = Random.Shared.Next().ToString();
        org.CreateTime = DateTime.Now;
        org.CreateUser = UserManager.UserAccount;
        org.CreateUserId = UserManager.UserId;
    }
    /// <summary>
    /// 检查输入参数
    /// </summary>
    private async Task CheckInput(SysOrg input)
    {
        //判断分类是否正确
        if (input.Category != OrgEnum.COMPANY && input.Category != OrgEnum.DEPT)
            throw Oops.Bah(Localizer["Dup", input.Category, input.Name]);

        var sysOrgList = await GetAllAsync();//获取全部
        if (sysOrgList.Any(it => it.ParentId == input.ParentId && it.Name == input.Name && it.Id != input.Id))//判断同级是否有名称重复的
            throw Oops.Bah(Localizer["NameDup", input.Name]);
        input.Names = input.Name;//全称默认自己
        if (input.ParentId != 0)
        {
            //获取父级,判断父级ID正不正确
            var parent = sysOrgList.Where(it => it.Id == input.ParentId).FirstOrDefault();
            if (parent != null)
            {
                if (parent.Id == input.Id)
                    throw Oops.Bah(Localizer["ParentChoiceSelf"]);
            }
            else
            {
                throw Oops.Bah(Localizer["ParentNull", input.Id]);
            }
            var parentIdList = GetNames(sysOrgList, input.ParentId, input.Name, out var names);
            input.Names = names;
            input.ParentIdList = parentIdList;
        }
        //如果code没填
        if (string.IsNullOrEmpty(input.Code))
        {
            input.Code = Random.Shared.Next().ToString();
        }
        else
        {
            //判断是否有相同的Code
            if (sysOrgList.Any(it => it.Code == input.Code && it.Id != input.Id))
                throw Oops.Bah(Localizer["CodeDup", input.Code]);
        }
    }



    /// <summary>
    /// 刷新缓存
    /// </summary>
    /// <returns></returns>
    private void RefreshCache()
    {
        App.CacheService.Remove($"{CacheConst.Cache_SysOrg}");
        App.CacheService.Remove($"{CacheConst.Cache_SysUser}");
        App.CacheService.Remove($"{CacheConst.Cache_SysCompany}");
    }


    /// <summary>
    /// 获取全称
    /// </summary>
    /// <param name="sysOrgList">组织列表</param>
    /// <param name="parentId">父Id</param>
    /// <param name="orgName">组织名称</param>
    /// <param name="names">组织全称</param>
    /// <returns>组织父Id列表</returns>
    public List<long> GetNames(List<SysOrg> sysOrgList, long parentId, string orgName, out string names)
    {
        names = string.Empty;
        //获取父级菜单
        var parents = GetOrgParents(sysOrgList, parentId);
        foreach (var item in parents)
        {
            names += $"{item.Name}/";
        }
        names += orgName;//赋值全称
        var parentIdList = parents.Select(it => it.Id).ToList();//赋值父Id列表
        return parentIdList;
    }


    /// <inheritdoc />
    public List<SysOrg> GetOrgParents(List<SysOrg> allOrgList, long orgId, bool includeSelf = true)
    {
        //找到组织
        var sysOrgList = allOrgList.Where(it => it.Id == orgId).FirstOrDefault();
        if (sysOrgList != null)//如果组织不为空
        {
            var data = new List<SysOrg>();
            var parents = GetOrgParents(allOrgList, sysOrgList.ParentId, includeSelf);//递归获取父节点
            data.AddRange(parents);//添加父节点;
            if (includeSelf)
                data.Add(sysOrgList);//添加到列表
            return data;//返回结果
        }
        return new List<SysOrg>();
    }

    #endregion 方法
}
