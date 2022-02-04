using Microsoft.EntityFrameworkCore;

namespace StudentSystem.Data
{
    public class StudentsSystemDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Homework> Homeworks { get; set; }

        public DbSet<Resource> Resources { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.
                UseSqlServer("Server=.;Database=StudentsSystemDb;Integrated Security=True;");
            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                   .Entity<StudentCourse>()
                   .HasKey(sc => new { sc.StudentId, sc.CourseId });

            builder
                   .Entity<Student>()
                   .HasMany(s => s.Courses)
                   .WithOne(c => c.Student)
                   .HasForeignKey(c => c.StudentId);

            builder
                   .Entity<Student>()
                   .HasMany(s => s.Homeworks)
                   .WithOne(h => h.Student)
                   .HasForeignKey(h => h.StudentId);

            builder
                   .Entity<Course>()
                   .HasMany(c => c.Students)
                   .WithOne(s => s.Course)
                   .HasForeignKey(s => s.CourseId);

            builder.Entity<Course>()
                   .HasMany(c => c.Resources)
                   .WithOne(r => r.Course)
                   .HasForeignKey(r => r.CourseId);

            builder
                   .Entity<Course>()
                   .HasMany(c => c.Homeworks)
                   .WithOne(hw => hw.Course)
                   .HasForeignKey(hw => hw.CourseId);

            base.OnModelCreating(builder);
        }
    }
}
