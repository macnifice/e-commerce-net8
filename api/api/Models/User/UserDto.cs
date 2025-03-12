﻿using System.ComponentModel.DataAnnotations;

namespace api.Models.User
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string RefreshToken { get; set; }
    }
}
