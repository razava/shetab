using Api.Configurations;
using Domain.Models.Relational;

namespace Api.Services.MemoryCaching;

public interface IStaticSettings
{
    //public AppSettings AppSettings { get; set; }
    //public StaticData<List<Category>> Categories { get; set; }
    //public StaticData<Category> CategoryRoot { get; set; }
    //public StaticData<List<Process>> Processes { get; set; }
    //public StaticData<List<Chart>> Charts { get; set; }
    //public StaticData<List<Stage>> Stages { get; set; }
    //public StaticData<List<Education>> Educations { get; set; }
    //public StaticData<List<Province>> Provinces { get; set; }
    //public StaticData<List<County>> Counties { get; set; }
    //public StaticData<List<District>> Districts { get; set; }
    //public StaticData<List<City>> Cities { get; set; }
    //public StaticData<List<Region>> Regions { get; set; }
    //public StaticData<List<Actor>> Actors { get; set; }
    //public StaticData<List<ApplicationUser>> UserActors { get; set; }
    //public StaticData<List<ApplicationRole>> RoleActors { get; set; }
    //public StaticData<List<BotActor>> BotActors { get; set; }
    //public StaticData<List<ApplicationRole>> Roles { get; set; }
    //public StaticData<SortedList<string, List<ApplicationUser>>> UsersInRoles { get; set; }
    //public StaticData<List<OrganizationalUnit>> OrganizationalUnits { get; set; }

    public List<ShahrbinInstance> Instances { get; set; }
    public SortedList<int, InstanceSettings> InstanceSettings { get; set; }
    public AppSettings AppSettings { get; set; }
    public StaticData<List<ApplicationRole>> Roles { get; set; }
    public StaticData<SortedList<string, List<ApplicationUser>>> UsersInRoles { get; set; }
    public StaticData<List<Province>> Provinces { get; set; }
    public StaticData<List<County>> Counties { get; set; }
    public StaticData<List<District>> Districts { get; set; }
    public StaticData<List<City>> Cities { get; set; }
    public StaticData<List<Region>> Regions { get; set; }
    public StaticData<List<ViolationType>> ViolationTypes { get; set; }

    public void LoadCategories(int instanceId);
    public void LoadProcesses(int instanceId);
    public void LoadCharts(int instanceId);
    public void LoadStages(int instanceId);
    public void LoadActors(int instanceId);
    public void LoadUsersAndRoles();
    public void LoadOrganizationalUnits(int instanceId);
    public void Reload(int instanceId);
    public void LoadProvinces();
    public void LoadCounties();
    public void LoadDistricts();
    public void LoadCities();
    public void LoadRegions();
    public void LoadViolationTypes();
    public void LoadAll();
}
