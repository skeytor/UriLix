using Microsoft.AspNetCore.Authentication.OAuth;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace UriLix.Infrastructure.Security.Helpers;

internal static class GitHubHelpers
{
    internal static async Task<JsonElement> GetUserInfo(OAuthCreatingTicketContext ctx, string url)
    {
        using HttpRequestMessage request = new(HttpMethod.Get, url);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ctx.AccessToken);
        using HttpResponseMessage response = await ctx.Backchannel.SendAsync(request, ctx.HttpContext.RequestAborted);
        response.EnsureSuccessStatusCode();
        JsonElement jsonData = await response.Content.ReadFromJsonAsync<JsonElement>(ctx.HttpContext.RequestAborted);
        return jsonData;
    }
}
