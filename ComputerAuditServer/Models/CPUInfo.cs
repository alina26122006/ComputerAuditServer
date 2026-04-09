using System.ComponentModel.DataAnnotations;

namespace ComputerAuditServer.Models
{
    public class CPUInfo
    {
        [Key]
        public int Id { get; set; }
        public int AuditReportId { get; set; }
        public virtual AuditReport AuditReport { get; set; }

        public string Name { get; set; }
        public string ProcessorId { get; set; }
        public int Cores { get; set; }
        public int Threads { get; set; }
    }
}
