﻿// <auto-generated />
using System;
using BabyStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BabyStore.Migrations
{
    [DbContext(typeof(BabyStoreContext))]
    [Migration("20190403021405_P1")]
    partial class P1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BabyStore.Models.Category", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("ID");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("BabyStore.Models.Product", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CategoryID");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<decimal>("Price");

                    b.HasKey("ID");

                    b.HasIndex("CategoryID");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("BabyStore.Models.ProductImage", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FileName")
                        .HasMaxLength(100);

                    b.HasKey("ID");

                    b.HasIndex("FileName")
                        .IsUnique()
                        .HasFilter("[FileName] IS NOT NULL");

                    b.ToTable("ProductImage");
                });

            modelBuilder.Entity("BabyStore.Models.ProductImageMapping", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ImageNumber");

                    b.Property<int>("ProductID");

                    b.Property<int>("ProductImageID");

                    b.HasKey("ID");

                    b.HasIndex("ProductID");

                    b.HasIndex("ProductImageID");

                    b.ToTable("ProductImageMapping");
                });

            modelBuilder.Entity("BabyStore.ViewModel.RoleViewModel", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("RoleViewModel");
                });

            modelBuilder.Entity("BabyStore.Models.Product", b =>
                {
                    b.HasOne("BabyStore.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryID");
                });

            modelBuilder.Entity("BabyStore.Models.ProductImageMapping", b =>
                {
                    b.HasOne("BabyStore.Models.Product", "Product")
                        .WithMany("ProductImageMappings")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BabyStore.Models.ProductImage", "ProductImage")
                        .WithMany("ProductImageMappings")
                        .HasForeignKey("ProductImageID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
