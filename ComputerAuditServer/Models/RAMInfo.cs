using System.ComponentModel.DataAnnotations;

namespace ComputerAuditServer.Models
{
    public class RAMInfo
    {
        [Key]
        public int Id { get; set; }
        public int AuditReportId { get; set; }
        public virtual AuditReport AuditReport { get; set; }

        public int TotalSlots { get; set; }
        public int UsedSlots { get; set; }
        public double TotalRAMGB { get; set; }

        public virtual ICollection<RAMModule> Modules { get; set; }
    }
}
