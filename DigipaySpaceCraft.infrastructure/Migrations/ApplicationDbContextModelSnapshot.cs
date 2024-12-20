﻿// <auto-generated />
using System;
using DigipaySpaceCraft.infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DigipaySpaceCraft.infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DigipaySpaceCraft.Domain.Entites.WeatherRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<float>("GenerationTimeMs")
                        .HasColumnType("real");

                    b.Property<DateTime>("RequestedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"));

                    b.ToTable("WeatherRequests");
                });

            modelBuilder.Entity("WeatherHourlyData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<float>("Temperature")
                        .HasColumnType("real");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<int?>("WeatherRequestId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WeatherRequestId");

                    b.ToTable("WeatherHourlyData");
                });

            modelBuilder.Entity("WeatherHourlyData", b =>
                {
                    b.HasOne("DigipaySpaceCraft.Domain.Entites.WeatherRequest", null)
                        .WithMany("HourlyWeatherData")
                        .HasForeignKey("WeatherRequestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DigipaySpaceCraft.Domain.Entites.WeatherRequest", b =>
                {
                    b.Navigation("HourlyWeatherData");
                });
#pragma warning restore 612, 618
        }
    }
}
