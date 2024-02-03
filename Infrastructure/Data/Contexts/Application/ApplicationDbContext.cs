﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Core.Entities;

namespace Infrastructure.Data.Contexts.Application
{
    public class ApplicationDbContext : DbContext
    {
        #region Ctor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        #endregion

        #region DbSet
        public DbSet<Student> Students { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Course> Courses { get; set; }
        #endregion

        #region Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Fluent API

            #region Courses_Teachers
            modelBuilder.Entity<Course>()
            .HasMany(course => course.Teachers)
            .WithMany(teacher => teacher.Courses)
            .UsingEntity(
                l => l.HasOne(typeof(Teacher)).WithMany().HasForeignKey("CourseId"),
                r => r.HasOne(typeof(Course)).WithMany().HasForeignKey("TeacherId")
            ).ToTable("Courses_Teachers");
            #endregion

            #region Courses_Classrooms
            modelBuilder.Entity<Course>()
                .HasMany(course => course.Classrooms)
                .WithMany(department => department.Courses)
                .UsingEntity(
                    l => l.HasOne(typeof(Classroom)).WithMany().HasForeignKey("DepartmentId"),
                    r => r.HasOne(typeof(Course)).WithMany().HasForeignKey("CourseId")
                ).ToTable("Courses_Classrooms");
            #endregion

            #region Courses_Departments
            modelBuilder.Entity<Course>()
                .HasMany(course => course.Departments)
                .WithMany(department => department.Courses)
                .UsingEntity(
                    l => l.HasOne(typeof(Department)).WithMany().HasForeignKey("CourseId"),
                    r => r.HasOne(typeof(Course)).WithMany().HasForeignKey("DepartmentId")
                ).ToTable("Courses_Departments");
            #endregion

            #region Department_Students
            modelBuilder.Entity<Department>()
                .HasMany(school => school.Students)
                .WithOne(student => student.Department)
                .HasForeignKey(school => school.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region Departments_School
            modelBuilder.Entity<Department>()
            .HasOne(department => department.School)
            .WithMany(school => school.Departments)
            .HasForeignKey(department => department.SchoolId)
            .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #endregion

        }
        #endregion
    }
}
