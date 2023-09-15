using Domain.Models.Gov;
using Domain.Models.Relational;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Domain.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ProcessTransition>()
                .HasOne(c => c.From)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ProcessTransition>()
                .HasOne(c => c.To)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Feedback>()
                .HasOne(c => c.ShahrbinInstance)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<QuickAccess>()
                .HasOne(c => c.ShahrbinInstance)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Report>()
                .HasOne(c => c.ShahrbinInstance)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //    => optionsBuilder.LogTo(Console.WriteLine);
        public DbSet<Media> Media { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Process> Process { get; set; }
        public DbSet<ProcessStage> Stage { get; set; }
        public DbSet<ProcessTransition> Transition { get; set; }
        public DbSet<Report> Reports { get; set; }
        //public DbSet<Priority> Priority { get; set; }
        public DbSet<TransitionLog> TransitionLogs { get; set; }
        public DbSet<BotActor> BotActors { get; set; }
        public DbSet<Actor> Actor { get; set; }
        public DbSet<Province> Province { get; set; }
        public DbSet<County> County { get; set; }
        public DbSet<District> District { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Region> Region { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<Poll> Poll { get; set; }
        public DbSet<PollChoice> Choice { get; set; }
        public DbSet<PollAnswer> Answer { get; set; }
        public DbSet<Feedback> Feedback { get; set; }
        public DbSet<ProcessReason> Reason { get; set; }
        public DbSet<OrganizationalUnit> OrganizationalUnit { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<FormElement> FormElement { get; set; }
        public DbSet<Violation> Violation { get; set; }
        public DbSet<ViolationType> ViolationType { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<ApplicationLink> ApplicationLink { get; set; }
        public DbSet<QuickAccess> QuickAccess { get; set; }
        public DbSet<Chart> Chart { get; set; }

        public DbSet<GovUserInfo> GovUserInfos { get; set; }
        public DbSet<ShahrbinInstance> ShahrbinInstance { get; set; }

        //Inspection
        public DbSet<Complaint> Complaint { get; set; }
        public DbSet<ComplaintCategory> ComplaintCategory { get; set; }
        public DbSet<ComplaintOrganizationalUnit> ComplaintOrganizationalUnit { get; set; }
        public DbSet<ComplaintLog> ComplaintLog { get; set; }
    }
}
