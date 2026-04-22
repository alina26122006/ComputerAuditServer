using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace ComputerAuditServer.Models
{
    public enum PCStatus
    {
        Active,
        Inactive,
        Maintenance,
        Decommissioned
    }

    [Table("pc")]
    public class PC
    {
        [Key]
        [Column("id_pc")]
        public int IdPc { get; set; }

        [Column("inventory_number")]
        [MaxLength(100)]
        public string InventoryNumber { get; set; } = "";

        [Column("computer_name")]
        [MaxLength(100)]
        public string ComputerName { get; set; } = "";

        [Column("manufacturer")]
        [MaxLength(200)]
        public string? Manufacturer { get; set; }

        [Column("model")]
        [MaxLength(200)]
        public string? Model { get; set; }

        [Column("processor")]
        [MaxLength(200)]
        public string? Processor { get; set; }

        [Column("ram_gb")]
        public int? RamGb { get; set; }

        [Column("storage_gb")]
        public int? StorageGb { get; set; }

        [Column("os_version")]
        [MaxLength(100)]
        public string? OsVersion { get; set; }

        [Column("ip_address")]
        [MaxLength(45)]
        public string? IpAddress { get; set; }

        [Column("mac_address")]
        [MaxLength(17)]
        public string? MacAddress { get; set; }

        [Column("last_seen")]
        public DateTime LastSeen { get; set; }

        [Column("status")]
        public PCStatus Status { get; set; }

        [Column("user_id")]  
        public int? UserId { get; set; }

        [Column("office_id")] 
        public int? OfficeId { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        [ForeignKey("OfficeId")]
        public virtual Office? Office { get; set; }

        public virtual ICollection<EventLog>? EventLogs { get; set; }
        public virtual ICollection<PCReport>? PCReports { get; set; }
    }
}