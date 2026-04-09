using ComputerAuditServer.Models;

namespace ComputerAuditServer.Services
{
    public class ComparisonResult
    {
        public bool HasChanges { get; set; }
        public List<string> Changes { get; set; } = new List<string>();
        public AuditReport PreviousReport { get; set; }
    }
}
