namespace api.Models.User.Response
{
    public class TokenRsDto
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }    
        public UserDto User { get; set; }
    }
}
