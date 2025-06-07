using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class DiliveryPoint
    {
        [Key] public int Id { get; set; }
        public string Description { get; set; } = "Нет данных о центре";
    }
}
