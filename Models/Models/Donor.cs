using Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class Donor
    {
        [Key] public int Id { get; set; }
        public string FullName { get; set; } = "Нет данных";
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public BloodTypeEnum BloodTypeEnum { get; set; }
    }
}
