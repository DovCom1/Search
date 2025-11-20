using Microsoft.Extensions.Options;

namespace Search.Service.Request;

public class RequestFactory(IOptions<RequestDomains> options)
{
    private readonly RequestDomains _domains = options.Value;

    public HttpRequestMessage GetUserByUid(string uid)
    {
        var url = $"{_domains.UserService}{RequestPath.SearchUser}?uid={Uri.EscapeDataString(uid)}";
        return new HttpRequestMessage(HttpMethod.Get, url);
    }

    public HttpRequestMessage GetUserByNickname(string nickname, int offset, int limit)
    {
        var url = $"{_domains.UserService}{RequestPath.SearchUser}?nickname={Uri.EscapeDataString(nickname)}&limit={limit}&offset={offset}";
        return new HttpRequestMessage(HttpMethod.Get, url);
    }
}