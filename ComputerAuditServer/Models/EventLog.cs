using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerAuditServer.Models
{
    public enum EventType
    {
        HardwareChange,
        SoftwareChange,
        NetworkChange,
        StatusChange,
        UserChange,
        OfficeChange
    }

    [Table("event_log")]
    public class EventLog
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("pc_id")]
        public int PcId { get; set; }

        [Column("event_type")]
        public EventType EventType { get; set; }

        [Column("payload")]
        public string Payload { get; set; } // JSON string

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("is_synced")]
        public bool IsSynced { get; set; }

        // Foreign keys
        [ForeignKey("PcId")]
        public virtual PC PC { get; set; }
    }
}