﻿// <auto-generated />
using AmootSearch.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AmootSearch.Migrations
{
    [DbContext(typeof(DataBaseContext))]
    [Migration("20231110141916_add_model2")]
    partial class add_model2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.24")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("AmootSearch.Entities.Model", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Index")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Info")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TotalNumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Index")
                        .IsUnique()
                        .HasFilter("[Index] IS NOT NULL");

                    b.ToTable("Models");
                });

            modelBuilder.Entity("AmootSearch.Entities.Text", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Texts");
                });
#pragma warning restore 612, 618
        }
    }
}
