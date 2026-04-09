using System.ComponentModel.DataAnnotations;

namespace ComputerAuditServer.Models
{
    public class AuditReport
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ComputerName { get; set; }

        [Required]
        public DateTime ScanTime { get; set; }

        public string DomainUser { get; set; }
        public string WindowsVersion { get; set; }

        // Связанные данные
        public virtual SystemIdentifiers SystemIds { get; set; }
        public virtual ICollection<NetworkAdapter> NetworkAdapters { get; set; }
        public virtual ICollection<PhysicalDisk> PhysicalDisks { get; set; }
        public virtual Motherboard Motherboard { get; set; }
        public virtual CPUInfo CPU { get; set; }
        public virtual RAMInfo RAM { get; set; }
        public virtual ICollection<GPUInfo> GPUs { get; set; }
        public virtual ICollection<MonitorInfo> Monitors { get; set; }
        public virtual ICollection<PrinterInfo> Printers { get; set; }

        // Флаг, указывающий были ли изменения
        public bool HasChanges { get; set; }

        // Ссылка на предыдущую версию
        public int? PreviousReportId { get; set; }
        public virtual AuditReport PreviousReport { get; set; }

        // JSON для хранения полного отчета
        public string FullReportJson { get; set; }
    }
}
