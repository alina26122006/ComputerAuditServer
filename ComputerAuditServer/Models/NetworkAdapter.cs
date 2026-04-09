using System.ComponentModel.DataAnnotations;

namespace ComputerAuditServer.Models
{
    public class NetworkAdapter
    {
        [Key]
        public int Id { get; set; }
        public int AuditReportId { get; set; }
        public virtual AuditReport AuditReport { get; set; }

        public string Name { get; set; }
        public string MACAddress { get; set; }
        public string IPv4Address { get; set; }
        public string Status { get; set; }
    }
}
