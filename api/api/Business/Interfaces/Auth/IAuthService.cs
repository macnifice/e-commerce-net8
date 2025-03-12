using api.Models.User;
using api.Models.User.Request;
using api.Models.User.Response;

namespace api.Business.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<RegisterUserRsDto> RegisterAsync(RegisterUserRqDto rq);

        Task<TokenRsDto> LoginAsync(LoginUserRqDto rq);

        Task<TokenRsDto> RefreshTokenAsync(RefreshTokenRqDto rq);

        Task<UserViewDto> UserInfo(int id); 

    }
}
