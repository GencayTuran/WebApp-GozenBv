﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApp_GozenBv.Data;

#nullable disable

namespace WebApp_GozenBv.Migrations
{
    [DbContext(typeof(DataDbContext))]
    partial class DataDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("WebApp_GozenBv.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("FirmaId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FirmaId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("WebApp_GozenBv.Models.Firma", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("FirmaName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Firmas");
                });

            modelBuilder.Entity("WebApp_GozenBv.Models.Stock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<double>("Cost")
                        .HasColumnType("float");

                    b.Property<int>("MinQuantity")
                        .HasColumnType("int");

                    b.Property<string>("ProductBrand")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<bool>("Used")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Stock");
                });

            modelBuilder.Entity("WebApp_GozenBv.Models.StockLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime?>("CompletionDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Damaged")
                        .HasColumnType("bit");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<string>("LogCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ReturnDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("StockLogDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("StockLogs");
                });

            modelBuilder.Entity("WebApp_GozenBv.Models.StockLogItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("DamagedAmount")
                        .HasColumnType("int");

                    b.Property<int?>("DeletedAmount")
                        .HasColumnType("int");

                    b.Property<string>("LogCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductNameBrand")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RepairedAmount")
                        .HasColumnType("int");

                    b.Property<int>("StockAmount")
                        .HasColumnType("int");

                    b.Property<int>("StockId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("StockLogItems");
                });

            modelBuilder.Entity("WebApp_GozenBv.Models.WagenMaintenance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("MaintenanceDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("MaintenanceNotes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WagenId")
                        .HasColumnType("int");

                    b.Property<int?>("WagenParkId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WagenParkId");

                    b.ToTable("WagenMaintenances");
                });

            modelBuilder.Entity("WebApp_GozenBv.Models.WagenPark", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Brand")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ChassisNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DeadlineKeuring")
                        .HasColumnType("datetime2");

                    b.Property<int>("FirmaId")
                        .HasColumnType("int");

                    b.Property<DateTime>("KeuringDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("Km")
                        .HasColumnType("float");

                    b.Property<string>("LicencePlate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Model")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FirmaId");

                    b.ToTable("WagenPark");
                });

            modelBuilder.Entity("WebApp_GozenBv.Models.Employee", b =>
                {
                    b.HasOne("WebApp_GozenBv.Models.Firma", "Firma")
                        .WithMany()
                        .HasForeignKey("FirmaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Firma");
                });

            modelBuilder.Entity("WebApp_GozenBv.Models.StockLog", b =>
                {
                    b.HasOne("WebApp_GozenBv.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("WebApp_GozenBv.Models.WagenMaintenance", b =>
                {
                    b.HasOne("WebApp_GozenBv.Models.WagenPark", "WagenPark")
                        .WithMany()
                        .HasForeignKey("WagenParkId");

                    b.Navigation("WagenPark");
                });

            modelBuilder.Entity("WebApp_GozenBv.Models.WagenPark", b =>
                {
                    b.HasOne("WebApp_GozenBv.Models.Firma", "Firma")
                        .WithMany()
                        .HasForeignKey("FirmaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Firma");
                });
#pragma warning restore 612, 618
        }
    }
}
