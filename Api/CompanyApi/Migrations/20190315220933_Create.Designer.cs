﻿// <auto-generated />
using System;
using CompanyApi.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CompanyApi.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20190315220933_Create")]
    partial class Create
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CompanyApi.Models.Company", b =>
                {
                    b.Property<int>("CompanyId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Isin")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.HasKey("CompanyId");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("CompanyApi.Models.Stock", b =>
                {
                    b.Property<int>("StockId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyId");

                    b.Property<DateTime>("Time");

                    b.Property<double>("Value");

                    b.HasKey("StockId");

                    b.HasIndex("CompanyId");

                    b.ToTable("Stock");
                });

            modelBuilder.Entity("CompanyApi.Models.Stock", b =>
                {
                    b.HasOne("CompanyApi.Models.Company", "Company")
                        .WithMany("Stocks")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
