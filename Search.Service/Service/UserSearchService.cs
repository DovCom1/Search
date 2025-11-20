using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using System.Text.Json;
using Search.Contract.Service;
using Search.Model.DTO;
using Search.Model.DTO.User;
using Microsoft.Extensions.Options;
using Search.Model.Exceptions;
using Search.Service.Request;

namespace Search.Service.Service;

public class UserSearchService(IHttpClientFactory clientFactory, RequestFactory requestFactory) : IUserSearchService
{
    public async Task<ShortUserDTO> GetUserByUidAsync(string uid, CancellationToken ct = default)
    {
        var client = clientFactory.CreateClient();
        var request = requestFactory.GetUserByUid(uid);
        // var response = await httpClient.GetAsync($"/api/users/search-api?uid={uid}", ct);
        var response = await client.SendAsync(request, ct);
        if (!response.IsSuccessStatusCode)
        {
            await ReadErrorResponse(response, ct);
        }
        var user = await response.Content.ReadFromJsonAsync<ShortUserDTO>(
                       new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, ct)
            ?? throw new NullReferenceException("Пустой ответ");
        return user;
    }

    public async Task<PagedUsersMainDTO> GetUsersByNicknameAsync(string nickname, int offset, int limit,
        CancellationToken ct = default)
    {
        var client = clientFactory.CreateClient();
        var request = requestFactory.GetUserByNickname(nickname, offset, limit);
        var response = await client.SendAsync(request, ct);
        // var response = await httpClient.GetAsync($"/api/users/search-api?nickname={Uri.EscapeDataString(nickname)}&offset={offset}&limit={limit}", ct);
        if (!response.IsSuccessStatusCode)
        {
            await ReadErrorResponse(response, ct);
        }
        var users = await response.Content.ReadFromJsonAsync<PagedUsersMainDTO>(
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, ct)
            ?? throw new NullReferenceException();
        return users;
    }
    
    [DoesNotReturn]
    private static async Task ReadErrorResponse(HttpResponseMessage response, CancellationToken ct)
    {
        var errorDto = await response.Content.ReadFromJsonAsync<ErrorDTO>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, ct);
        throw new SearchException(
            errorDto?.Error ?? "Неизвестная ошибка",
            errorDto?.Status ?? (int)response.StatusCode);
    }
}