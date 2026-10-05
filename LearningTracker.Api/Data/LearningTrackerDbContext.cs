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
}
