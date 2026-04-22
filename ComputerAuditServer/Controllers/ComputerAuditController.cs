using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;
using ComputerAuditServer.Data;
using ComputerAuditServer.Models;

namespace ComputerAuditServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComputerAuditController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ComputerAuditController> _logger;

        public ComputerAuditController(
            ApplicationDbContext context,
            ILogger<ComputerAuditController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("audit")]
        public async Task<IActionResult> ReceiveAudit([FromBody] object auditData)
        {
            try
            {
                _logger.LogInformation("Получены данные от компьютера");

                string jsonString = auditData.ToString();

                // Детальное логирование
                _logger.LogInformation($"Размер JSON: {jsonString.Length} символов");

                var pcReport = new PCReport
                {
                    RawData = jsonString,
                    ReportedAt = DateTime.Now,
                    ReportStatus = "received"
                };

                using (JsonDocument doc = JsonDocument.Parse(jsonString))
                {
                    JsonElement root = doc.RootElement;

                    // Проверяем наличие ComputerName
                    if (!root.TryGetProperty("ComputerName", out JsonElement computerNameElement))
                    {
                        return BadRequest(new { Status = "error", Message = "Отсутствует ComputerName в JSON" });
                    }

                    string computerName = computerNameElement.GetString();
                    _logger.LogInformation($"ComputerName: {computerName}");

                    // Ищем компьютер в БД
                    var pc = await _context.PCs
                        .FirstOrDefaultAsync(p => p.ComputerName == computerName);

                    if (pc == null)
                    {
                        _logger.LogInformation($"Создаем новый компьютер: {computerName}");
                        pc = new PC
                        {
                            ComputerName = computerName,
                            LastSeen = DateTime.Now,
                            Status = PCStatus.Active,
                            InventoryNumber = Guid.NewGuid().ToString().Substring(0, 8)
                        };
                        _context.PCs.Add(pc);

                        try
                        {
                            await _context.SaveChangesAsync();
                            _logger.LogInformation($"Компьютер создан с ID: {pc.IdPc}");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Ошибка при создании компьютера");
                            throw;
                        }
                    }
                    else
                    {
                        _logger.LogInformation($"Найден существующий компьютер ID: {pc.IdPc}");
                        pc.LastSeen = DateTime.Now;
                    }

                    pcReport.PcId = pc.IdPc;

                    // Обновляем информацию о ПК из отчета
                    try
                    {
                        await UpdatePCFromReport(pc, root);
                        await _context.SaveChangesAsync();
                        _logger.LogInformation("Информация о ПК обновлена");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Ошибка при обновлении информации о ПК");
                        throw;
                    }
                }

                _context.PCReports.Add(pcReport);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Status = "success",
                    Message = "Данные успешно сохранены",
                    PcId = pcReport.PcId
                });
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка базы данных");
                var innerException = ex.InnerException;
                string errorMessage = ex.Message;

                while (innerException != null)
                {
                    errorMessage += $" -> {innerException.Message}";
                    innerException = innerException.InnerException;
                }

                return StatusCode(500, new { Status = "error", Message = errorMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Общая ошибка при обработке отчета");
                return StatusCode(500, new { Status = "error", Message = ex.Message });
            }
        }

        // ДОБАВЬТЕ ЭТОТ МЕТОД - он обновляет информацию о ПК из отчета
        // ДОБАВЬТЕ ЭТОТ МЕТОД - он обновляет информацию о ПК из отчета
        private async Task UpdatePCFromReport(PC pc, JsonElement reportData)
        {
            try
            {
                // Для строковых полей - проверяем на null
                if (string.IsNullOrEmpty(pc.Manufacturer))
                    pc.Manufacturer = "Unknown";

                if (string.IsNullOrEmpty(pc.Model))
                    pc.Model = "Unknown";

                if (string.IsNullOrEmpty(pc.Processor))
                    pc.Processor = "Unknown";

                if (string.IsNullOrEmpty(pc.OsVersion))
                    pc.OsVersion = "Unknown";

                if (string.IsNullOrEmpty(pc.IpAddress))
                    pc.IpAddress = "0.0.0.0";

                if (string.IsNullOrEmpty(pc.MacAddress))
                    pc.MacAddress = "00-00-00-00-00-00";

                // Для числовых полей - проверяем на null (если поле nullable)
                // Если в модели PC поле RamGb имеет тип int? (nullable)
                if (!pc.RamGb.HasValue)
                    pc.RamGb = 0;

                if (!pc.StorageGb.HasValue)
                    pc.StorageGb = 0;

                // Получаем данные из JSON
                if (reportData.TryGetProperty("CPU", out JsonElement cpu))
                {
                    if (cpu.TryGetProperty("Name", out JsonElement cpuName))
                        pc.Processor = cpuName.GetString();
                }

                if (reportData.TryGetProperty("RAM", out JsonElement ram))
                {
                    if (ram.TryGetProperty("TotalRAMGB", out JsonElement ramGb))
                        pc.RamGb = (int)ramGb.GetDouble();
                }

                if (reportData.TryGetProperty("Motherboard", out JsonElement motherboard))
                {
                    if (motherboard.TryGetProperty("Manufacturer", out JsonElement manufacturer))
                        pc.Manufacturer = manufacturer.GetString();

                    if (motherboard.TryGetProperty("Product", out JsonElement product))
                        pc.Model = product.GetString();
                }

                if (reportData.TryGetProperty("WindowsVersion", out JsonElement osVersion))
                {
                    pc.OsVersion = osVersion.GetString();
                }

                if (reportData.TryGetProperty("PhysicalDisks", out JsonElement disks))
                {
                    int totalStorage = 0;
                    foreach (var disk in disks.EnumerateArray())
                    {
                        if (disk.TryGetProperty("SizeGB", out JsonElement size))
                        {
                            totalStorage += (int)size.GetDouble();
                        }
                    }
                    pc.StorageGb = totalStorage;
                }

                if (reportData.TryGetProperty("NetworkAdapters", out JsonElement adapters))
                {
                    foreach (var adapter in adapters.EnumerateArray())
                    {
                        if (adapter.TryGetProperty("Status", out JsonElement status) &&
                            status.GetString() == "Up")
                        {
                            if (adapter.TryGetProperty("IPv4Address", out JsonElement ip))
                                pc.IpAddress = ip.GetString();

                            if (adapter.TryGetProperty("MACAddress", out JsonElement mac))
                                pc.MacAddress = mac.GetString();
                            break;
                        }
                    }
                }

                _logger.LogInformation($"Обновлен ПК: {pc.ComputerName}, RAM: {pc.RamGb}GB, Storage: {pc.StorageGb}GB");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка в UpdatePCFromReport");
                throw;
            }
        }
        // GET: api/ComputerAudit/history/{computerName}
        [HttpGet("history/{computerName}")]
        public async Task<IActionResult> GetHistory(string computerName, [FromQuery] int limit = 10)
        {
            try
            {
                // Сначала найдем ПК по имени
                var pc = await _context.PCs
                    .FirstOrDefaultAsync(p => p.ComputerName == computerName);

                if (pc == null)
                {
                    return NotFound(new { Status = "error", Message = $"Компьютер {computerName} не найден" });
                }

                // Получаем отчеты для этого ПК
                var reports = await _context.PCReports
                    .Where(r => r.PcId == pc.IdPc)
                    .OrderByDescending(r => r.ReportedAt)
                    .Take(limit)
                    .Select(r => new
                    {
                        r.Id,
                        r.ReportedAt,
                        r.ReportStatus
                    })
                    .ToListAsync();

                return Ok(reports);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении истории");
                return StatusCode(500, new { Status = "error", Message = ex.Message });
            }
        }

        // GET: api/ComputerAudit/report/{id}
        [HttpGet("report/{id}")]
        public async Task<IActionResult> GetReport(int id)
        {
            try
            {
                var report = await _context.PCReports
                    .Include(r => r.PC)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (report == null)
                    return NotFound(new { Status = "error", Message = $"Отчет {id} не найден" });

                return Ok(new
                {
                    report.Id,
                    report.ReportedAt,
                    report.ReportStatus,
                    PC = new
                    {
                        report.PC.ComputerName,
                        report.PC.Manufacturer,
                        report.PC.Model,
                        report.PC.Processor,
                        report.PC.RamGb,
                        report.PC.StorageGb,
                        report.PC.OsVersion,
                        report.PC.IpAddress,
                        report.PC.MacAddress,
                        report.PC.Status
                    },
                    RawData = JsonDocument.Parse(report.RawData).RootElement
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении отчета");
                return StatusCode(500, new { Status = "error", Message = ex.Message });
            }
        }

        // GET: api/ComputerAudit/changes
        [HttpGet("changes")]
        public async Task<IActionResult> GetChanges([FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            try
            {
                var query = _context.PCReports.AsQueryable();

                if (from.HasValue)
                    query = query.Where(r => r.ReportedAt >= from.Value);

                if (to.HasValue)
                    query = query.Where(r => r.ReportedAt <= to.Value);

                var changes = await query
                    .Include(r => r.PC)
                    .OrderByDescending(r => r.ReportedAt)
                    .Select(r => new
                    {
                        r.Id,
                        ComputerName = r.PC.ComputerName,
                        r.ReportedAt,
                        r.ReportStatus
                    })
                    .ToListAsync();

                return Ok(changes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении изменений");
                return StatusCode(500, new { Status = "error", Message = ex.Message });
            }
        }
    }
}