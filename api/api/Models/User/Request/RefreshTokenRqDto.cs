using Microsoft.Extensions.Configuration.UserSecrets;

namespace api.Models.User.Request
{
    public class RefreshTokenRqDto
    {
        public int UserId { get; set; }
        public required string RefreshToken { get; set; }
    }
}
