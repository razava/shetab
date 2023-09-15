using Microsoft.EntityFrameworkCore;
using Api.Configurations;
using Domain.Models.Relational;
using Domain.Data;
using Api.ExtensionMethods;

namespace Api.Services.MemoryCaching;

public class StaticSettings : IStaticSettings
{
    private IServiceScopeFactory scopeFactory;

    public List<ShahrbinInstance> Instances { get; set; } = new List<ShahrbinInstance>();
    public SortedList<int, InstanceSettings> InstanceSettings { get; set; } = new SortedList<int, InstanceSettings>();
    public AppSettings AppSettings { get; set; } = new AppSettings();
    public StaticData<List<ApplicationRole>> Roles { get; set; } = new StaticData<List<ApplicationRole>>();
    public StaticData<SortedList<string, List<ApplicationUser>>> UsersInRoles { get; set; } = new StaticData<SortedList<string, List<ApplicationUser>>>();
    public StaticData<List<Education>> Educations { get; set; } = new StaticData<List<Education>>();
    public StaticData<List<Province>> Provinces { get; set; } = new StaticData<List<Province>>();
    public StaticData<List<County>> Counties { get; set; } = new StaticData<List<County>>();
    public StaticData<List<District>> Districts { get; set; } = new StaticData<List<District>>();
    public StaticData<List<City>> Cities { get; set; } = new StaticData<List<City>>();
    public StaticData<List<Region>> Regions { get; set; } = new StaticData<List<Region>>();

    public StaticData<List<ViolationType>> ViolationTypes { get; set; } = new StaticData<List<ViolationType>>();

    private ApplicationDbContext context;
    private IServiceScope scope;
    public StaticSettings(IConfiguration configuration, IServiceScopeFactory scopeFactory)
    {
        AppSettings.SmsOptions = configuration.GetSection(SmsOptions.Name).Get<SmsOptions>();
        AppSettings.GeneralSettings = configuration.GetSection(GeneralSettings.Name).Get<GeneralSettings>();
        AppSettings.ParsiMapOptions = configuration.GetSection(ParsiMapOptions.Name).Get<ParsiMapOptions>();
        AppSettings.FirebaseProxyOptions = configuration.GetSection(FirebaseProxyOptions.Name).Get<FirebaseProxyOptions>();
        AppSettings.FeedbackOptions = configuration.GetSection(FeedbackOptions.Name).Get<FeedbackOptions>();
        AppSettings.AppVersions!.AppVersionList = configuration.GetSection(AppVersions.Name).Get<List<AppVersion>>();
        AppSettings.ImageQualityOptions = configuration.GetSection(ImageQualityOptions.Name).Get<ImageQualityOptions>();
        AppSettings.GovOptions = configuration.GetSection(GovOptions.Name).Get<GovOptions>();

        this.scopeFactory = scopeFactory;
        // Create a new scope (since DbContext is scoped by default)
        scope = scopeFactory.CreateScope();

        // Get a Dbcontext from the scope
        context = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        LoadAll();
    }

    public void LoadOrganizationalUnits(int instanceId)
    {
        InstanceSettings[instanceId].OrganizationalUnits.Data = context.OrganizationalUnit
            .Where(p => p.ShahrbinInstanceId == instanceId)
            .Include(p => p.OrganizationalUnits)
            .Include(p => p.ParentOrganizationalUnits)
            .ToList();
        InstanceSettings[instanceId].OrganizationalUnits.LastModified = DateTimeOffset.UtcNow.TrimToSeconds();

    }

    public void LoadCategories(int instanceId)
    {
        var categories = context.Category
            .Where(p => p.ShahrbinInstanceId == instanceId)
            .Where(p => !p.IsDeleted)
            .Include(p => p.FormElements.OrderBy(q => q.Order))
            .OrderBy(p => p.Order)
            .AsNoTracking()
            .ToList();

        var root = categories.Where(p => p.ParentId == null).FirstOrDefault();
        if (root is null)
            return;
        foreach (var category in categories)
        {
            var children = categories.Where(p => p.ParentId == category.Id).ToList();
            category.Categories = children;
            children.ForEach(q => { q.Parent = category; });
        }

        InstanceSettings[instanceId].Categories.Data = categories;
        InstanceSettings[instanceId].Categories.LastModified = DateTimeOffset.UtcNow.TrimToSeconds();

        InstanceSettings[instanceId].CategoryRoot.Data = root;
        InstanceSettings[instanceId].CategoryRoot.LastModified = InstanceSettings[instanceId].Categories.LastModified;
    }

    public void LoadProcesses(int instanceId)
    {

        InstanceSettings[instanceId].Processes.Data = context.Process
            .Where(p => p.ShahrbinInstanceId == instanceId)
            .Include(p => p.Stages)
            .ThenInclude(p => p.Actors)
            .ThenInclude(p => p.Regions)
            .Include(p => p.Transitions)
            .ThenInclude(p => p.ReasonList)
            .Include(p => p.Transitions)
            .ThenInclude(p => p.From)
            .ThenInclude(p => p.DisplayRole)
            .Include(p => p.Transitions)
            .ThenInclude(p => p.From)
            .ThenInclude(p => p.Actors)
            .ThenInclude(p => p.Regions)
            .Include(p => p.Transitions)
            .ThenInclude(p => p.To)
            .ThenInclude(p => p.DisplayRole)
            .Include(p => p.Transitions)
            .ThenInclude(p => p.To)
            .ThenInclude(p => p.Actors)
            .ThenInclude(p => p.Regions)
            .Include(p => p.RevisionUnit)
            .AsNoTracking()
            .ToList();
        InstanceSettings[instanceId].Processes.LastModified = DateTimeOffset.UtcNow.TrimToSeconds();
    }

    public void LoadCharts(int instanceId)
    {

        InstanceSettings[instanceId].Charts.Data = context.Chart
            .Where(p => p.ShahrbinInstanceId == instanceId)
            .Include(p => p.Roles)
            .AsNoTracking()
            .ToList();
        InstanceSettings[instanceId].Charts.LastModified = DateTimeOffset.UtcNow.TrimToSeconds();
    }

    public void LoadStages(int instanceId)
    {
        InstanceSettings[instanceId].ProcessStages.Data = context.Stage
            //.Where(p => p.ShahrbinInstanceId == instanceId)       //TODO: This should be fixed. Now all instances include all stages which is redundant
            .Include(p => p.Actors)
            .AsNoTracking()
            .ToList();
        InstanceSettings[instanceId].ProcessStages.LastModified = DateTimeOffset.UtcNow.TrimToSeconds();
    }

    public void LoadProvinces()
    {

        var faComparer = StringComparer.Create(new System.Globalization.CultureInfo("fa-IR"), true);
        var result = context.Province
            .AsNoTracking()
            .ToList();
        Provinces.Data = result.OrderBy(p => p.Name, faComparer).ToList();
        Provinces.LastModified = DateTimeOffset.UtcNow.TrimToSeconds();
    }

    public void LoadCounties()
    {

        var faComparer = StringComparer.Create(new System.Globalization.CultureInfo("fa-IR"), true);
        var result = context.County
            .AsNoTracking()
            .ToList();
        Counties.Data = result.OrderBy(p => p.Name, faComparer).ToList();
        Counties.LastModified = DateTimeOffset.UtcNow.TrimToSeconds();
    }

    public void LoadDistricts()
    {

        var faComparer = StringComparer.Create(new System.Globalization.CultureInfo("fa-IR"), true);
        var result = context.District
            .AsNoTracking()
            .ToList();
        Districts.Data = result.OrderBy(p => p.Name, faComparer).ToList();
        Districts.LastModified = DateTimeOffset.UtcNow.TrimToSeconds();
    }

    public void LoadCities()
    {

        var faComparer = StringComparer.Create(new System.Globalization.CultureInfo("fa-IR"), true);
        var result = context.City
            .AsNoTracking()
            .ToList();
        Cities.Data = result.OrderBy(p => p.Name, faComparer).ToList();
        Cities.LastModified = DateTimeOffset.UtcNow.TrimToSeconds();
    }

    public void LoadRegions()
    {

        var faComparer = StringComparer.Create(new System.Globalization.CultureInfo("fa-IR"), true);
        var result = context.Region
            .Include(p => p.City)
            .AsNoTracking()
            .ToList();
        Regions.Data = result.OrderBy(p => p.Name, faComparer).ToList();
        Regions.LastModified = DateTimeOffset.UtcNow.TrimToSeconds();
    }

    public void LoadViolationTypes()
    {

        ViolationTypes.Data = context.ViolationType
            .AsNoTracking()
            .ToList();
        ViolationTypes.LastModified = DateTimeOffset.UtcNow.TrimToSeconds();
    }

    public void LoadActors(int instanceId)
    {
        InstanceSettings[instanceId].Actors.Data = context.Actor
            //.Where(p => p.ShahrbinInstanceId == instanceId)           //TODO: This should be fixed. Now all instances include all actors which is redundant
            .Include(p => p.Regions)
            .Include(p => p.Stages)
            .AsNoTracking()
            .ToList();
        InstanceSettings[instanceId].Actors.LastModified = DateTimeOffset.UtcNow.TrimToSeconds();

        var userActorIdentifiers = InstanceSettings[instanceId].Actors.Data.Where(p => p.Type == ActorType.Person).Select(p => p.Identifier).ToList();
        var roleActorIdentifiers = InstanceSettings[instanceId].Actors.Data.Where(p => p.Type == ActorType.Role).Select(p => p.Identifier).ToList();
        InstanceSettings[instanceId].UserActors.Data = context.Users.Where(p => userActorIdentifiers.Contains(p.Id)).AsNoTracking().ToList();
        InstanceSettings[instanceId].RoleActors.Data = context.Roles.Where(p => roleActorIdentifiers.Contains(p.Id)).AsNoTracking().ToList();
        InstanceSettings[instanceId].UserActors.LastModified = DateTimeOffset.UtcNow.TrimToSeconds();
        InstanceSettings[instanceId].RoleActors.LastModified = DateTimeOffset.UtcNow.TrimToSeconds();

        InstanceSettings[instanceId].BotActors.Data = context.BotActors
                .Include(p => p.Actors)
                .Include(p => p.Transition)
                .ToList();
        InstanceSettings[instanceId].BotActors.LastModified = DateTimeOffset.UtcNow.TrimToSeconds();
    }

    public void LoadUsersAndRoles()
    {
        Roles.Data = context.Roles.AsNoTracking().ToList();
        Roles.LastModified = DateTimeOffset.UtcNow.TrimToSeconds();

        UsersInRoles.Data = new SortedList<string, List<ApplicationUser>>();
        UsersInRoles.LastModified = DateTimeOffset.UtcNow.TrimToSeconds();
        foreach (var role in Roles.Data)
        {
            var userIds = context.UserRoles.Where(p => p.RoleId == role.Id).Select(p => p.UserId).AsNoTracking().ToList();
            var users = context.Users.Where(p => userIds.Contains(p.Id)).Include(p => p.Contractors).Include(p => p.Executeves).AsNoTracking().ToList();
            UsersInRoles.Data.Add(role.Name ?? "", users);
        }
    }

    public void Reload(int instanceId)
    {
        LoadCategories(instanceId);
        LoadProcesses(instanceId);
        LoadCharts(instanceId);
        LoadStages(instanceId);
        LoadActors(instanceId);
        LoadOrganizationalUnits(instanceId);
    }

    public void LoadAll()
    {
        LoadProvinces();
        LoadCounties();
        LoadDistricts();
        LoadCities();
        LoadRegions();
        LoadViolationTypes();
        LoadUsersAndRoles();

        Instances = context.ShahrbinInstance.ToList();

        InstanceSettings.Clear();
        foreach (var instance in Instances)
        {
            InstanceSettings.Add(instance.Id, new InstanceSettings() { ShahrbinInstance = instance });
            Reload(instance.Id);
        }
    }
}

public class InstanceSettings
{
    public ShahrbinInstance ShahrbinInstance { get; set; } = null!;
    //public AppSettings AppSettings { get; set; } = new AppSettings();
    public StaticData<List<Category>> Categories { get; set; } = new StaticData<List<Category>>();
    public StaticData<Category> CategoryRoot { get; set; } = new StaticData<Category>();
    public StaticData<List<Process>> Processes { get; set; } = new StaticData<List<Process>>();
    public StaticData<List<Chart>> Charts { get; set; } = new StaticData<List<Chart>>();
    public StaticData<List<ProcessStage>> ProcessStages { get; set; } = new StaticData<List<ProcessStage>>();
    public StaticData<List<Actor>> Actors { get; set; } = new StaticData<List<Actor>>();
    public StaticData<List<ApplicationUser>> UserActors { get; set; } = new StaticData<List<ApplicationUser>>();
    public StaticData<List<ApplicationRole>> RoleActors { get; set; } = new StaticData<List<ApplicationRole>>();
    public StaticData<List<BotActor>> BotActors { get; set; } = new StaticData<List<BotActor>>();
    public StaticData<List<OrganizationalUnit>> OrganizationalUnits { get; set; } = new StaticData<List<OrganizationalUnit>>();
}
