//------------------------------------------------------------------------------
//  此代码版权声明为全文件覆盖，如有原作者特别声明，会在下方手动补充
//  此代码版权（除特别声明外的代码）归作者本人Diego所有
//  源代码使用协议遵循本仓库的开源协议及附加协议
//  Gitee源代码仓库：https://gitee.com/diego2098/BlazorAdmin
//  Github源代码仓库：https://github.com/kimdiego2098/BlazorAdmin
//  使用文档：https://thingsgateway.cn/
//  QQ群：605534569
//------------------------------------------------------------------------------

using BootstrapBlazor.Components;

using BlazorAdmin.Json.Extension;

using BlazorAdmin.Common;


namespace BlazorAdmin.Application;

internal sealed class AuthRazorService : IAuthRazorService
{

    private AjaxService AjaxService { get; set; }
    public AuthRazorService(AjaxService ajaxService)
    {
        AjaxService = ajaxService;
    }
    /// <summary>
    /// 用户登录
    /// </summary>
    public async Task<UnifyResult<LoginOutput>> LoginAsync(LoginInput input)
    {
        var ajaxOption = new AjaxOption
        {
            Url = "/api/auth/login",
            Method = "POST",
            ToJson = true,
            Data = input,
        };
        var str = await AjaxService.InvokeAsync(ajaxOption).ConfigureAwait(false);
        if (str != null)
        {
            var ret = str.RootElement.GetRawText().FromJsonNetString<UnifyResult<LoginOutput>>();
            return ret;
        }
        else
        {
            throw new ArgumentNullException();
        }
    }

    /// <summary>
    /// 注销当前用户
    /// </summary>
    public async Task<UnifyResult<object>> LoginOutAsync()
    {
        var ajaxOption = new AjaxOption
        {
            Url = "/api/auth/logout",
            Method = "POST",
            ToJson = true,
        };
        using var str = await AjaxService.InvokeAsync(ajaxOption).ConfigureAwait(false);
        if (str != null)
        {
            var ret = str.RootElement.GetRawText().FromJsonNetString<UnifyResult<object>>();
            return ret;
        }
        else
        {
            throw new ArgumentNullException();
        }
    }
}
