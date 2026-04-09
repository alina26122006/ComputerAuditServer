using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;
using ComputerAuditServer.Data;
using ComputerAuditServer.Models;
using ComputerAuditServer.Services;
using Microsoft.EntityFrameworkCore;
using ComputerAuditServer.Data;
using ComputerAuditServer.Services;

namespace ComputerAuditServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComputerAuditController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ComparisonService _comparisonService;
        private readonly ILogger<ComputerAuditController> _logger;

        public ComputerAuditController(
            ApplicationDbContext context,
            ComparisonService comparisonService,
            ILogger<ComputerAuditController> logger)
        {
            _context = context;
            _comparisonService = comparisonService;
            _logger = logger;
        }

        [HttpPost("audit")]
        public async Task<IActionResult> ReceiveAudit([FromBody] JsonElement auditData)
        {
            try
            {
                _logger.LogInformation($"Получены данные от компьютера");

                // Десериализация полученных данных
                var report = JsonSerializer.Deserialize<AuditReport>(auditData.GetRawText(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (report == null)
                {
                    return BadRequest("Неверный формат данных");
                }

                // Сохраняем полный JSON
                report.FullReportJson = auditData.GetRawText();
                report.ScanTime = DateTime.Now;

                // Ищем последний отчет для этого компьютера
                var lastReport = await _context.AuditReport
                    .Include(r => r.RAM)
                        .ThenInclude(r => r.Modules)
                    .Include(r => r.CPU)
                    .Include(r => r.Motherboard)
                    .Include(r => r.PhysicalDisks)
                    .Include(r => r.NetworkAdapters)
                    .Include(r => r.Monitors)
                    .Include(r => r.GPUs)
                    .Include(r => r.Printers)
                    .Where(r => r.ComputerName == report.ComputerName)
                    .OrderByDescending(r => r.ScanTime)
                    .FirstOrDefaultAsync();

                // Сравниваем с предыдущим отчетом
                var comparison = _comparisonService.CompareReports(report, lastReport);
                report.HasChanges = comparison.HasChanges;
                report.PreviousReportId = comparison.PreviousReport?.Id;

                // Сохраняем новый отчет
                await SaveReportToDatabase(report, auditData);

                // Формируем ответ
                var response = new
                {
                    Status = "success",
                    Message = comparison.HasChanges ? "Обнаружены изменения в конфигурации" : "Изменений не обнаружено",
                    HasChanges = comparison.HasChanges,
                    Changes = comparison.Changes,
                    ReportId = report.Id,
                    PreviousReportId = report.PreviousReportId
                };

                _logger.LogInformation($"Отчет сохранен. Изменения: {comparison.HasChanges}");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обработке отчета");
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        private async Task SaveReportToDatabase(AuditReport report, JsonElement auditData)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Сохраняем основной отчет
                _context.AuditReport.Add(report);
                await _context.SaveChangesAsync();

                // Сохраняем связанные данные, используя десериализацию
                // Для упрощения, можно сохранить через JSON и потом обработать
                // Здесь мы используем тот же JSON для создания связанных записей

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        [HttpGet("history/{computerName}")]
        public async Task<IActionResult> GetHistory(string computerName, [FromQuery] int limit = 10)
        {
            try
            {
                var reports = await _context.AuditReport
                    .Where(r => r.ComputerName == computerName)
                    .OrderByDescending(r => r.ScanTime)
                    .Take(limit)
                    .Select(r => new
                    {
                        r.Id,
                        r.ScanTime,
                        r.HasChanges,
                        r.PreviousReportId
                    })
                    .ToListAsync();

                return Ok(reports);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении истории");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("report/{id}")]
        public async Task<IActionResult> GetReport(int id)
        {
            try
            {
                var report = await _context.AuditReport
                    .Include(r => r.SystemIds)
                    .Include(r => r.NetworkAdapters)
                    .Include(r => r.PhysicalDisks)
                    .Include(r => r.Motherboard)
                    .Include(r => r.CPU)
                    .Include(r => r.RAM)
                        .ThenInclude(r => r.Modules)
                    .Include(r => r.GPUs)
                    .Include(r => r.Monitors)
                    .Include(r => r.Printers)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (report == null)
                    return NotFound();

                return Ok(report);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении отчета");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("changes")]
        public async Task<IActionResult> GetChanges([FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            try
            {
                var query = _context.AuditReport
                    .Where(r => r.HasChanges);

                if (from.HasValue)
                    query = query.Where(r => r.ScanTime >= from.Value);

                if (to.HasValue)
                    query = query.Where(r => r.ScanTime <= to.Value);

                var changes = await query
                    .OrderByDescending(r => r.ScanTime)
                    .Select(r => new
                    {
                        r.Id,
                        r.ComputerName,
                        r.ScanTime,
                        r.DomainUser
                    })
                    .ToListAsync();

                return Ok(changes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении изменений");
                return StatusCode(500, ex.Message);
            }
        }
    }
}