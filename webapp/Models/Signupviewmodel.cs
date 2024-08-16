using System.ComponentModel.DataAnnotations;

namespace webapp.Models
{
    public class Signupviewmodel
    {

        [Required(ErrorMessage = "Username is required")]
        [RegularExpression(@"^(?=.*[0-9])[a-zA-Z0-9]{5,}$", ErrorMessage = "Username must contain at least one numeric character and be at least 5 characters long.")]
        public string Username { get; set; }

        public DateTime DataofBirth { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        
        
            [Required(ErrorMessage = "Password is required")]
            [DataType(DataType.Password)]
            [RegularExpression(@"^(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{7,}$", ErrorMessage = "Password must be at least 7 characters long and contain at least one special character.")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        
        public bool Terms { get; set; }=false;


    }

}

