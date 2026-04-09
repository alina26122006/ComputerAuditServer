using System.ComponentModel.DataAnnotations;

namespace ComputerAuditServer.Models
{
    public class MonitorInfo
    {
        [Key]
        public int Id { get; set; }
        public int AuditReportId { get; set; }
        public virtual AuditReport AuditReport { get; set; }

        public int Number { get; set; }
        public string ManufacturerCode { get; set; }
        public string ModelName { get; set; }
        public string SerialNumber { get; set; }
        public string InstanceName { get; set; }
        public int? HorizontalSizeCm { get; set; }
        public int? VerticalSizeCm { get; set; }
        public double? DiagonalInches { get; set; }
        public string ConnectionPort { get; set; }
        public int? WeekOfManufacture { get; set; }
        public int? YearOfManufacture { get; set; }
        public string EDIDHex { get; set; }
    }
}
