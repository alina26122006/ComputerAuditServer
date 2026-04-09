using System.ComponentModel.DataAnnotations;

namespace ComputerAuditServer.Models
{
    public class PrinterInfo
    {
        [Key]
        public int Id { get; set; }
        public int AuditReportId { get; set; }
        public virtual AuditReport AuditReport { get; set; }

        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime? LastPrintTime { get; set; }
        public string PortName { get; set; }
        public string ConnectionType { get; set; }
        public string IPAddress { get; set; }
        public string MACAddress { get; set; }
        public string LinkStatus { get; set; }
    }
}
