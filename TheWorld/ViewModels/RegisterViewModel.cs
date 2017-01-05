using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheWorld.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Required")]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "Min. 4 Characters.") ]
        public string UserName { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Min. 8 Characters.")]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression("[A-Za-z]+")]
        public string FirstName { get; set; }
        [Required]
        [RegularExpression("[A-Za-z]+")]
        public string FamilyName { get; set; }
    }
}
