using Microsoft.AspNetCore.Identity;

namespace Schedoo.Server.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Patronymic { get; set; }
        public string? Position { get; set; }
        public string? GroupId { get; set; }
        
        public virtual Group Group { get; set; }
    }
}