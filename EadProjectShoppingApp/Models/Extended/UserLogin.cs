using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EadProjectShoppingApp.Models.Extended
{
    public class UserLogin
    {

        [Display(Name = "Email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "EmailID is required")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Adresse mail non valide !")]
        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; }


        [Display(Name = "Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]

        public string Password { get; set; }


        [Display(Name = "Remembr me")]

        public bool RememberMe { get; set; }

    }
}