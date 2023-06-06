using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class User : Entity
    {
        [Required(AllowEmptyStrings = false)]
        public string Username { get; set; } = string.Empty;
        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;
        [EmailAddress(ErrorMessage = "błędny email")]
        public string Email { get; set; } = string.Empty;

        public Roles Roles { get; set; }
    }
}
