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

namespace ThingsGateway.Admin.Application;

/// <inheritdoc/>
[ThingsGateway.DependencyInjection.SuppressSniffer]
public class OrgUtil
{
    #region 机构

    /// <summary>
    /// 构造树形数据
    /// </summary>
    /// <param name="items">资源列表</param>
    /// <param name="parentId">父ID</param>
    /// <returns></returns>
    public static IEnumerable<TableTreeNode<SysOrg>> BuildTableTrees(IEnumerable<SysOrg> items, long parentId = 0)
    {
        return items
        .Where(it => it.ParentId == parentId)
        .Select((item, index) =>
            new TableTreeNode<SysOrg>(item)
            {
                HasChildren = items.Any(i => i.ParentId == item.Id),
                IsExpand = items.Any(i => i.ParentId == item.Id),
                Items = BuildTableTrees(items, item.Id).ToList()
            }
        );
    }

    /// <summary>
    /// 构建树节点
    /// </summary>
    public static List<TreeViewItem<SysOrg>> BuildTreeItemList(IEnumerable<SysOrg> sysresources, List<long> selectedItems, Microsoft.AspNetCore.Components.RenderFragment<SysOrg> render, long parentId = 0, TreeViewItem<SysOrg>? parent = null)
    {
        if (sysresources == null) return null;
        var trees = new List<TreeViewItem<SysOrg>>();
        var roots = sysresources.Where(i => i.ParentId == parentId).OrderBy(i => i.SortCode);
        foreach (var node in roots)
        {
            var item = new TreeViewItem<SysOrg>(node)
            {
                Text = node.Name,
                IsActive = selectedItems.Any(v => node.Id == v),
                IsExpand = selectedItems.Any(v => node.Id == v),
                Parent = parent,
                Template = render,
                CheckedState = selectedItems.Any(i => i == node.Id) ? CheckboxState.Checked : CheckboxState.UnChecked
            };
            item.Items = BuildTreeItemList(sysresources, selectedItems, render, node.Id, item) ?? new();
            trees.Add(item);
        }
        return trees;
    }


    /// <summary>
    /// 构造树形
    /// </summary>
    /// <param name="resourceList">资源列表</param>
    /// <param name="parentId">父ID</param>
    /// <returns></returns>
    public static IEnumerable<SysOrg> ConstructOrgTrees(IEnumerable<SysOrg> resourceList, long parentId = 0)
    {
        //找下级资源ID列表
        var resources = resourceList.Where(it => it.ParentId == parentId).OrderBy(it => it.SortCode);
        if (resources.Any())//如果数量大于0
        {
            foreach (var item in resources)//遍历资源
            {
                var children = ConstructOrgTrees(resourceList, item.Id).ToList();//添加子节点
                item.Children = children.Count > 0 ? children : null;
            }
        }
        return resources;
    }

    /// <summary>
    /// 获取父机构集合
    /// </summary>
    /// <param name="allOrgList">所有机构列表</param>
    /// <param name="myOrgs">我的机构列表</param>
    /// <returns></returns>
    public static IEnumerable<SysOrg> GetMyParentOrgs(IEnumerable<SysOrg> allOrgList, IEnumerable<SysOrg> myOrgs)
    {
        var parentList = myOrgs
            .SelectMany(it => OrgUtil.GetOrgParent(allOrgList, it.ParentId))
                                .Where(parent => parent != null
                                && !myOrgs.Contains(parent)
                                && !myOrgs.Any(m => m.Id == parent.Id))
                                .Distinct();
        return parentList;
    }

    /// <summary>
    /// 获取资源所有下级，结果不会转为树形
    /// </summary>
    /// <param name="resourceList">资源列表</param>
    /// <param name="parentId">父Id</param>
    /// <returns></returns>
    public static IEnumerable<SysOrg> GetOrgChilden(IEnumerable<SysOrg> resourceList, long parentId)
    {
        //找下级资源ID列表
        return resourceList.Where(it => it.ParentId == parentId)
                           .SelectMany(item => new List<SysOrg> { item }.Concat(GetOrgChilden(resourceList, item.Id)));
    }

    /// <summary>
    /// 获取资源所有父级，结果不会转为树形
    /// </summary>
    /// <param name="resourceList">资源列表</param>
    /// <param name="resourceId">Id</param>
    /// <returns></returns>
    public static IEnumerable<SysOrg> GetOrgParent(IEnumerable<SysOrg> resourceList, long resourceId)
    {
        //找上级资源ID列表
        return resourceList.Where(it => it.Id == resourceId)
                           .SelectMany(item => new List<SysOrg> { item }.Concat(GetOrgParent(resourceList, item.ParentId)));
    }

    #endregion 机构


}
