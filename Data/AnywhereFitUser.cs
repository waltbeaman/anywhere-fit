using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AnywhereFit.Data
{
    public class AnywhereFitUser : IdentityUser
    {
        [Key]
        public new string Id { get; set; } = null!;
        [Required]
        public new string UserName { get; set; } = null!;
        [Required]
        public new string PasswordHash { get; set; } = null!;

    }
}
