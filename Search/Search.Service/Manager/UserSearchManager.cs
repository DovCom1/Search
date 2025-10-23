using Search.Contract.Manager;
using Search.Contract.Service;
using Search.Model.DTO.User;

namespace Search.Service.Manager;

public class UserSearchManager(IUserSearchService userSearchService) : IUserSearchManager
{
    public async Task<ShortUserDTO> SearchUserByUidAsync(string uid, CancellationToken ct)
    {
        return await userSearchService.GetUserByUidAsync(uid, ct);
    }

    public async Task<PagedUsersMainDTO> SearchUsersByNicknameAsync(string nickname, int offset, int limit,
        CancellationToken ct)
    {
        return await userSearchService.GetUsersByNicknameAsync(nickname, offset, limit, ct);
    }
}