using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Models
{
    public class Record
    {
        [Key] public int Id { get; set; }
        public int DonorId { get; set; }
        [ForeignKey("DonorId")] public Donor? Donor { get; set; }
        public int DiliveryPointId { get; set; }
        [ForeignKey("DiliveryPointId")] public DiliveryPoint? DiliveryPoint { get; set; }
        public DateTime DateOnly { get; set; }
    }
}
