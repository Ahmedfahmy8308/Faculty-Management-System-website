using System;
using System.Collections.Generic;
using FacultyWebsite.Models;
using Microsoft.EntityFrameworkCore;

namespace FacultyWebsite.Data;

public partial class DBAppContext : DbContext
{
    public DBAppContext()
    {
    }

    public DBAppContext(DbContextOptions<DBAppContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseAssignment> CourseAssignments { get; set; }

    public virtual DbSet<CourseLec> CourseLecs { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<DocCourse> DocCourses { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<PreviousCourse> PreviousCourses { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentCourse> StudentCourses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=FAHMY;Initial Catalog=StudentManagementSystem;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Coursenum).HasName("PK__Courses__CF349579CEA188F9");

            entity.ToTable("Courses", "FacultySchema");

            entity.HasIndex(e => e.Coursename, "Crs_unq").IsUnique();

            entity.Property(e => e.Coursenum)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.BookPdf)
                .IsUnicode(false)
                .HasColumnName("Book_pdf");
            entity.Property(e => e.CourseDescription).IsUnicode(false);
            entity.Property(e => e.CourseLevel)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Coursename)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CourseAssignment>(entity =>
        {
            entity.HasKey(e => e.AssignmentNum);

            entity.ToTable("Course_assignment");

            entity.Property(e => e.AssignmentNum)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("Assignment_num");
            entity.Property(e => e.AssignmentPath)
                .IsUnicode(false)
                .HasColumnName("Assignment_Path");
            entity.Property(e => e.CourseNum)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Course_num");

            entity.HasOne(d => d.CourseNumNavigation).WithMany(p => p.CourseAssignments)
                .HasForeignKey(d => d.CourseNum)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Course_assignment_Courses");
        });

        modelBuilder.Entity<CourseLec>(entity =>
        {
            entity.HasKey(e => e.LectureNum);

            entity.ToTable("Course_Lec");

            entity.Property(e => e.LectureNum)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("Lecture_num");
            entity.Property(e => e.CourseNum)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Course_num");
            entity.Property(e => e.LecturePath)
                .IsUnicode(false)
                .HasColumnName("Lecture_Path");

            entity.HasOne(d => d.CourseNumNavigation).WithMany(p => p.CourseLecs)
                .HasForeignKey(d => d.CourseNum)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Course_Lec_Courses");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Depnum).HasName("PK__departme__03F81A77136C8F70");

            entity.ToTable("departments", "FacultySchema");

            entity.HasIndex(e => e.Depname, "depname_unq").IsUnique();

            entity.Property(e => e.Depnum)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Depname)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Ssn)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("SSN");
        });

        modelBuilder.Entity<DocCourse>(entity =>
        {
            entity.HasKey(e => e.SsnNum);

            entity.ToTable("DocCourses", "DoctorSchema");

            entity.Property(e => e.SsnNum)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SSN_NUM");
            entity.Property(e => e.Coursenum)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("coursenum");
            entity.Property(e => e.Ssn)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("SSN");

            entity.HasOne(d => d.CoursenumNavigation).WithMany(p => p.DocCourses)
                .HasForeignKey(d => d.Coursenum)
                .HasConstraintName("crsnum_fk_doccrs");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.Ssn);

            entity.ToTable("Doctors", "DoctorSchema");

            entity.HasIndex(e => e.Email, "Doc_Email_unq").IsUnique();

            entity.HasIndex(e => e.Phone, "Doc_Phone_unq").IsUnique();

            entity.Property(e => e.Ssn)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("SSN");
            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fname)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .HasMaxLength(450)
                .HasColumnName("ID");
            entity.Property(e => e.Lname)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PreviousCourse>(entity =>
        {
            entity.HasKey(e => e.SsnNum).HasName("PK_Previous_courses_1");

            entity.ToTable("Previous_courses");

            entity.Property(e => e.SsnNum)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ssn_Num");
            entity.Property(e => e.CourseName)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CourseNum)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Degree)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.StudentId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Student_ID");

            entity.HasOne(d => d.CourseNumNavigation).WithMany(p => p.PreviousCourses)
                .HasForeignKey(d => d.CourseNum)
                .HasConstraintName("FK_Previous_courses_Courses");

            entity.HasOne(d => d.Student).WithMany(p => p.PreviousCourses)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_Previous_courses_Students1");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Ssn).HasName("PK__Students__CA1E8E3DC161F510");

            entity.ToTable("Students", "StudentSchema");

            entity.HasIndex(e => e.Email, "St_Email_unq").IsUnique();

            entity.HasIndex(e => e.Phone, "St_Phone_unq").IsUnique();

            entity.Property(e => e.Ssn)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("SSN");
            entity.Property(e => e.Depnum)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fname)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .HasMaxLength(450)
                .HasColumnName("ID");
            entity.Property(e => e.IdentityId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Identity_ID");
            entity.Property(e => e.ImgPath)
                .IsUnicode(false)
                .HasColumnName("Img_path");
            entity.Property(e => e.Lname)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Seatnum)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(25)
                .IsUnicode(false);

            entity.HasOne(d => d.DepnumNavigation).WithMany(p => p.Students)
                .HasForeignKey(d => d.Depnum)
                .HasConstraintName("depnum_fk_std");
        });

        modelBuilder.Entity<StudentCourse>(entity =>
        {
            entity.HasKey(e => e.SsnNum);

            entity.ToTable("StudentCourses", "StudentSchema");

            entity.Property(e => e.SsnNum)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SSN_NUM");
            entity.Property(e => e.Coursenum)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Ssn)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("SSN");

            entity.HasOne(d => d.CoursenumNavigation).WithMany(p => p.StudentCourses)
                .HasForeignKey(d => d.Coursenum)
                .HasConstraintName("crsnum_fk_stdcrs");

            entity.HasOne(d => d.SsnNavigation).WithMany(p => p.StudentCourses)
                .HasForeignKey(d => d.Ssn)
                .HasConstraintName("ssn_fk_stdcrs");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
