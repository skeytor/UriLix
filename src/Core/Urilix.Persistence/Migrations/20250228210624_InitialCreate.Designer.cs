﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UriLix.Persistence;

#nullable disable

namespace UriLix.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250228210624_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("UriLix.Domain.Entities.ClickStatistic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Browser")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Device")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("IpAddress")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<string>("Referrer")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<Guid>("ShortenedUrlId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserAgent")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("VisitedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.HasIndex("ShortenedUrlId");

                    b.ToTable("ClickStatistic", (string)null);
                });

            modelBuilder.Entity("UriLix.Domain.Entities.ShortenedUrl", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Alias")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("CreateAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("OriginalUrl")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("ShortCode")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<DateTime>("UpdateAt")
                        .ValueGeneratedOnUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("Alias")
                        .IsUnique()
                        .HasFilter("[Alias] IS NOT NULL AND [Alias] <> ''");

                    b.HasIndex("Id");

                    b.HasIndex("ShortCode")
                        .IsUnique()
                        .HasFilter("[ShortCode] IS NOT NULL AND [ShortCode] <> ''");

                    b.HasIndex("UserId");

                    b.ToTable("ShortenedUrl", (string)null);
                });

            modelBuilder.Entity("UriLix.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("LastLoginAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime>("UpdateAt")
                        .ValueGeneratedOnUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Id");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("UriLix.Domain.Entities.ClickStatistic", b =>
                {
                    b.HasOne("UriLix.Domain.Entities.ShortenedUrl", "ShortenedUrl")
                        .WithMany("ClickStatistics")
                        .HasForeignKey("ShortenedUrlId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ShortenedUrl");
                });

            modelBuilder.Entity("UriLix.Domain.Entities.ShortenedUrl", b =>
                {
                    b.HasOne("UriLix.Domain.Entities.User", "User")
                        .WithMany("ShortenedLinks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("User");
                });

            modelBuilder.Entity("UriLix.Domain.Entities.ShortenedUrl", b =>
                {
                    b.Navigation("ClickStatistics");
                });

            modelBuilder.Entity("UriLix.Domain.Entities.User", b =>
                {
                    b.Navigation("ShortenedLinks");
                });
#pragma warning restore 612, 618
        }
    }
}
