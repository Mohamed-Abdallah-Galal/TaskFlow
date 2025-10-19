
using Microsoft.EntityFrameworkCore;
using TaskFlow.DAL.Entites;

namespace TaskFlow.DAL.DataBase
{
    public class TaskFlowDbContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=TaskFlow;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure AppUser - Only enum conversion
            modelBuilder.Entity<AppUser>(entity =>
            {
                // Configure enum as string
                entity.Property(u => u.UserRole)
                    .HasConversion<string>()
                    .HasMaxLength(20);
            });

            // Configure TaskItem - Only enum conversions and relationships
            modelBuilder.Entity<TaskItem>(entity =>
            {
                // Configure enums as strings
                entity.Property(t => t.Priority)
                    .HasConversion<string>()
                    .HasMaxLength(20);

                entity.Property(t => t.Status)
                    .HasConversion<string>()
                    .HasMaxLength(20);

                entity.Property(t => t.Category)
                .HasConversion<string>()
                .HasMaxLength(20);

                // Configure relationships (ESSENTIAL - fixes your error)
                entity.HasOne(t => t.CreatedBy)
                    .WithMany(u=>u.CreatedTasks)
                    .HasForeignKey(t=>t.CreatedById)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.AssignedTo)
                    .WithMany(u => u.AssignedTasks)
                    .HasForeignKey(t => t.AssignedToId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Comment - Only relationships
            modelBuilder.Entity<Comment>(entity =>
            {
                // Relationships
                entity.HasOne(c => c.Task)
                    .WithMany(t => t.Comments)
                    .HasForeignKey(c => c.TaskId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(c => c.User)
                    .WithMany(u => u.Comments)
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Attachment - Only relationships
            modelBuilder.Entity<Attachment>(entity =>
            {
                entity.Property(a => a.FileType)
                .HasConversion<string>()
                .HasMaxLength(20);
                // Relationships
                entity.HasOne(a => a.Task)
                    .WithMany(t => t.Attachments)
                    .HasForeignKey(a => a.TaskId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            //Configure TaskHistory -Only relationships
            modelBuilder.Entity<TaskHistory>(entity =>
            {

                entity.Property(th => th.ActionType)
                .HasConversion<string>()
                .HasMaxLength(20);
                // Relationships
                entity.HasOne(th => th.Task)
                    .WithMany(t => t.History)
                    .HasForeignKey(th => th.TaskId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(th => th.ChangedByUser)
                    .WithMany(u => u.TaskHistories)
                    .HasForeignKey(th => th.ChangedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // Optional but recommended: Soft delete query filters
            modelBuilder.Entity<AppUser>().HasQueryFilter(u => !u.IsDeleted);
            modelBuilder.Entity<TaskItem>().HasQueryFilter(t => !t.IsDeleted);
            modelBuilder.Entity<Comment>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<Attachment>().HasQueryFilter(a => !a.IsDeleted);
            modelBuilder.Entity<TaskHistory>().HasQueryFilter(th => !th.IsDeleted);
        }
        public DbSet<AppUser> appUsers { get; set; }
        public DbSet<Attachment> attachments { get; set; }
        public DbSet<Comment> comments { get; set; }
        public DbSet<TaskItem> taskItems { get; set; }
        public DbSet<TaskHistory> taskHistories { get; set; }
    }
}
