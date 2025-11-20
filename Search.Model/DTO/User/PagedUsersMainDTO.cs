namespace Search.Model.DTO.User;

public record PagedUsersMainDTO(IEnumerable<ShortUserDTO> Data,
    int Offset,
    int Limit,
    int Total);