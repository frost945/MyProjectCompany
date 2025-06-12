using System.ComponentModel.DataAnnotations;

namespace MyProjectCompany.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Логiн")]
        public string? UserName{ get; set; }

        [Required]
        [Display(Name = "Пароль")]
        [UIHint("password")]
        public string? Password{ get; set; }

        [Display(Name = "Запам'ятати мене?")]
        public bool RememberMe { get; set; }

    }
}
