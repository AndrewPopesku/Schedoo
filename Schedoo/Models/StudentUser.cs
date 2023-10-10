using Microsoft.AspNetCore.Identity;

namespace Schedoo.Models
{
    public class StudentUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialty { get; set; }
        public string GroupName { get; set; }
    }
}