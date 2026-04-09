using System.ComponentModel.DataAnnotations;

namespace ComputerAuditServer.Models
{
    public class SystemIdentifiers
    {
        [Key]
        public int Id { get; set; }
        public int AuditReportId { get; set; }
        public virtual AuditReport AuditReport { get; set; }

        public string ComputerName { get; set; }
        public string SystemUUID { get; set; }
        public string BIOSSerial { get; set; }
    }
}
