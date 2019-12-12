using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Modals
{
    public class ApplicationUser : IdentityUser
    {

        //initial errors on initial request 

//        {
//    "succeeded": false,
//    "errors": [
//        {
//            "code": "PasswordTooShort",
//            "description": "Passwords must be at least 6 characters."
//        },
//        {
//            "code": "PasswordRequiresNonAlphanumeric",
//            "description": "Passwords must have at least one non alphanumeric character."
//        },
//        {
//            "code": "PasswordRequiresLower",
//            "description": "Passwords must have at least one lowercase ('a'-'z')."
//        },
//        {
//            "code": "PasswordRequiresUpper",
//            "description": "Passwords must have at least one uppercase ('A'-'Z')."
//        }
//    ]
//}

            // these errors occurs because of validation inside the identity User class
        [Column(TypeName = "nvarchar(150)")]
        public string FullName { get; set; }
    }
}
