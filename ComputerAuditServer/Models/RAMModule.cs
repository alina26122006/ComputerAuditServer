using System.ComponentModel.DataAnnotations;

namespace ComputerAuditServer.Models
{
    public class RAMModule
    {
        [Key]
        public int Id { get; set; }
        public int RAMInfoId { get; set; }
        public virtual RAMInfo RAMInfo { get; set; }

        public double CapacityGB { get; set; }
        public string SpeedMHz { get; set; }
        public string PartNumber { get; set; }
    }
}
