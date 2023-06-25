﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WMSApi.Models;

#nullable disable

namespace WMSApi.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20230625010706_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WMSApi.Models.Item", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateLastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UPC")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("WMSApi.Models.Pallet", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("PalletBayId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("PalletBayId");

                    b.ToTable("Pallets");
                });

            modelBuilder.Entity("WMSApi.Models.PalletBay", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Floor")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Row")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Section")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PalletBays");
                });

            modelBuilder.Entity("WMSApi.Models.PurchaseOrder", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Comments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateEstimatedDelivery")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateLastModified")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateReceived")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("PurchaseOrders");
                });

            modelBuilder.Entity("WMSApi.Models.PurchaseOrderItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<double>("CurrentQuantity")
                        .HasColumnType("float");

                    b.Property<double>("FreightPrice")
                        .HasColumnType("float");

                    b.Property<long>("ItemId")
                        .HasColumnType("bigint");

                    b.Property<double>("MarginPrice")
                        .HasColumnType("float");

                    b.Property<double>("MarkupPrice")
                        .HasColumnType("float");

                    b.Property<long?>("PalletId")
                        .HasColumnType("bigint");

                    b.Property<long>("PurchaseOrderId")
                        .HasColumnType("bigint");

                    b.Property<double>("PurchasedQuantity")
                        .HasColumnType("float");

                    b.Property<double>("SellPrice")
                        .HasColumnType("float");

                    b.Property<double>("UnitPrice")
                        .HasColumnType("float");

                    b.Property<double>("Weight")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("PalletId");

                    b.HasIndex("PurchaseOrderId");

                    b.ToTable("PurchaseOrderItems");
                });

            modelBuilder.Entity("WMSApi.Models.SalesOrder", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Comments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateFilling")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateLastModified")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateReceived")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateShipped")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateStaged")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("SalesOrders");
                });

            modelBuilder.Entity("WMSApi.Models.SalesOrderItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("PurchaseOrderItemId")
                        .HasColumnType("bigint");

                    b.Property<double>("Quantity")
                        .HasColumnType("float");

                    b.Property<long>("SalesOrderId")
                        .HasColumnType("bigint");

                    b.Property<double>("UnitPrice")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("PurchaseOrderItemId");

                    b.HasIndex("SalesOrderId");

                    b.ToTable("SalesOrderItems");
                });

            modelBuilder.Entity("WMSApi.Models.Pallet", b =>
                {
                    b.HasOne("WMSApi.Models.PalletBay", "PalletBay")
                        .WithMany("Pallets")
                        .HasForeignKey("PalletBayId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PalletBay");
                });

            modelBuilder.Entity("WMSApi.Models.PurchaseOrderItem", b =>
                {
                    b.HasOne("WMSApi.Models.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WMSApi.Models.Pallet", "Pallet")
                        .WithMany()
                        .HasForeignKey("PalletId");

                    b.HasOne("WMSApi.Models.PurchaseOrder", "PurchaseOrder")
                        .WithMany("PurchaseOrderItems")
                        .HasForeignKey("PurchaseOrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("Pallet");

                    b.Navigation("PurchaseOrder");
                });

            modelBuilder.Entity("WMSApi.Models.SalesOrderItem", b =>
                {
                    b.HasOne("WMSApi.Models.PurchaseOrderItem", "PurchaseOrderItem")
                        .WithMany()
                        .HasForeignKey("PurchaseOrderItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WMSApi.Models.SalesOrder", "SalesOrder")
                        .WithMany("SalesOrderItems")
                        .HasForeignKey("SalesOrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PurchaseOrderItem");

                    b.Navigation("SalesOrder");
                });

            modelBuilder.Entity("WMSApi.Models.PalletBay", b =>
                {
                    b.Navigation("Pallets");
                });

            modelBuilder.Entity("WMSApi.Models.PurchaseOrder", b =>
                {
                    b.Navigation("PurchaseOrderItems");
                });

            modelBuilder.Entity("WMSApi.Models.SalesOrder", b =>
                {
                    b.Navigation("SalesOrderItems");
                });
#pragma warning restore 612, 618
        }
    }
}
