﻿// <auto-generated />
using System;
using KuberAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KuberAPI.Migrations
{
    [DbContext(typeof(KuberContext))]
    [Migration("20190820061900_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("KuberAPI.Models.Address", b =>
                {
                    b.Property<int>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FormattedAddress");

                    b.Property<decimal>("Latitude")
                        .HasColumnType("decimal(18,14)");

                    b.Property<decimal>("Longitude")
                        .HasColumnType("decimal(18,14)");

                    b.HasKey("AddressId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("KuberAPI.Models.Ride", b =>
                {
                    b.Property<int>("RideId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("Accepted");

                    b.Property<DateTime?>("Cancelled");

                    b.Property<string>("CurrentAddress");

                    b.Property<decimal?>("CurrentLatitude")
                        .HasColumnType("decimal(18,14)");

                    b.Property<decimal?>("CurrentLongitude")
                        .HasColumnType("decimal(18,14)");

                    b.Property<int?>("DriverId");

                    b.Property<DateTime?>("DroppedOff");

                    b.Property<int>("FromAddressId");

                    b.Property<int>("PassengerId");

                    b.Property<DateTime?>("PickedUp");

                    b.Property<DateTime?>("PositionUpdated");

                    b.Property<DateTime?>("Rejected");

                    b.Property<DateTime>("Requested");

                    b.Property<int>("RideStatus");

                    b.Property<int>("ToAddressId");

                    b.HasKey("RideId");

                    b.HasIndex("DriverId");

                    b.HasIndex("FromAddressId");

                    b.HasIndex("PassengerId");

                    b.HasIndex("ToAddressId");

                    b.ToTable("Rides");
                });

            modelBuilder.Entity("KuberAPI.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created");

                    b.Property<string>("EmailAddress")
                        .IsRequired();

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PasswordSalt");

                    b.Property<int>("UserType");

                    b.HasKey("UserId");

                    b.HasIndex("EmailAddress")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("KuberAPI.Models.Ride", b =>
                {
                    b.HasOne("KuberAPI.Models.User", "Driver")
                        .WithMany()
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("KuberAPI.Models.Address", "FromAddress")
                        .WithMany()
                        .HasForeignKey("FromAddressId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("KuberAPI.Models.User", "Passenger")
                        .WithMany()
                        .HasForeignKey("PassengerId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("KuberAPI.Models.Address", "ToAddress")
                        .WithMany()
                        .HasForeignKey("ToAddressId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
