using Search.Model.DTO.User;

namespace Search.Contract.Service;

public interface IUserSearchService
{
    public Task<ShortUserDTO> GetUserByUidAsync(string uid, CancellationToken ct = default);

    public Task<PagedUsersMainDTO> GetUsersByNicknameAsync(string nickname, int offset, int limit,
        CancellationToken ct = default);
}