using System.ComponentModel.DataAnnotations;

namespace ComputerAuditServer.Models
{
    public class GPUInfo
    {
        [Key]
        public int Id { get; set; }
        public int AuditReportId { get; set; }
        public virtual AuditReport AuditReport { get; set; }

        public string Name { get; set; }
        public double AdapterRAMGB { get; set; }
        public string DriverVersion { get; set; }
        public string DriverDate { get; set; }
        public string Resolution { get; set; }
        public string RefreshRate { get; set; }
        public string VideoMode { get; set; }
        public string PNDeviceID { get; set; }
    }
}
