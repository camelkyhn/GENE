﻿// <auto-generated />
using System;
using Gene.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Gene.Storage.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20220717021604_Gene_2_Entity_History")]
    partial class Gene_2_Entity_History
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Gene.Middleware.Entities.Core.Action", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("CreatedDate")
                        .IsRequired()
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("CreatedUserId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .IsRequired()
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("UpdatedUserId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CreatedUserId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("UpdatedUserId");

                    b.ToTable("Action", (string)null);
                });

            modelBuilder.Entity("Gene.Middleware.Entities.Core.Area", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("CreatedDate")
                        .IsRequired()
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("CreatedUserId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("IconText")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .IsRequired()
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("UpdatedUserId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CreatedUserId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("UpdatedUserId");

                    b.ToTable("Area", (string)null);
                });

            modelBuilder.Entity("Gene.Middleware.Entities.Core.Controller", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AreaId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("CreatedDate")
                        .IsRequired()
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("CreatedUserId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("IconText")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .IsRequired()
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("UpdatedUserId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("AreaId");

                    b.HasIndex("CreatedUserId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("UpdatedUserId");

                    b.ToTable("Controller", (string)null);
                });

            modelBuilder.Entity("Gene.Middleware.Entities.Core.ControllerAction", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ActionId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ControllerId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("CreatedDate")
                        .IsRequired()
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("CreatedUserId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .IsRequired()
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("UpdatedUserId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ActionId");

                    b.HasIndex("ControllerId");

                    b.HasIndex("CreatedUserId");

                    b.HasIndex("UpdatedUserId");

                    b.ToTable("ControllerAction", (string)null);
                });

            modelBuilder.Entity("Gene.Middleware.Entities.Core.ControllerActionRole", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ControllerActionId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("CreatedDate")
                        .IsRequired()
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("CreatedUserId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("RoleId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .IsRequired()
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("UpdatedUserId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ControllerActionId");

                    b.HasIndex("CreatedUserId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UpdatedUserId");

                    b.ToTable("ControllerActionRole", (string)null);
                });

            modelBuilder.Entity("Gene.Middleware.Entities.Core.EntityHistoryRecord", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTimeOffset>("DateTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("EntityId")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("EntityName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("JsonValue")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("EntityHistoryRecord", (string)null);
                });

            modelBuilder.Entity("Gene.Middleware.Entities.Identity.Notification", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("CreatedDate")
                        .IsRequired()
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("CreatedUserId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsOpened")
                        .HasColumnType("bit");

                    b.Property<Guid?>("ReceiverUserId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .IsRequired()
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("UpdatedUserId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CreatedUserId");

                    b.HasIndex("ReceiverUserId");

                    b.HasIndex("UpdatedUserId");

                    b.ToTable("Notification");
                });

            modelBuilder.Entity("Gene.Middleware.Entities.Identity.Role", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("CreatedDate")
                        .IsRequired()
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("CreatedUserId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .IsRequired()
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("UpdatedUserId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CreatedUserId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("UpdatedUserId");

                    b.ToTable("Role", (string)null);
                });

            modelBuilder.Entity("Gene.Middleware.Entities.Identity.User", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<short>("AccessFailedCount")
                        .HasColumnType("smallint");

                    b.Property<DateTimeOffset?>("CreatedDate")
                        .IsRequired()
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("CreatedUserId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("IsEmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("IsEmailEnabled")
                        .HasColumnType("bit");

                    b.Property<bool>("IsLockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSmsEnabled")
                        .HasColumnType("bit");

                    b.Property<bool>("IsTwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .IsRequired()
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("UpdatedUserId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CreatedUserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("UpdatedUserId");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("Gene.Middleware.Entities.Identity.UserRole", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("CreatedDate")
                        .IsRequired()
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("CreatedUserId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("RoleId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .IsRequired()
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("UpdatedUserId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("UserId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CreatedUserId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UpdatedUserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRole", (string)null);
                });

            modelBuilder.Entity("Gene.Middleware.Entities.Core.Action", b =>
                {
                    b.HasOne("Gene.Middleware.Entities.Identity.User", "CreatedUser")
                        .WithMany("CreatedActions")
                        .HasForeignKey("CreatedUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Gene.Middleware.Entities.Identity.User", "UpdatedUser")
                        .WithMany("UpdatedActions")
                        .HasForeignKey("UpdatedUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CreatedUser");

                    b.Navigation("UpdatedUser");
                });

            modelBuilder.Entity("Gene.Middleware.Entities.Core.Area", b =>
                {
                    b.HasOne("Gene.Middleware.Entities.Identity.User", "CreatedUser")
                        .WithMany("CreatedAreas")
                        .HasForeignKey("CreatedUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Gene.Middleware.Entities.Identity.User", "UpdatedUser")
                        .WithMany("UpdatedAreas")
                        .HasForeignKey("UpdatedUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CreatedUser");

                    b.Navigation("UpdatedUser");
                });

            modelBuilder.Entity("Gene.Middleware.Entities.Core.Controller", b =>
                {
                    b.HasOne("Gene.Middleware.Entities.Core.Area", "Area")
                        .WithMany("Controllers")
                        .HasForeignKey("AreaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Gene.Middleware.Entities.Identity.User", "CreatedUser")
                        .WithMany("CreatedControllers")
                        .HasForeignKey("CreatedUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Gene.Middleware.Entities.Identity.User", "UpdatedUser")
                        .WithMany("UpdatedControllers")
                        .HasForeignKey("UpdatedUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Area");

                    b.Navigation("CreatedUser");

                    b.Navigation("UpdatedUser");
                });

            modelBuilder.Entity("Gene.Middleware.Entities.Core.ControllerAction", b =>
                {
                    b.HasOne("Gene.Middleware.Entities.Core.Action", "Action")
                        .WithMany("ControllerActions")
                        .HasForeignKey("ActionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Gene.Middleware.Entities.Core.Controller", "Controller")
                        .WithMany("ControllerActions")
                        .HasForeignKey("ControllerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Gene.Middleware.Entities.Identity.User", "CreatedUser")
                        .WithMany("CreatedControllerActions")
                        .HasForeignKey("CreatedUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Gene.Middleware.Entities.Identity.User", "UpdatedUser")
                        .WithMany("UpdatedControllerActions")
                        .HasForeignKey("UpdatedUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Action");

                    b.Navigation("Controller");

                    b.Navigation("CreatedUser");

                    b.Navigation("UpdatedUser");
                });

            modelBuilder.Entity("Gene.Middleware.Entities.Core.ControllerActionRole", b =>
                {
                    b.HasOne("Gene.Middleware.Entities.Core.ControllerAction", "ControllerAction")
                        .WithMany("ControllerActionRoles")
                        .HasForeignKey("ControllerActionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Gene.Middleware.Entities.Identity.User", "CreatedUser")
                        .WithMany("CreatedControllerActionRoles")
                        .HasForeignKey("CreatedUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Gene.Middleware.Entities.Identity.Role", "Role")
                        .WithMany("ControllerActionRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Gene.Middleware.Entities.Identity.User", "UpdatedUser")
                        .WithMany("UpdatedControllerActionRoles")
                        .HasForeignKey("UpdatedUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ControllerAction");

                    b.Navigation("CreatedUser");

                    b.Navigation("Role");

                    b.Navigation("UpdatedUser");
                });

            modelBuilder.Entity("Gene.Middleware.Entities.Identity.Notification", b =>
                {
                    b.HasOne("Gene.Middleware.Entities.Identity.User", "CreatedUser")
                        .WithMany("CreatedNotifications")
                        .HasForeignKey("CreatedUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Gene.Middleware.Entities.Identity.User", "ReceiverUser")
                        .WithMany("Notifications")
                        .HasForeignKey("ReceiverUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Gene.Middleware.Entities.Identity.User", "UpdatedUser")
                        .WithMany("UpdatedNotifications")
                        .HasForeignKey("UpdatedUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CreatedUser");

                    b.Navigation("ReceiverUser");

                    b.Navigation("UpdatedUser");
                });

            modelBuilder.Entity("Gene.Middleware.Entities.Identity.Role", b =>
                {
                    b.HasOne("Gene.Middleware.Entities.Identity.User", "CreatedUser")
                        .WithMany("CreatedRoles")
                        .HasForeignKey("CreatedUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Gene.Middleware.Entities.Identity.User", "UpdatedUser")
                        .WithMany("UpdatedRoles")
                        .HasForeignKey("UpdatedUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CreatedUser");

                    b.Navigation("UpdatedUser");
                });

            modelBuilder.Entity("Gene.Middleware.Entities.Identity.User", b =>
                {
                    b.HasOne("Gene.Middleware.Entities.Identity.User", "CreatedUser")
                        .WithMany("CreatedUsers")
                        .HasForeignKey("CreatedUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Gene.Middleware.Entities.Identity.User", "UpdatedUser")
                        .WithMany("UpdatedUsers")
                        .HasForeignKey("UpdatedUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CreatedUser");

                    b.Navigation("UpdatedUser");
                });

            modelBuilder.Entity("Gene.Middleware.Entities.Identity.UserRole", b =>
                {
                    b.HasOne("Gene.Middleware.Entities.Identity.User", "CreatedUser")
                        .WithMany("CreatedUserRoles")
                        .HasForeignKey("CreatedUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Gene.Middleware.Entities.Identity.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Gene.Middleware.Entities.Identity.User", "UpdatedUser")
                        .WithMany("UpdatedUserRoles")
                        .HasForeignKey("UpdatedUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Gene.Middleware.Entities.Identity.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedUser");

                    b.Navigation("Role");

                    b.Navigation("UpdatedUser");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Gene.Middleware.Entities.Core.Action", b =>
                {
                    b.Navigation("ControllerActions");
                });

            modelBuilder.Entity("Gene.Middleware.Entities.Core.Area", b =>
                {
                    b.Navigation("Controllers");
                });

            modelBuilder.Entity("Gene.Middleware.Entities.Core.Controller", b =>
                {
                    b.Navigation("ControllerActions");
                });

            modelBuilder.Entity("Gene.Middleware.Entities.Core.ControllerAction", b =>
                {
                    b.Navigation("ControllerActionRoles");
                });

            modelBuilder.Entity("Gene.Middleware.Entities.Identity.Role", b =>
                {
                    b.Navigation("ControllerActionRoles");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("Gene.Middleware.Entities.Identity.User", b =>
                {
                    b.Navigation("CreatedActions");

                    b.Navigation("CreatedAreas");

                    b.Navigation("CreatedControllerActionRoles");

                    b.Navigation("CreatedControllerActions");

                    b.Navigation("CreatedControllers");

                    b.Navigation("CreatedNotifications");

                    b.Navigation("CreatedRoles");

                    b.Navigation("CreatedUserRoles");

                    b.Navigation("CreatedUsers");

                    b.Navigation("Notifications");

                    b.Navigation("UpdatedActions");

                    b.Navigation("UpdatedAreas");

                    b.Navigation("UpdatedControllerActionRoles");

                    b.Navigation("UpdatedControllerActions");

                    b.Navigation("UpdatedControllers");

                    b.Navigation("UpdatedNotifications");

                    b.Navigation("UpdatedRoles");

                    b.Navigation("UpdatedUserRoles");

                    b.Navigation("UpdatedUsers");

                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}