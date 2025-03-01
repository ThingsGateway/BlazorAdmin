﻿//------------------------------------------------------------------------------
//  此代码版权声明为全文件覆盖，如有原作者特别声明，会在下方手动补充
//  此代码版权（除特别声明外的代码）归作者本人Diego所有
//  源代码使用协议遵循本仓库的开源协议及附加协议
//  Gitee源代码仓库：https://gitee.com/diego2098/BlazorAdmin
//  Github源代码仓库：https://github.com/kimdiego2098/BlazorAdmin
//  使用文档：https://thingsgateway.cn/
//  QQ群：605534569
//------------------------------------------------------------------------------

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

using System.Net;
using System.Security.Claims;

using UAParser;

namespace BlazorAdmin.Application;

public class HybridAppService : IAppService
{
    public HybridAppService()
    {
        var str = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/127.0.0.0 Safari/537.36 Edg/127.0.0.0";
        ClientInfo = Parser.GetDefault().Parse(str);
        RemoteIpAddress = IPAddress.Parse("127.0.0.1");
    }
    public ClientInfo? ClientInfo { get; }

    private static BlazorHybridAuthenticationStateProvider _authenticationStateProvider;
    private static BlazorHybridAuthenticationStateProvider AuthenticationStateProvider
    {
        get
        {
            if (_authenticationStateProvider == null)
                _authenticationStateProvider = ((BlazorHybridAuthenticationStateProvider)App.RootServices.GetService<AuthenticationStateProvider>());

            return _authenticationStateProvider;
        }
    }

    private ClaimsPrincipal? user;

    public ClaimsPrincipal? User
    {
        get { return user; }
        internal set
        {
            if (user == value)
            {
                return;
            }
            user = value;
            AuthenticationStateProvider.UserChanged(user);
        }
    }

    public IPAddress? RemoteIpAddress { get; }

    public string GetReturnUrl(string returnUrl)
    {
        return returnUrl;
    }

    public Task LoginOutAsync()
    {
        User = new ClaimsPrincipal(new ClaimsIdentity());
        return Task.CompletedTask;
    }

    public Task LoginAsync(ClaimsIdentity claimsIdentity, int expire)
    {
        User = new ClaimsPrincipal(claimsIdentity);
        return Task.CompletedTask;
    }

}
