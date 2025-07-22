using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace EmployeeManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Column(TypeName = "nvarchar(50)")]
        public string FirstName {  get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string LastName { get; set; }
    }
}
