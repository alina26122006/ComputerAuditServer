using System.ComponentModel.DataAnnotations;

namespace ComputerAuditServer.Models
{
    public class PhysicalDisk
    {
        [Key]
        public int Id { get; set; }
        public int AuditReportId { get; set; }
        public virtual AuditReport AuditReport { get; set; }

        public string Model { get; set; }
        public double SizeGB { get; set; }
        public string SerialNumber { get; set; }
    }
}
