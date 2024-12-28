using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Botanic_Project.Web.Models.Entities
{
    public class User//: IdentityUser
    {
        [Key] //Bu alan, veritabanında birincil anahtar olarak kullanılacak
        [Column("user_id")]// Veritabanındaki sütun adını belirtiyoruz, bu durumda "plant_id"
        public int Id { get; set; } //Guid= Unique identifier
        [Column("first_name")]
        public string FirstName { get; set; }
        [Column("last_name")]
        public string LastName { get; set; }
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Remember me?")]
        [Column("remember_me")]
        public bool RememberMe { get; set; } = false;
    }
}
