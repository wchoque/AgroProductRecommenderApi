using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DataAccess.Models
{
    public partial class AppCentroEstudiosDBContext : DbContext
    {
        public AppCentroEstudiosDBContext()
        {
        }

        public AppCentroEstudiosDBContext(DbContextOptions<AppCentroEstudiosDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseBySemester> CourseBySemesters { get; set; }
        public virtual DbSet<CourseBySemesterEnroll> CourseBySemesterEnrolls { get; set; }
        public virtual DbSet<CourseBySemesterEvaluation> CourseBySemesterEvaluations { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<Note> Notes { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<Sede> Sedes { get; set; }
        public virtual DbSet<Semester> Semesters { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserByType> UserByTypes { get; set; }
        public virtual DbSet<UserInformation> UserInformations { get; set; }
        public virtual DbSet<UserType> UserTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.;Database=AppCentroEstudiosDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.ToTable("ChatMessage");

                entity.Property(e => e.MessageContent)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.UserIdFromNavigation)
                    .WithMany(p => p.ChatMessageUserIdFromNavigations)
                    .HasForeignKey(d => d.UserIdFrom)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChatMessage_UserIdFrom");

                entity.HasOne(d => d.UserIdToNavigation)
                    .WithMany(p => p.ChatMessageUserIdToNavigations)
                    .HasForeignKey(d => d.UserIdTo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChatMessage_UserIdTo");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Course");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CourseBySemester>(entity =>
            {
                entity.ToTable("CourseBySemester");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.CourseBySemesters)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CourseBySemester_Course");

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.CourseBySemesters)
                    .HasForeignKey(d => d.SemesterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CourseBySemester_Semester");
            });

            modelBuilder.Entity<CourseBySemesterEnroll>(entity =>
            {
                entity.ToTable("CourseBySemesterEnroll");

                entity.HasOne(d => d.CourseBySemester)
                    .WithMany(p => p.CourseBySemesterEnrolls)
                    .HasForeignKey(d => d.CourseBySemesterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CourseBySemesterEnroll_CourseBySemester");

                entity.HasOne(d => d.UserByTypeStudent)
                    .WithMany(p => p.CourseBySemesterEnrollUserByTypeStudents)
                    .HasForeignKey(d => d.UserByTypeStudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CourseBySemesterEnroll_UserByTypeStudent");

                entity.HasOne(d => d.UserByTypeTeacher)
                    .WithMany(p => p.CourseBySemesterEnrollUserByTypeTeachers)
                    .HasForeignKey(d => d.UserByTypeTeacherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CourseBySemesterEnroll_UserByTypeTeacher");
            });

            modelBuilder.Entity<CourseBySemesterEvaluation>(entity =>
            {
                entity.ToTable("CourseBySemesterEvaluation");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.CourseBySemester)
                    .WithMany(p => p.CourseBySemesterEvaluations)
                    .HasForeignKey(d => d.CourseBySemesterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CourseBySemesterEvaluation_CourseBySemester");
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.ToTable("Invoice");

                entity.Property(e => e.Amount).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.CourseBySemesterEnroll)
                    .WithMany(p => p.Invoices)
                    .HasForeignKey(d => d.CourseBySemesterEnrollId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Invoice_CourseBySemesterEnroll");
            });

            modelBuilder.Entity<Note>(entity =>
            {
                entity.ToTable("Note");

                entity.Property(e => e.Value).HasColumnType("decimal(4, 2)");

                entity.HasOne(d => d.CourseBySemesterEnroll)
                    .WithMany(p => p.Notes)
                    .HasForeignKey(d => d.CourseBySemesterEnrollId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Note_CourseBySemesterEnroll");

                entity.HasOne(d => d.CourseBySemesterEvaluation)
                    .WithMany(p => p.Notes)
                    .HasForeignKey(d => d.CourseBySemesterEvaluationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Note_CourseBySemesterEvaluation");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("Post");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.UserByType)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.UserByTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Post_UserByType");
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.ToTable("Schedule");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.CourseBySemester)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.CourseBySemesterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Schedule_CourseBySemester");
            });

            modelBuilder.Entity<Sede>(entity =>
            {
                entity.ToTable("Sede");

                entity.Property(e => e.Latitud).HasColumnType("decimal(10, 6)");

                entity.Property(e => e.Longitud).HasColumnType("decimal(10, 6)");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Semester>(entity =>
            {
                entity.ToTable("Semester");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.AvatarUrl).IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.UserInformation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserInformationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_UserInformation");
            });

            modelBuilder.Entity<UserByType>(entity =>
            {
                entity.ToTable("UserByType");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserByTypes)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserByType_User");

                entity.HasOne(d => d.UserType)
                    .WithMany(p => p.UserByTypes)
                    .HasForeignKey(d => d.UserTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserByType_UserType");
            });

            modelBuilder.Entity<UserInformation>(entity =>
            {
                entity.ToTable("UserInformation");

                entity.Property(e => e.Dni)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ImageUrl).IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(9)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserType>(entity =>
            {
                entity.ToTable("UserType");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
