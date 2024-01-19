﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Schedoo.Server.Data;

#nullable disable

namespace Schedoo.Server.Migrations
{
    [DbContext(typeof(SchedooContext))]
    partial class SchedooContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Schedoo.Server.Models.Class", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LessonType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LinkToMeeting")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TeacherId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.HasIndex("TeacherId");

                    b.ToTable("Classes");
                });

            modelBuilder.Entity("Schedoo.Server.Models.Room", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("Schedoo.Server.Models.Schedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<Guid>("ClassId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DayOfWeek")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GroupId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TimeSlotId")
                        .HasColumnType("int");

                    b.Property<string>("WeekType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ClassId");

                    b.HasIndex("TimeSlotId");

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("Schedoo.Server.Models.ScheduleDate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("ScheduleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ScheduleDates");
                });

            modelBuilder.Entity("Schedoo.Server.Models.Semester", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("CurrentSemester")
                        .HasColumnType("bit");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndDay")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartDay")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Semesters");
                });

            modelBuilder.Entity("Schedoo.Server.Models.Teacher", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Patronymic")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("Schedoo.Server.Models.TimeSlot", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("ClassName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("EndTime")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("StartTime")
                        .HasColumnType("time");

                    b.HasKey("Id");

                    b.ToTable("TimeSlots");
                });

            modelBuilder.Entity("Schedoo.Server.Models.Class", b =>
                {
                    b.HasOne("Schedoo.Server.Models.Room", "Room")
                        .WithMany()
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Schedoo.Server.Models.Teacher", "Teacher")
                        .WithMany()
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("Schedoo.Server.Models.Schedule", b =>
                {
                    b.HasOne("Schedoo.Server.Models.Class", "Class")
                        .WithMany()
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Schedoo.Server.Models.TimeSlot", "TimeSlot")
                        .WithMany()
                        .HasForeignKey("TimeSlotId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Class");

                    b.Navigation("TimeSlot");
                });
#pragma warning restore 612, 618
        }
    }
}
