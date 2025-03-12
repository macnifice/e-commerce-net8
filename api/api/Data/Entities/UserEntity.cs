using System.ComponentModel.DataAnnotations;

namespace api.Data.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El nombre de usuario no puede superar los 50 caracteres.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string PasswordHash { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El apellido no puede superar los 100 caracteres.")]
        public string LastName { get; set; }
        [EmailAddress(ErrorMessage = "Debe ser una dirección de correo válida.")]
        [MaxLength(255, ErrorMessage = "El correo no puede superar los 255 caracteres.")]
        public string Email { get; set; }
        [MaxLength(500, ErrorMessage = "La dirección no puede superar los 500 caracteres.")]
        public string Address { get; set; }
        [Required(ErrorMessage = "El rol es obligatorio.")]
        [MaxLength(20, ErrorMessage = "El rol no puede superar los 20 caracteres.")]
        public string Role { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public ICollection<CustomerArticleEntity> CustomerArticle { get; set; }
    }
}
