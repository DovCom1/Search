using Search.Contract.Manager;
using Search.Contract.Service;
using Search.Model.DTO.User;
using Search.Model.Exceptions;

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
        ValidatePagination(offset, limit);
        return await userSearchService.GetUsersByNicknameAsync(nickname, offset, limit, ct);
    }
    
    private static void ValidatePagination(int offset, int limit)
    {
        if (offset < 0) throw new SearchException($"Offset не может быть отрицательным.", 400);
        if (limit <= 0) throw new SearchException($"Limit должен быть больше нуля.", 400);
        if (limit > 20) throw new SearchException($"Limit не может превышать 20.", 400);
    }
}