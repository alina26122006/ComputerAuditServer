using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerAuditServer.Models
{
    [Table("user")]
    public class User
    {
        [Key]
        [Column("id_user")]
        public int IdUser { get; set; }

        [Column("login")]
        [MaxLength(100)]
        public string Login { get; set; }

        [Column("name")]
        [MaxLength(200)]
        public string Name { get; set; }

        [Column("email")]
        [MaxLength(200)]
        public string Email { get; set; }

        // Navigation properties
        public virtual ICollection<PC> PCs { get; set; }
    }
}