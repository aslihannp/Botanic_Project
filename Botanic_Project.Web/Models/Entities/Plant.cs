using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Botanic_Project.Web.Models.Entities
{
    public class Plant
    {
        [Key] //Bu alan, veritabanında birincil anahtar olarak kullanılacak
        [Column("plant_id")]// Veritabanındaki sütun adını belirtiyoruz, bu durumda "plant_id"
        public int Id { get; set; } //Guid= Unique identifier

        public string Name { get; set; }
        public string Family { get; set; }
        [Column("with_flower")]
        public bool WithFlower { get; set; }
        public string Description { get; set; }
    }
}
