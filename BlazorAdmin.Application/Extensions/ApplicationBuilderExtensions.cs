﻿//------------------------------------------------------------------------------
//  此代码版权声明为全文件覆盖，如有原作者特别声明，会在下方手动补充
//  此代码版权（除特别声明外的代码）归作者本人Diego所有
//  源代码使用协议遵循本仓库的开源协议及附加协议
//  Gitee源代码仓库：https://gitee.com/diego2098/BlazorAdmin
//  Github源代码仓库：https://github.com/kimdiego2098/BlazorAdmin
//  使用文档：https://thingsgateway.cn/
//  QQ群：605534569
//------------------------------------------------------------------------------

using Microsoft.AspNetCore.Http;

using System.Net;
using System.Text.Json;

namespace Microsoft.AspNetCore.Builder;

[Furion.DependencyInjection.SuppressSniffer]
public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseBootstrapBlazor(this IApplicationBuilder builder)
    {
        // 获得客户端 IP 地址
        builder.UseWhen(context => context.Request.Path.StartsWithSegments("/ip.axd"), app => app.Run(async context =>
        {
            var ip = "";
            var headers = context.Request.Headers;
            if (headers.TryGetValue("X-Forwarded-For", out var value))
            {
                var ips = new List<string>();
                foreach (var xf in value)
                {
                    if (!string.IsNullOrEmpty(xf))
                    {
                        ips.Add(xf);
                    }
                }
                ip = string.Join(";", ips);
            }
            else
            {
                ip = context.Connection.RemoteIpAddress.ToIPv4String();
            }

            context.Response.Headers.TryAdd("Content-Type", new Microsoft.Extensions.Primitives.StringValues("application/json; charset=utf-8"));
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { Id = context.TraceIdentifier, Ip = ip })).ConfigureAwait(false);
        }));
        return builder;
    }
    public static string ToIPv4String(this IPAddress? address)
    {
        var ipv4Address = (address ?? IPAddress.IPv6Loopback).ToString();
        return ipv4Address.StartsWith("::ffff:") ? (address ?? IPAddress.IPv6Loopback).MapToIPv4().ToString() : ipv4Address;
    }
}
