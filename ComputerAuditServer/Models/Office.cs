using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerAuditServer.Models
{
    [Table("office")]
    public class Office
    {
        [Key]
        [Column("office_id")]
        public int OfficeId { get; set; }

        [Column("number")]
        [MaxLength(50)]
        public string Number { get; set; }

        [Column("address")]
        [MaxLength(500)]
        public string Address { get; set; }

        [Column("floor")]
        [MaxLength(50)]
        public string Floor { get; set; }

        // Navigation properties
        public virtual ICollection<PC> PCs { get; set; }
    }
}