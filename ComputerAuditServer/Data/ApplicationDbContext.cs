using Microsoft.EntityFrameworkCore;
using ComputerAuditServer.Models;

namespace ComputerAuditServer.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // СТАРЫЕ МОДЕЛИ (из первой версии)
        public DbSet<AuditReport> AuditReport { get; set; }
        public DbSet<CPUInfo> CPUInfos { get; set; }
        public DbSet<GPUInfo> GPUInfos { get; set; }
        public DbSet<MonitorInfo> MonitorInfos { get; set; }
        public DbSet<PrinterInfo> PrinterInfos { get; set; }
        public DbSet<NetworkAdapter> NetworkAdapterInfos { get; set; }
        public DbSet<PhysicalDisk> PhysicalDiskInfos { get; set; }
        public DbSet<Motherboard> MotherboardInfos { get; set; }
        public DbSet<RAMInfo> RAMInfos { get; set; }
        public DbSet<RAMModule> RAMModules { get; set; }
        public DbSet<SystemIdentifiers> SystemIdentifiers { get; set; }

        // НОВЫЕ МОДЕЛИ (из спроектированной БД)
        public DbSet<User> Users { get; set; }
        public DbSet<Office> Offices { get; set; }
        public DbSet<PC> PCs { get; set; }
        public DbSet<EventLog> EventLogs { get; set; }
        public DbSet<PCReport> PCReports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===== НАСТРОЙКА СТАРЫХ МОДЕЛЕЙ =====

            // Настройка таблиц для старых моделей (если они нужны)
            modelBuilder.Entity<AuditReport>().ToTable("audit_reports");
            modelBuilder.Entity<CPUInfo>().ToTable("cpu_infos");
            modelBuilder.Entity<GPUInfo>().ToTable("gpu_infos");
            modelBuilder.Entity<MonitorInfo>().ToTable("monitor_infos");
            modelBuilder.Entity<PrinterInfo>().ToTable("printer_infos");
            modelBuilder.Entity<NetworkAdapter>().ToTable("network_adapters");
            modelBuilder.Entity<PhysicalDisk>().ToTable("physical_disks");
            modelBuilder.Entity<Motherboard>().ToTable("motherboards");
            modelBuilder.Entity<RAMInfo>().ToTable("ram_infos");
            modelBuilder.Entity<RAMModule>().ToTable("ram_modules");
            modelBuilder.Entity<SystemIdentifiers>().ToTable("system_identifiers");

            // Связи для старых моделей
            modelBuilder.Entity<AuditReport>()
                .HasOne(a => a.SystemIds)
                .WithOne()
                .HasForeignKey<AuditReport>(a => a.Id);

            modelBuilder.Entity<AuditReport>()
                .HasMany(a => a.NetworkAdapters)
                .WithOne()
                .HasForeignKey("AuditReportId");

            modelBuilder.Entity<AuditReport>()
                .HasMany(a => a.PhysicalDisks)
                .WithOne()
                .HasForeignKey("AuditReportId");

            modelBuilder.Entity<AuditReport>()
                .HasOne(a => a.Motherboard)
                .WithOne()
                .HasForeignKey<AuditReport>(a => a.Id);

            modelBuilder.Entity<AuditReport>()
                .HasOne(a => a.CPU)
                .WithOne()
                .HasForeignKey<AuditReport>(a => a.Id);

            modelBuilder.Entity<AuditReport>()
                .HasOne(a => a.RAM)
                .WithOne()
                .HasForeignKey<AuditReport>(a => a.Id);

            modelBuilder.Entity<AuditReport>()
                .HasMany(a => a.GPUs)
                .WithOne()
                .HasForeignKey("AuditReportId");

            modelBuilder.Entity<AuditReport>()
                .HasMany(a => a.Monitors)
                .WithOne()
                .HasForeignKey("AuditReportId");

            modelBuilder.Entity<AuditReport>()
                .HasMany(a => a.Printers)
                .WithOne()
                .HasForeignKey("AuditReportId");

            modelBuilder.Entity<RAMInfo>()
                .HasMany(r => r.Modules)
                .WithOne()
                .HasForeignKey("RAMInfoId");

            // ===== НАСТРОЙКА НОВЫХ МОДЕЛЕЙ =====

            // Уникальный индекс для inventory_number
            modelBuilder.Entity<PC>()
                .HasIndex(p => p.InventoryNumber)
                .IsUnique();

            // Индексы для оптимизации запросов
            modelBuilder.Entity<PC>()
                .HasIndex(p => p.ComputerName);

            modelBuilder.Entity<PC>()
                .HasIndex(p => p.LastSeen);

            modelBuilder.Entity<PC>()
                .HasIndex(p => p.Status);

            modelBuilder.Entity<EventLog>()
                .HasIndex(e => e.CreatedAt);

            modelBuilder.Entity<EventLog>()
                .HasIndex(e => e.IsSynced);

            modelBuilder.Entity<PCReport>()
                .HasIndex(r => r.ReportedAt);

            // Настройка JSON полей для PostgreSQL
            modelBuilder.Entity<EventLog>()
                .Property(e => e.Payload)
                .HasColumnType("jsonb");

            modelBuilder.Entity<PCReport>()
                .Property(r => r.RawData)
                .HasColumnType("jsonb");

            // Настройка enum типов
            modelBuilder.Entity<PC>()
                .Property(p => p.Status)
                .HasConversion<string>();

            modelBuilder.Entity<EventLog>()
                .Property(e => e.EventType)
                .HasConversion<string>();
        }
    }
}