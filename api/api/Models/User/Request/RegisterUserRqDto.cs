namespace api.Models.User.Request
{
    public class RegisterUserRqDto
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }
    }
}
