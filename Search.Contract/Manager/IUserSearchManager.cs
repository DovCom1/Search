using Search.Model.DTO.User;

namespace Search.Contract.Manager;

public interface IUserSearchManager
{
    public Task<ShortUserDTO> SearchUserByUidAsync(string uid, CancellationToken ct);

    public Task<PagedUsersMainDTO> SearchUsersByNicknameAsync(string nickname, int offset, int limit,
        CancellationToken ct);
}