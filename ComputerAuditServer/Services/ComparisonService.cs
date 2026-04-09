using ComputerAuditServer.Models;

namespace ComputerAuditServer.Services
{
    public class ComparisonService
    {
        public ComparisonResult CompareReports(AuditReport newReport, AuditReport oldReport)
        {
            var result = new ComparisonResult();

            if (oldReport == null)
            {
                result.HasChanges = true;
                result.Changes.Add("Первый отчет для этого компьютера");
                return result;
            }

            result.PreviousReport = oldReport;

            // Сравнение RAM
            if (Math.Abs(newReport.RAM.TotalRAMGB - oldReport.RAM.TotalRAMGB) > 0.1)
            {
                result.HasChanges = true;
                result.Changes.Add($"RAM изменена: {oldReport.RAM.TotalRAMGB}GB -> {newReport.RAM.TotalRAMGB}GB");
            }

            // Сравнение CPU
            if (newReport.CPU.Name != oldReport.CPU.Name)
            {
                result.HasChanges = true;
                result.Changes.Add($"CPU изменен: {oldReport.CPU.Name} -> {newReport.CPU.Name}");
            }

            // Сравнение дисков
            var newDisks = newReport.PhysicalDisks.Select(d => d.SerialNumber).OrderBy(s => s).ToList();
            var oldDisks = oldReport.PhysicalDisks.Select(d => d.SerialNumber).OrderBy(s => s).ToList();

            if (!newDisks.SequenceEqual(oldDisks))
            {
                result.HasChanges = true;
                result.Changes.Add("Изменен состав дисков");
            }

            // Сравнение мониторов
            if (newReport.Monitors.Count != oldReport.Monitors.Count)
            {
                result.HasChanges = true;
                result.Changes.Add($"Количество мониторов изменено: {oldReport.Monitors.Count} -> {newReport.Monitors.Count}");
            }

            // Сравнение сетевых адаптеров
            var newMacs = newReport.NetworkAdapters.Select(a => a.MACAddress).OrderBy(m => m).ToList();
            var oldMacs = oldReport.NetworkAdapters.Select(a => a.MACAddress).OrderBy(m => m).ToList();

            if (!newMacs.SequenceEqual(oldMacs))
            {
                result.HasChanges = true;
                result.Changes.Add("Изменен состав сетевых адаптеров");
            }

            // Сравнение материнской платы
            if (newReport.Motherboard.Serial != oldReport.Motherboard.Serial)
            {
                result.HasChanges = true;
                result.Changes.Add($"Материнская плата изменена: {oldReport.Motherboard.Product} -> {newReport.Motherboard.Product}");
            }

            return result;
        }
    }
}
