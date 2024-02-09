using Domain.Models.Gov;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.IdentityAggregate;
using Domain.Models.Relational.PollAggregate;
using Domain.Models.Relational.ProcessAggregate;
using Domain.Models.Relational.ReportAggregate;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(builder);
    }
    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.LogTo(Console.WriteLine); 
    //public DbSet<Media> Media { get; set; }
    public DbSet<Upload> Upload { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<Process> Process { get; set; }
    public DbSet<ProcessStage> ProcessStage { get; set; }
    public DbSet<ProcessTransition> ProcessTransition { get; set; }
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
    public DbSet<PollChoice> PollChoice { get; set; }
    public DbSet<PollAnswer> PollAnswer { get; set; }
    public DbSet<Feedback> Feedback { get; set; }
    public DbSet<ProcessReason> ProcessReason { get; set; }
    public DbSet<OrganizationalUnit> OrganizationalUnit { get; set; }
    public DbSet<Comment> Comment { get; set; }
    public DbSet<FormElement> FormElement { get; set; }
    public DbSet<Violation> Violation { get; set; }
    public DbSet<ViolationType> ViolationType { get; set; }
    public DbSet<News> News { get; set; }
    public DbSet<ApplicationLink> ApplicationLink { get; set; }
    public DbSet<QuickAccess> QuickAccess { get; set; }
    public DbSet<Chart> Chart { get; set; }
    public DbSet<Faq> Faq { get; set; }

    public DbSet<GovUserInfo> GovUserInfos { get; set; }
    public DbSet<ShahrbinInstance> ShahrbinInstance { get; set; }
    public DbSet<ExecutiveContractor> ExecutiveContractor { get; set; }
    public DbSet<Form> Form { get; set; }

    /*
    //Inspection
    public DbSet<Complaint> Complaint { get; set; }
    public DbSet<ComplaintCategory> ComplaintCategory { get; set; }
    public DbSet<ComplaintOrganizationalUnit> ComplaintOrganizationalUnit { get; set; }
    public DbSet<ComplaintLog> ComplaintLog { get; set; }
    */
}
