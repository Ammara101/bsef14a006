using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EadProjectShoppingApp.Models
{

    [MetadataType(typeof(UserMetadata))]

    public partial class User
    {
        public String ConfirmPassword { get; set; }

    }

    public class UserMetadata
        {
            [Display(Name = "First Name")]
            [Required(AllowEmptyStrings = false, ErrorMessage = "First Name Required")]

            public String FirstName { get; set; }


            [Display(Name = "Last Name")]
            [Required(AllowEmptyStrings = false, ErrorMessage = "Last Name Required")]

           public String LastName { get; set; }


            [Display(Name = "Email")]
            [Required(AllowEmptyStrings = false, ErrorMessage = "EmailID is required")]
            [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Adresse mail non valide !")]
            [DataType(DataType.EmailAddress)]
            public String EmailId { get; set; }

        


            [Display(Name = "Password")]
            [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
            [DataType(DataType.Password)]
            [MinLength(6, ErrorMessage = "Password should have 6 haracters atleast")]
            public String Password { get; set; }



            [Display(Name = "Confirm Password")]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Confirm Password and Password does not match")]
            public String ConfirmPassword { get; set; }
            

           //[Display(Name = "SignUp as Admin or User")]
           //[Required(AllowEmptyStrings = false, ErrorMessage = "Please check the check box if you want to register as admin")]
           
           //public bool IsAdmin { get; set; }

    }

}