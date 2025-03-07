﻿//------------------------------------------------------------------------------
//  此代码版权声明为全文件覆盖，如有原作者特别声明，会在下方手动补充
//  此代码版权（除特别声明外的代码）归作者本人Diego所有
//  源代码使用协议遵循本仓库的开源协议及附加协议
//  Gitee源代码仓库：https://gitee.com/diego2098/BlazorAdmin
//  Github源代码仓库：https://github.com/kimdiego2098/BlazorAdmin
//  使用文档：https://thingsgateway.cn/
//  QQ群：605534569
//------------------------------------------------------------------------------

using Furion.LinqBuilder;

using BlazorAdmin.Application;

namespace BlazorAdmin.Razor;

public partial class SysUserPage
{
    private SysUser? SearchModel { get; set; } = new();
    private long OrgId { get; set; }

    [Inject]
    [NotNull]
    private ISysUserService? SysUserService { get; set; }

    [Inject]
    [NotNull]
    private ISysResourceService? SysResourceService { get; set; }

    #region 查询

    private async Task<QueryData<SysUser>> OnQueryAsync(QueryPageOptions options)
    {
        var data = await SysUserService.PageAsync(options, new UserSelectorInput() { OrgId = OrgId });
        return data;
    }
    private Task TreeChangedAsync(long id)
    {
        OrgId = id;
        return table.QueryAsync();
    }
    #endregion 查询

    #region 修改

    private async Task<bool> Delete(IEnumerable<SysUser> sysUsers)
    {
        try
        {
            return await SysUserService.DeleteUserAsync(sysUsers.Select(a => a.Id));
        }
        catch (Exception ex)
        {
            await ToastService.Warn(ex);
            return false;
        }
    }

    private async Task GrantApi(long id)
    {
        var hasResources = (await SysUserService.ApiOwnPermissionAsync(id))?.GrantInfoList;
        var ids = new List<string>();
        ids.AddRange(hasResources.Select(a => a.ApiUrl));

        var op = new DialogOption()
        {
            IsScrolling = true,
            Size = Size.ExtraLarge,
            Title = OperDescLocalizer["UserGrantApiPermission"],
            ShowCloseButton = false,
            ShowMaximizeButton = true,
            ShowSaveButton = true,
            OnSaveAsync = async () =>
            {
                try
                {
                    GrantPermissionData data = new();
                    data.Id = id;
                    data.GrantInfoList = ids.Select(a => new RelationPermission() { ApiUrl = a });
                    await SysUserService.GrantApiPermissionAsync(data);
                    await ToastService.Default();
                    return true;
                }
                catch (Exception ex)
                {
                    await ToastService.Warn(ex);
                    return false;
                }
            },
            Class = "dialog-table",
            BodyTemplate = BootstrapDynamicComponent.CreateComponent<GrantApiDialog>(new Dictionary<string, object?>
            {
                [nameof(GrantApiDialog.Value)] = ids,
                [nameof(GrantApiDialog.ValueChanged)] = (List<string> v) => { ids = v; return Task.CompletedTask; },
            }).Render(),
        };

        await DialogService.Show(op);
    }

    private async Task GrantResource(long id)
    {
        var grantInfoList = (await SysUserService.OwnResourceAsync(id))?.GrantInfoList.ToList();

        var menuData = grantInfoList.Select(a => a.MenuId);
        var buttonData = grantInfoList.SelectMany(a => a.ButtonIds);
        var value = menuData.Concat(buttonData).ToList();
        var op = new DialogOption()
        {
            IsScrolling = true,
            Size = Size.ExtraLarge,
            Title = OperDescLocalizer["UserGrantResource"],
            ShowCloseButton = false,
            ShowMaximizeButton = true,
            ShowSaveButton = true,
            OnSaveAsync = async () =>
            {
                try
                {
                    GrantResourceData data = new();


                    var allResource = await SysResourceService.GetAllAsync();
                    var resources = allResource.Where(a => value.Contains(a.Id));
                    var pResources = SysResourceService.GetMyParentResources(allResource, resources);
                    var grantInfoList = new List<RelationResourcePermission>();
                    foreach (var item in pResources.Concat(resources).Distinct().Where(a => a.Category == ResourceCategoryEnum.Menu && !a.Href.IsNullOrEmpty()))
                    {
                        var relationResourcePermission = new RelationResourcePermission();
                        relationResourcePermission.MenuId = item.Id;
                        relationResourcePermission.ButtonIds = SysResourceService.GetResourceChilden(allResource, item.Id).Where(a => value.Contains(a.Id)).Select(a => a.Id).ToHashSet();
                        grantInfoList.Add(relationResourcePermission);
                    }

                    var buttons = resources.Where(a => a.Category == ResourceCategoryEnum.Button && a.ParentId == 0);
                    grantInfoList.Add(new RelationResourcePermission()
                    {
                        MenuId = 0,
                        ButtonIds = buttons.Select(a => a.Id).ToHashSet()
                    });

                    data.GrantInfoList = grantInfoList;
                    data.Id = id;

                    await SysUserService.GrantResourceAsync(data);

                    await ToastService.Default();
                    return true;
                }
                catch (Exception ex)
                {
                    await ToastService.Warn(ex);
                    return false;
                }

            },
            Class = "dialog-table",
            BodyTemplate = BootstrapDynamicComponent.CreateComponent<GrantResourceDialog>(new Dictionary<string, object?>
            {
                [nameof(GrantResourceDialog.Value)] = value,
                [nameof(GrantResourceDialog.ValueChanged)] = (List<long> v) => { value = v; return Task.CompletedTask; },
            }).Render(),
        };
        await DialogService.Show(op);
    }

    public HashSet<long> GrantRoleChoiceValues { get; set; }
    private async Task GrantRole(long id)
    {
        var data = (await SysUserService.OwnRoleAsync(id)).ToHashSet();
        GrantRoleChoiceValues = data;
        var option = new DialogOption()
        {
            IsScrolling = true,
            Size = Size.ExtraExtraLarge,
            Title = OperDescLocalizer["UserGrantRole"],
            ShowMaximizeButton = true,
            Class = "dialog-table",
            ShowSaveButton = true,
            OnSaveAsync = async () =>
            {
                try
                {
                    await OnGrantRoleValueChanged(GrantRoleChoiceValues, id, true);
                    await ToastService.Default();
                    return true;
                }
                catch (Exception ex)
                {
                    await ToastService.Warn(ex);
                    return false;
                }
            },
            BodyTemplate = BootstrapDynamicComponent.CreateComponent<RoleChoiceDialog>(new Dictionary<string, object?>
            {
                [nameof(RoleChoiceDialog.Values)] = data,
                [nameof(RoleChoiceDialog.ValuesChanged)] = (HashSet<long> v) => OnGrantRoleValueChanged(v, id),

            }).Render(),

        };
        await DialogService.Show(option);
    }

    private async Task OnGrantRoleValueChanged(HashSet<long> values, long userId, bool change = false)
    {
        if (GrantRoleChoiceValues != values)
        {
            GrantRoleChoiceValues = values;
        }
        if (change)
        {
            GrantUserOrRoleInput userGrantRoleInput = new();
            userGrantRoleInput.Id = userId;
            userGrantRoleInput.GrantInfoList = GrantRoleChoiceValues;
            await SysUserService.GrantRoleAsync(userGrantRoleInput);
        }
    }


    private async Task ResetPassword(long id)
    {
        try
        {
            await SysUserService.ResetPasswordAsync(id);
            await ToastService.Default();
        }
        catch (Exception ex)
        {
            await ToastService.Warn(ex);
        }
    }

    private async Task<bool> Save(SysUser sysUser, ItemChangedType itemChangedType)
    {
        try
        {
            return await SysUserService.SaveUserAsync(sysUser, itemChangedType);
        }
        catch (Exception ex)
        {
            await ToastService.Warn(ex);
            return false;
        }
    }

    #endregion 修改
}
