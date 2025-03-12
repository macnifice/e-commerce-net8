namespace api.Models.User.Response
{
    public class ValidateTokenRsDto
    {
        public string UserName { get; set; }
        public required string RefreshToken { get; set; }
        public  DateTime RefreshTokenExpiryTime { get; set; }
    }
}
