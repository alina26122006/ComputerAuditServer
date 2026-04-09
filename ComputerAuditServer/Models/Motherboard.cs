using System.ComponentModel.DataAnnotations;

namespace ComputerAuditServer.Models
{
    public class Motherboard
    {
        [Key]
        public int Id { get; set; }
        public int AuditReportId { get; set; }
        public virtual AuditReport AuditReport { get; set; }

        public string Manufacturer { get; set; }
        public string Product { get; set; }
        public string Serial { get; set; }
    }
}
