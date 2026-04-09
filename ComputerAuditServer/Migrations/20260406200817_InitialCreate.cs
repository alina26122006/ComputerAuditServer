using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ComputerAuditServer.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "office",
                columns: table => new
                {
                    office_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    floor = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_office", x => x.office_id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id_user = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.id_user);
                });

            migrationBuilder.CreateTable(
                name: "pc",
                columns: table => new
                {
                    id_pc = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    inventory_number = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: true),
                    office_id = table.Column<int>(type: "integer", nullable: true),
                    computer_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    manufacturer = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    model = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    processor = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ram_gb = table.Column<int>(type: "integer", nullable: false),
                    storage_gb = table.Column<int>(type: "integer", nullable: false),
                    os_version = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ip_address = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    mac_address = table.Column<string>(type: "character varying(17)", maxLength: 17, nullable: false),
                    last_seen = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pc", x => x.id_pc);
                    table.ForeignKey(
                        name: "FK_pc_office_office_id",
                        column: x => x.office_id,
                        principalTable: "office",
                        principalColumn: "office_id");
                    table.ForeignKey(
                        name: "FK_pc_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id_user");
                });

            migrationBuilder.CreateTable(
                name: "event_log",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pc_id = table.Column<int>(type: "integer", nullable: false),
                    event_type = table.Column<string>(type: "text", nullable: false),
                    payload = table.Column<string>(type: "jsonb", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_synced = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_event_log", x => x.id);
                    table.ForeignKey(
                        name: "FK_event_log_pc_pc_id",
                        column: x => x.pc_id,
                        principalTable: "pc",
                        principalColumn: "id_pc",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pc_report",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pc_id = table.Column<int>(type: "integer", nullable: false),
                    raw_data = table.Column<string>(type: "jsonb", nullable: false),
                    reported_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    report_status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pc_report", x => x.id);
                    table.ForeignKey(
                        name: "FK_pc_report_pc_pc_id",
                        column: x => x.pc_id,
                        principalTable: "pc",
                        principalColumn: "id_pc",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "audit_reports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    ComputerName = table.Column<string>(type: "text", nullable: false),
                    ScanTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DomainUser = table.Column<string>(type: "text", nullable: false),
                    WindowsVersion = table.Column<string>(type: "text", nullable: false),
                    HasChanges = table.Column<bool>(type: "boolean", nullable: false),
                    PreviousReportId = table.Column<int>(type: "integer", nullable: true),
                    FullReportJson = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_audit_reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_audit_reports_audit_reports_PreviousReportId",
                        column: x => x.PreviousReportId,
                        principalTable: "audit_reports",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "cpu_infos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuditReportId = table.Column<int>(type: "integer", nullable: false),
                    AuditReportId1 = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ProcessorId = table.Column<string>(type: "text", nullable: false),
                    Cores = table.Column<int>(type: "integer", nullable: false),
                    Threads = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cpu_infos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cpu_infos_audit_reports_AuditReportId1",
                        column: x => x.AuditReportId1,
                        principalTable: "audit_reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "gpu_infos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuditReportId = table.Column<int>(type: "integer", nullable: false),
                    AuditReportId1 = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    AdapterRAMGB = table.Column<double>(type: "double precision", nullable: false),
                    DriverVersion = table.Column<string>(type: "text", nullable: false),
                    DriverDate = table.Column<string>(type: "text", nullable: false),
                    Resolution = table.Column<string>(type: "text", nullable: false),
                    RefreshRate = table.Column<string>(type: "text", nullable: false),
                    VideoMode = table.Column<string>(type: "text", nullable: false),
                    PNDeviceID = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gpu_infos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_gpu_infos_audit_reports_AuditReportId",
                        column: x => x.AuditReportId,
                        principalTable: "audit_reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_gpu_infos_audit_reports_AuditReportId1",
                        column: x => x.AuditReportId1,
                        principalTable: "audit_reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "monitor_infos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuditReportId = table.Column<int>(type: "integer", nullable: false),
                    AuditReportId1 = table.Column<int>(type: "integer", nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    ManufacturerCode = table.Column<string>(type: "text", nullable: false),
                    ModelName = table.Column<string>(type: "text", nullable: false),
                    SerialNumber = table.Column<string>(type: "text", nullable: false),
                    InstanceName = table.Column<string>(type: "text", nullable: false),
                    HorizontalSizeCm = table.Column<int>(type: "integer", nullable: true),
                    VerticalSizeCm = table.Column<int>(type: "integer", nullable: true),
                    DiagonalInches = table.Column<double>(type: "double precision", nullable: true),
                    ConnectionPort = table.Column<string>(type: "text", nullable: false),
                    WeekOfManufacture = table.Column<int>(type: "integer", nullable: true),
                    YearOfManufacture = table.Column<int>(type: "integer", nullable: true),
                    EDIDHex = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_monitor_infos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_monitor_infos_audit_reports_AuditReportId",
                        column: x => x.AuditReportId,
                        principalTable: "audit_reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_monitor_infos_audit_reports_AuditReportId1",
                        column: x => x.AuditReportId1,
                        principalTable: "audit_reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "motherboards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuditReportId = table.Column<int>(type: "integer", nullable: false),
                    AuditReportId1 = table.Column<int>(type: "integer", nullable: false),
                    Manufacturer = table.Column<string>(type: "text", nullable: false),
                    Product = table.Column<string>(type: "text", nullable: false),
                    Serial = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_motherboards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_motherboards_audit_reports_AuditReportId1",
                        column: x => x.AuditReportId1,
                        principalTable: "audit_reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "network_adapters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuditReportId = table.Column<int>(type: "integer", nullable: false),
                    AuditReportId1 = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    MACAddress = table.Column<string>(type: "text", nullable: false),
                    IPv4Address = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_network_adapters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_network_adapters_audit_reports_AuditReportId",
                        column: x => x.AuditReportId,
                        principalTable: "audit_reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_network_adapters_audit_reports_AuditReportId1",
                        column: x => x.AuditReportId1,
                        principalTable: "audit_reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "physical_disks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuditReportId = table.Column<int>(type: "integer", nullable: false),
                    AuditReportId1 = table.Column<int>(type: "integer", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    SizeGB = table.Column<double>(type: "double precision", nullable: false),
                    SerialNumber = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_physical_disks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_physical_disks_audit_reports_AuditReportId",
                        column: x => x.AuditReportId,
                        principalTable: "audit_reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_physical_disks_audit_reports_AuditReportId1",
                        column: x => x.AuditReportId1,
                        principalTable: "audit_reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "printer_infos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuditReportId = table.Column<int>(type: "integer", nullable: false),
                    AuditReportId1 = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    LastPrintTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PortName = table.Column<string>(type: "text", nullable: false),
                    ConnectionType = table.Column<string>(type: "text", nullable: false),
                    IPAddress = table.Column<string>(type: "text", nullable: false),
                    MACAddress = table.Column<string>(type: "text", nullable: false),
                    LinkStatus = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_printer_infos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_printer_infos_audit_reports_AuditReportId",
                        column: x => x.AuditReportId,
                        principalTable: "audit_reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_printer_infos_audit_reports_AuditReportId1",
                        column: x => x.AuditReportId1,
                        principalTable: "audit_reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ram_infos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuditReportId = table.Column<int>(type: "integer", nullable: false),
                    AuditReportId1 = table.Column<int>(type: "integer", nullable: false),
                    TotalSlots = table.Column<int>(type: "integer", nullable: false),
                    UsedSlots = table.Column<int>(type: "integer", nullable: false),
                    TotalRAMGB = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ram_infos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ram_infos_audit_reports_AuditReportId1",
                        column: x => x.AuditReportId1,
                        principalTable: "audit_reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "system_identifiers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuditReportId = table.Column<int>(type: "integer", nullable: false),
                    AuditReportId1 = table.Column<int>(type: "integer", nullable: false),
                    ComputerName = table.Column<string>(type: "text", nullable: false),
                    SystemUUID = table.Column<string>(type: "text", nullable: false),
                    BIOSSerial = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_system_identifiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_system_identifiers_audit_reports_AuditReportId1",
                        column: x => x.AuditReportId1,
                        principalTable: "audit_reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ram_modules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RAMInfoId = table.Column<int>(type: "integer", nullable: false),
                    RAMInfoId1 = table.Column<int>(type: "integer", nullable: false),
                    CapacityGB = table.Column<double>(type: "double precision", nullable: false),
                    SpeedMHz = table.Column<string>(type: "text", nullable: false),
                    PartNumber = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ram_modules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ram_modules_ram_infos_RAMInfoId",
                        column: x => x.RAMInfoId,
                        principalTable: "ram_infos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ram_modules_ram_infos_RAMInfoId1",
                        column: x => x.RAMInfoId1,
                        principalTable: "ram_infos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_audit_reports_PreviousReportId",
                table: "audit_reports",
                column: "PreviousReportId");

            migrationBuilder.CreateIndex(
                name: "IX_cpu_infos_AuditReportId1",
                table: "cpu_infos",
                column: "AuditReportId1");

            migrationBuilder.CreateIndex(
                name: "IX_event_log_created_at",
                table: "event_log",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_event_log_is_synced",
                table: "event_log",
                column: "is_synced");

            migrationBuilder.CreateIndex(
                name: "IX_event_log_pc_id",
                table: "event_log",
                column: "pc_id");

            migrationBuilder.CreateIndex(
                name: "IX_gpu_infos_AuditReportId",
                table: "gpu_infos",
                column: "AuditReportId");

            migrationBuilder.CreateIndex(
                name: "IX_gpu_infos_AuditReportId1",
                table: "gpu_infos",
                column: "AuditReportId1");

            migrationBuilder.CreateIndex(
                name: "IX_monitor_infos_AuditReportId",
                table: "monitor_infos",
                column: "AuditReportId");

            migrationBuilder.CreateIndex(
                name: "IX_monitor_infos_AuditReportId1",
                table: "monitor_infos",
                column: "AuditReportId1");

            migrationBuilder.CreateIndex(
                name: "IX_motherboards_AuditReportId1",
                table: "motherboards",
                column: "AuditReportId1");

            migrationBuilder.CreateIndex(
                name: "IX_network_adapters_AuditReportId",
                table: "network_adapters",
                column: "AuditReportId");

            migrationBuilder.CreateIndex(
                name: "IX_network_adapters_AuditReportId1",
                table: "network_adapters",
                column: "AuditReportId1");

            migrationBuilder.CreateIndex(
                name: "IX_pc_computer_name",
                table: "pc",
                column: "computer_name");

            migrationBuilder.CreateIndex(
                name: "IX_pc_inventory_number",
                table: "pc",
                column: "inventory_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_pc_last_seen",
                table: "pc",
                column: "last_seen");

            migrationBuilder.CreateIndex(
                name: "IX_pc_office_id",
                table: "pc",
                column: "office_id");

            migrationBuilder.CreateIndex(
                name: "IX_pc_status",
                table: "pc",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_pc_user_id",
                table: "pc",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_pc_report_pc_id",
                table: "pc_report",
                column: "pc_id");

            migrationBuilder.CreateIndex(
                name: "IX_pc_report_reported_at",
                table: "pc_report",
                column: "reported_at");

            migrationBuilder.CreateIndex(
                name: "IX_physical_disks_AuditReportId",
                table: "physical_disks",
                column: "AuditReportId");

            migrationBuilder.CreateIndex(
                name: "IX_physical_disks_AuditReportId1",
                table: "physical_disks",
                column: "AuditReportId1");

            migrationBuilder.CreateIndex(
                name: "IX_printer_infos_AuditReportId",
                table: "printer_infos",
                column: "AuditReportId");

            migrationBuilder.CreateIndex(
                name: "IX_printer_infos_AuditReportId1",
                table: "printer_infos",
                column: "AuditReportId1");

            migrationBuilder.CreateIndex(
                name: "IX_ram_infos_AuditReportId1",
                table: "ram_infos",
                column: "AuditReportId1");

            migrationBuilder.CreateIndex(
                name: "IX_ram_modules_RAMInfoId",
                table: "ram_modules",
                column: "RAMInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ram_modules_RAMInfoId1",
                table: "ram_modules",
                column: "RAMInfoId1");

            migrationBuilder.CreateIndex(
                name: "IX_system_identifiers_AuditReportId1",
                table: "system_identifiers",
                column: "AuditReportId1");

            migrationBuilder.AddForeignKey(
                name: "FK_audit_reports_cpu_infos_Id",
                table: "audit_reports",
                column: "Id",
                principalTable: "cpu_infos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_audit_reports_motherboards_Id",
                table: "audit_reports",
                column: "Id",
                principalTable: "motherboards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_audit_reports_ram_infos_Id",
                table: "audit_reports",
                column: "Id",
                principalTable: "ram_infos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_audit_reports_system_identifiers_Id",
                table: "audit_reports",
                column: "Id",
                principalTable: "system_identifiers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_audit_reports_cpu_infos_Id",
                table: "audit_reports");

            migrationBuilder.DropForeignKey(
                name: "FK_audit_reports_motherboards_Id",
                table: "audit_reports");

            migrationBuilder.DropForeignKey(
                name: "FK_audit_reports_ram_infos_Id",
                table: "audit_reports");

            migrationBuilder.DropForeignKey(
                name: "FK_audit_reports_system_identifiers_Id",
                table: "audit_reports");

            migrationBuilder.DropTable(
                name: "event_log");

            migrationBuilder.DropTable(
                name: "gpu_infos");

            migrationBuilder.DropTable(
                name: "monitor_infos");

            migrationBuilder.DropTable(
                name: "network_adapters");

            migrationBuilder.DropTable(
                name: "pc_report");

            migrationBuilder.DropTable(
                name: "physical_disks");

            migrationBuilder.DropTable(
                name: "printer_infos");

            migrationBuilder.DropTable(
                name: "ram_modules");

            migrationBuilder.DropTable(
                name: "pc");

            migrationBuilder.DropTable(
                name: "office");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "cpu_infos");

            migrationBuilder.DropTable(
                name: "motherboards");

            migrationBuilder.DropTable(
                name: "ram_infos");

            migrationBuilder.DropTable(
                name: "system_identifiers");

            migrationBuilder.DropTable(
                name: "audit_reports");
        }
    }
}
