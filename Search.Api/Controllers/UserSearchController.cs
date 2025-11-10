using Microsoft.AspNetCore.Mvc;
using Search.Contract.Manager;

namespace Search.API.Controllers;


[ApiController]
[Route("api/users/search")]
public class UserSearchController(IUserSearchManager userSearchManager) : ControllerBase
{
    [HttpGet("")]
    public async Task<ActionResult<object>> Search(
        [FromQuery] string? uid,
        [FromQuery] string? nickname,
        CancellationToken ct,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 10)
    {
        if (string.IsNullOrWhiteSpace(uid) && string.IsNullOrWhiteSpace(nickname))
        {
            return BadRequest("Укажите Uid или Никнейм для поиска");
        }
        if (!string.IsNullOrWhiteSpace(uid))
        {
            var user = await userSearchManager.SearchUserByUidAsync(uid, ct);
            return Ok(user);
        }
        if (!string.IsNullOrWhiteSpace(nickname))
        {
            var users = await userSearchManager.SearchUsersByNicknameAsync(nickname, offset, limit, ct);
            return Ok(users);
        }
        return BadRequest("Неподдерживаемый запрос");
    }
}