using System;
using System.Collections.Generic;
using FacultyWebsite.Models;
using Microsoft.EntityFrameworkCore;

namespace FacultyWebsite.Data;

public partial class AttendanceContext : DbContext
{
    public AttendanceContext()
    {
    }

    public AttendanceContext(DbContextOptions<AttendanceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<LectureAttendance> LectureAttendances { get; set; }

    public virtual DbSet<StudentAttendance> StudentAttendances { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=FAHMY;Initial Catalog=StudentManagementSystem;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LectureAttendance>(entity =>
        {
            entity.HasKey(e => e.LectureNumCourseNum);

            entity.ToTable("LectureAttendance");

            entity.Property(e => e.LectureNumCourseNum)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.CourseNum)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.DocSsn)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.LectureNum)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        modelBuilder.Entity<StudentAttendance>(entity =>
        {
            entity.HasKey(e => e.StuusernameLectureNumCourseNum);

            entity.ToTable("StudentAttendance");

            entity.Property(e => e.StuusernameLectureNumCourseNum)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.CourseNum)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.LectureNum)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Stuusername)
                .HasMaxLength(100)
                .IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
