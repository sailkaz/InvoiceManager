using System.ComponentModel.DataAnnotations;

namespace InvoiceManager.Models
{
    public class LoginViewModel
    {
        //public LoginViewModel(string email, string password, bool rememberMe)
        //{
        //    Email = email;
        //    Password = password;
        //    RememberMe = rememberMe;
        //}

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [Display(Name = "Zapamiętaj?")]
        public bool RememberMe { get; set; }
    }
}
