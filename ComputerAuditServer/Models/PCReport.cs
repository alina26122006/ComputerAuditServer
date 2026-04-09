using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerAuditServer.Models
{
    [Table("pc_report")]
    public class PCReport
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("pc_id")]
        public int PcId { get; set; }

        [Column("raw_data")]
        public string RawData { get; set; } // JSON string

        [Column("reported_at")]
        public DateTime ReportedAt { get; set; }

        [Column("report_status")]
        [MaxLength(50)]
        public string ReportStatus { get; set; }

        // Foreign keys
        [ForeignKey("PcId")]
        public virtual PC PC { get; set; }
    }
}