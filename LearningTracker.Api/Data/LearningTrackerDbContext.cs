using LearningTracker.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningTracker.Api.Data;

public class LearningTrackerDbContext : DbContext
{
    public LearningTrackerDbContext(DbContextOptions<LearningTrackerDbContext> options) : base(options)
    {

    }
    public DbSet<LearningTopic> LearningTopics { get; set; }
    public DbSet<StudySession> StudySessions { get; set; }
    public DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StudySession>()
            .HasOne(o => o.LearningTopic)   
            .WithMany(c => c.StudySessions)           
            .HasForeignKey(o => o.LearningTopicId)  
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}
