using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
namespace webapp.Models
{
    public class LoginViewModel
    {


        [Required(ErrorMessage = "Username is required")]
        [RegularExpression(@"^(?=.*[0-9])[a-zA-Z0-9]{5,}$", ErrorMessage = "Username must contain at least one numeric character and be at least 5 characters long.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{7,}$", ErrorMessage = "Password must be at least 7 characters long and contain at least one special character.")]
        public string Password { get; set; }

        public bool RememberMe { get; set; } = false;


    }


}





