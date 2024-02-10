using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using ClosedXML.Excel;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.IdentityAggregate;
using Domain.Models.Relational.ProcessAggregate;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace Application.Setup.Commands;

internal sealed class AddInstanceCommandHandler : IRequestHandler<AddInstanceCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IProcessRepository _processRepository;
    private readonly DbContext _context;
    public AddInstanceCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IProcessRepository processRepository)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _context = _unitOfWork.DbContext;
        _processRepository = processRepository;
    }
    public async Task<bool> Handle(AddInstanceCommand request, CancellationToken cancellationToken)
    {
        foreach(var file in request.files)
        {
            await addInstance(file);
        }

        return true;
    }

    public class InputForExecutive
    {
        public string Code { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool IsPerRegion { get; set; }
        public List<Region> Regions { get; set; } = new List<Region>();
    }

    public class InputForCategory
    {
        public string Title { get; set; } = string.Empty;
        public string ProcessTitle { get; set; } = string.Empty;
        public int Order { get; set; }
        public string Code { get; set; } = string.Empty;
        public int ResponseDuration { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool ObjectionAllowed { get; set; }
        public string RoleId { get; set; } = string.Empty;
        public List<InputForCategory> SubCategories { get; set; } = new List<InputForCategory>();
    }

    private async Task addInstance(IFormFile file)
    {
        var inputForExecutives = new List<InputForExecutive>();
        var inputForCategories = new List<InputForCategory>();

        //Read excel file
        using var workbook = new XLWorkbook(file.OpenReadStream());
        var ws = workbook.Worksheet(1);

        await initGlobals();
        var inputInfo = new
        {
            CityName = ws.Cell(2, 2).Value.ToString(),
            Name = ws.Cell(2, 1).Value.ToString(),
            Abbreviation = ws.Cell(2, 4).Value.ToString(),
            Description = ws.Cell(2, 3).Value.ToString(),
            EnglishName = ws.Cell(2, 5).Value.ToString(),
            Longitude = double.Parse(ws.Cell(2, 6).Value.ToString()),
            Latitude = double.Parse(ws.Cell(2, 7).Value.ToString())
        };
        
        var city = await _context.Set<City>().Where(p => p.Name == inputInfo.CityName).FirstOrDefaultAsync();
        if (city == null)
        {
            throw new InvalidCityException($"Invalid city: {inputInfo.CityName}");
        }
        var instance = new ShahrbinInstance()
        {
            City = city,
            Name = inputInfo.Name,
            Description = inputInfo.Description,
            Abbreviation = inputInfo.Abbreviation,
            EnglishName = inputInfo.EnglishName,
            Longitude = inputInfo.Longitude,
            Latitude = inputInfo.Latitude,
        };

        if (_context.Set<ShahrbinInstance>().Any(p => p.Name == instance.Name || p.CityId == city.Id))
        {
            throw new DuplicateInstanceException($"Duplicate instance: {instance.Name}, {city.Name}");
        }
        _context.Set<ShahrbinInstance>().Add(instance);
        await _context.SaveChangesAsync();

        var regions = await _context.Set<Region>()
            .Where(p => p.CityId == instance.CityId)
            .ToListAsync();
        await initDefaultsFromExcel(regions, instance);

        var citizenRoleId = await _context.Set<ApplicationRole>()
            .Where(r => r.Name == RoleNames.Citizen)
            .Select(r => r.Id)
            .SingleOrDefaultAsync();
        if (citizenRoleId is null) 
        {
            throw new Exception("No citizen role exists.");
        }
        InputForCategory? inputForCategory = null;
        ws = workbook.Worksheet(2);
        for (int row = 2; row <= ws.LastRowUsed().RowNumber(); row++)
        {
            if (!string.IsNullOrEmpty(ws.Cell(row, 1).Value.ToString()))
            {
                inputForCategory = new InputForCategory()
                {
                    Title = ws.Cell(row, 1).Value.ToString().Trim(),
                    Order = int.Parse(ws.Cell(row, 5).Value.ToString().Trim()),
                    Code = (inputForCategories.Count * 1000).ToString(),
                    Description = "",
                    Duration = 0,
                    ResponseDuration = 0,
                    ProcessTitle = "",
                    ObjectionAllowed = true,
                    SubCategories = new List<InputForCategory>(),
                    RoleId = citizenRoleId
                };
                inputForCategories.Add(inputForCategory);
            }
            else
            {
                if(inputForCategory is null)
                {
                    throw new ForbidNullParentCategoryException();
                }
                inputForCategory.SubCategories.Add(new InputForCategory()
                {
                    Title = ws.Cell(row, 2).Value.ToString().Trim(),
                    Code = (int.Parse(inputForCategory.Code) + inputForCategories.Count).ToString(),
                    Description = ws.Cell(row, 12).Value.ToString().Trim(),
                    Duration = int.Parse(ws.Cell(row, 11).Value.ToString().Trim()),
                    ResponseDuration = int.Parse(ws.Cell(row, 10).Value.ToString().Trim()),
                    ProcessTitle = $"{instance.Abbreviation}-p-{ws.Cell(row, 7).Value.ToString()}",
                    ObjectionAllowed = true,
                    Order = int.Parse(ws.Cell(row, 5).Value.ToString().Trim()),
                    RoleId = citizenRoleId
                });

                var code = $"{instance.Abbreviation}-p-{ws.Cell(row, 7).Value.ToString()}";
                if (!inputForExecutives.Any(p => p.Code == code))
                {
                    inputForExecutives.Add(new InputForExecutive()
                    {
                        Code = code,
                        Title = $"{ws.Cell(row, 6).Value.ToString()}",
                        UserName = $"{instance.Abbreviation}-e-{ws.Cell(row, 7).Value.ToString()}",
                        FirstName = $"{ws.Cell(row, 8).Value.ToString()}",
                        LastName = $"{ws.Cell(row, 9).Value.ToString()}",
                        Password = "aA@12345",
                        IsPerRegion = false
                    });
                }
            }
        }

        await initProcessesFromExcel(instance, inputForExecutives);
        await initCategoriesFromExcel(instance, inputForCategories);
        await initCharts(instance.Id);
    }

    private async Task initGlobals()
    {
        //if ((await _context.Set<Education>().CountAsync()) > 0)
        //    return;

        //await initEducation();
        //await initPriority();

        await initAdministrativeDivisions();
    }

    private async Task initAdministrativeDivisions()
    {
        #region Make Json
        //var provinces = new List<Models.Province>();
        //foreach(var p in Iran.Provinces)
        //{
        //    var province = _mapper.Map<Models.Province>(p);
        //    provinces.Add(province);
        //}

        //provinces.Sort((a, b) => a.Name.CompareTo(b.Name));
        //foreach(var p in provinces)
        //{
        //    p.Counties.Sort((a, b) => a.Name.CompareTo(b.Name));
        //    foreach (var c in p.Counties)
        //    {
        //        c.Districts.Sort((a, b) => a.Name.CompareTo(b.Name));
        //        foreach (var d in c.Districts)
        //        {
        //            d.Cities.Sort((a, b) => a.Name.CompareTo(b.Name));
        //            foreach (var ci in d.Cities)
        //            {
        //                ci.Regions = new List<Region>() { new Region() { Name = "همه" } };
        //            }
        //        }
        //    }
        //}

        //var options = new JsonSerializerOptions
        //{
        //    WriteIndented = true,
        //    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        //};
        //var jsonString = JsonSerializer.Serialize(provinces, options);

        //System.IO.File.WriteAllText("AdministrativeDivisions", jsonString, System.Text.Encoding.Unicode);
        #endregion
        if (_context.Set<Province>().Count() > 0)
            return;

        var jsonString = File.ReadAllText("AdministrativeDivisions", System.Text.Encoding.Unicode);
        var provinces = JsonSerializer.Deserialize<List<Province>>(jsonString);
        if (provinces is null || provinces.Count == 0)
            return;
        _context.Set<Province>().AddRange(provinces);
        await _context.SaveChangesAsync();
    }

    private async Task<bool> initProcessesFromExcel(ShahrbinInstance instance, List<InputForExecutive> inputForExecutives)
    {
        ApplicationUser user;
        Actor actor;
        var regions = await _context.Set<Region>()
            .Where(p => p.CityId == instance.CityId)
            .ToListAsync();
        InputForExecutive inputForExecutive;

        if (_context.Set<Process>().Count(p => p.ShahrbinInstanceId == instance.Id) == 0)
        {
            for (var i = 0; i < inputForExecutives.Count; i++)
            {
                inputForExecutive = inputForExecutives[i];

                //Create process related to an executive

                var actors = new List<Actor>();
                if (!inputForExecutive.IsPerRegion)
                {
                    user = new ApplicationUser()
                    {
                        UserName = $"{inputForExecutive.UserName}",
                        Title = inputForExecutive.Title,
                        PhoneNumberConfirmed = true,
                        ShahrbinInstanceId = instance.Id,
                    };

                    await _userRepository.CreateAsync(user, inputForExecutive.Password);

                    await _userRepository.AddToRolesAsync(user, new string[] { "Executive" });
                    actor = new Actor
                    {
                        Identifier = user.Id,
                        Type = ActorType.Person,
                        Regions = regions
                    };
                    actors.Add(actor);
                }
                else
                {
                    foreach (var region in regions)
                    {
                        user = new ApplicationUser()
                        {
                            UserName = $"{inputForExecutive.UserName}-m{region.Code}",
                            Title = inputForExecutive.Title + " " + "منطقه" + " " + region.Name,
                            PhoneNumberConfirmed = true,
                            ShahrbinInstanceId = instance.Id,
                        };

                        await _userRepository.CreateAsync(user, inputForExecutive.Password);

                        await _userRepository.AddToRolesAsync(user, new string[] { "Executive" });
                        actor = new Actor()
                        {
                            Identifier = user.Id,
                            Type = ActorType.Person,
                        };
                        actor.Regions.Add(region);
                        actors.Add(actor);
                    }
                }
                _context.Set<Actor>().AddRange(actors);
                await _context.SaveChangesAsync();

                await _processRepository.AddTypicalProcess(
                    instance.Id, 
                    $"{inputForExecutive.Code}",
                    inputForExecutive.Title, 
                    actors.Select(p => p.Id).ToList());
                await _unitOfWork.SaveAsync();
            }
        }

        return true;
    }

    private async Task initDefaultsFromExcel(List<Region> regions, ShahrbinInstance instance)
    {
        //Init roles
        var rolesInfo = new List<Tuple<string, string, List<string>>>()
            {
                new Tuple<string, string, List<string>>("Citizen", "شهروند",
                new List<string>
                {
                    AppClaimTypes.Report.Create.ByCitizen,
                    AppClaimTypes.Report.Read.Self,
                    AppClaimTypes.Report.Read.Nearest,
                    AppClaimTypes.Report.Read.Recent,
                    AppClaimTypes.Report.Read.Comments,
                    AppClaimTypes.Report.Read.History,
                    AppClaimTypes.Report.Like,
                    AppClaimTypes.Comments.Create,
                    AppClaimTypes.Comments.Delete,
                    AppClaimTypes.Account.Get,
                    AppClaimTypes.Account.Update,
                    AppClaimTypes.Category.Read,
                    AppClaimTypes.File.Upload,
                    AppClaimTypes.News.Read.ByCitizen,
                    AppClaimTypes.Polls.Answer,
                    AppClaimTypes.Polls.Read.ByCitizen,
                    AppClaimTypes.Messages.Read,
                    AppClaimTypes.Feedback.Create,
                    AppClaimTypes.Violation.Create.ForReport,
                    AppClaimTypes.Violation.Create.ForComment
                }),
                new Tuple<string, string, List<string>>("Operator", "اپراتور",
                new List<string>
                {
                    AppClaimTypes.Report.Create.ByOperator,
                    AppClaimTypes.Report.Read.All,
                    AppClaimTypes.Report.Read.Details,
                    AppClaimTypes.Report.Read.History,
                    AppClaimTypes.Report.Read.Comments,
                    AppClaimTypes.Report.Update,
                    AppClaimTypes.Report.Accept,
                    AppClaimTypes.Task.Read,
                    AppClaimTypes.Task.MakeTransition,
                    AppClaimTypes.Comments.Read,
                    AppClaimTypes.Comments.Reply,
                    AppClaimTypes.Comments.Update,
                    AppClaimTypes.Comments.Delete,
                    AppClaimTypes.Account.Get,
                    AppClaimTypes.Account.Update,
                    AppClaimTypes.Info.Read,
                    //AppClaimTypes.Common.Read.Executives,
                    AppClaimTypes.File.Upload,
                    AppClaimTypes.Satisfaction.Update,
                    AppClaimTypes.Violation.Read,
                    AppClaimTypes.Violation.Update,
                }),
                new Tuple<string, string, List<string>>("Executive", "واحد اجرایی",
                new List<string>
                {
                    AppClaimTypes.Report.Read.All,
                    AppClaimTypes.Report.Read.Details,
                    AppClaimTypes.Report.Read.History,
                    AppClaimTypes.Report.Read.Comments,
                    AppClaimTypes.Task.Read,
                    AppClaimTypes.Task.MakeTransition,
                    AppClaimTypes.Task.MessageToCitizen,
                    AppClaimTypes.Account.Get,
                    AppClaimTypes.Account.Update,
                    AppClaimTypes.Contractor.Get,
                    AppClaimTypes.Contractor.Create,
                    AppClaimTypes.Info.Read,
                    AppClaimTypes.File.Upload,
                    AppClaimTypes.Feedback.Read,
                    AppClaimTypes.Satisfaction.Read,
                    AppClaimTypes.Violation.Read
                }),
                new Tuple<string, string, List<string>>("Contractor", "پیمانکار",
                new List<string>
                {
                    AppClaimTypes.Report.Read.All,
                    AppClaimTypes.Report.Read.Details,
                    AppClaimTypes.Report.Read.History,
                    AppClaimTypes.Report.Read.Comments,
                    AppClaimTypes.Task.Read,
                    AppClaimTypes.Task.MakeTransition,
                    AppClaimTypes.Account.Get,
                    AppClaimTypes.Account.Update,
                    AppClaimTypes.File.Upload,
                    AppClaimTypes.Feedback.Read,
                    AppClaimTypes.Satisfaction.Read,
                    AppClaimTypes.Violation.Read
                }),
                new Tuple<string, string, List<string>>("Manager", "واحد سازمانی",
                new List<string>
                {
                    AppClaimTypes.Report.Read.All,
                    AppClaimTypes.Report.Read.Details,
                    AppClaimTypes.Report.Read.History,
                    AppClaimTypes.Report.Read.Comments,
                    AppClaimTypes.Task.Read,
                    AppClaimTypes.Task.MakeTransition,
                    AppClaimTypes.Account.Get,
                    AppClaimTypes.Account.Update,
                    AppClaimTypes.Info.Read,
                    AppClaimTypes.Common.Read.Executives,
                    AppClaimTypes.File.Upload,
                    AppClaimTypes.Feedback.Read,
                    AppClaimTypes.Satisfaction.Read,
                    AppClaimTypes.Violation.Read

                }),
                new Tuple<string, string, List<string>>("Advertiser", "مدیر تبلیغات",
                new List<string>
                {
                    //todo : compelete
                }),
                new Tuple<string, string, List<string>>("Mayor", "شهردار",
                new List<string>
                {
                    AppClaimTypes.Report.Read.All,
                    AppClaimTypes.Report.Read.Details,
                    AppClaimTypes.Report.Read.History,
                    AppClaimTypes.Report.Read.Comments,
                    AppClaimTypes.Account.Get,
                    AppClaimTypes.Account.Update,
                    AppClaimTypes.Info.Read,
                    AppClaimTypes.Feedback.Read,
                    AppClaimTypes.Satisfaction.Read,
                    AppClaimTypes.Violation.Read
                }),
                new Tuple<string, string, List<string>>("Admin", "مدیر فنی",
                new List<string>
                {
                    AppClaimTypes.Account.Get,
                    AppClaimTypes.Account.Update,
                    AppClaimTypes.User.Create,
                    AppClaimTypes.User.Create,
                    AppClaimTypes.User.Read.All,
                    AppClaimTypes.User.Read.Roles,
                    AppClaimTypes.User.Read.Regions,
                    AppClaimTypes.User.Read.Contractors,
                    AppClaimTypes.User.Update.Details,
                    AppClaimTypes.User.Update.Password,
                    AppClaimTypes.User.Update.Roles,
                    AppClaimTypes.User.Update.Regions,
                    AppClaimTypes.Info.Read,
                    AppClaimTypes.Category.Read,
                    AppClaimTypes.Category.Manage,
                    AppClaimTypes.Processes.Read,
                    AppClaimTypes.Processes.Manage,
                    AppClaimTypes.OrganizationalUnit.Read,
                    AppClaimTypes.OrganizationalUnit.Manage,
                    AppClaimTypes.File.Upload,
                    AppClaimTypes.QuickAccess.Read.ByAdmin,
                    AppClaimTypes.QuickAccess.Manage,
                    AppClaimTypes.News.Read.ByAdmin,
                    AppClaimTypes.News.Manage,
                    AppClaimTypes.Polls.Read.ByAdmin,
                    AppClaimTypes.Polls.Read.Summary,
                    AppClaimTypes.Polls.Manage,

                }),
                new Tuple<string, string, List<string>>("PowerUser","",
                new List<string>
                {
                    //todo : compelete
                }),
                new Tuple<string, string, List<string>>("Inspector", "بازرس",
                new List<string>
                {
                    AppClaimTypes.Report.Read.All,
                    AppClaimTypes.Report.Read.Details,
                    AppClaimTypes.Report.Read.History,
                    AppClaimTypes.Report.Read.Comments,
                    //AppClaimTypes.Report.Update,
                    AppClaimTypes.Task.Read,
                    AppClaimTypes.Task.MakeTransition,
                    AppClaimTypes.Task.Review,
                    AppClaimTypes.Account.Get,
                    AppClaimTypes.Account.Update,
                    AppClaimTypes.Info.Read,
                    AppClaimTypes.File.Upload,
                    AppClaimTypes.Feedback.Read,
                    AppClaimTypes.Satisfaction.Read,
                    AppClaimTypes.Violation.Read
                })
            };


        var roles = new List<ApplicationRole>();
        foreach (var role in rolesInfo)
        {
            if (!await _userRepository.RoleExistsAsync(role.Item1))
            {
                var r = new ApplicationRole() { Name = role.Item1, Title = role.Item2 };
                await _userRepository.CreateRoleAsync(r);
                foreach (var claim in role.Item3)
                {
                    await _userRepository.AddClaimsToRoleAsunc(r, new Claim(claim, "TRUE"));
                }
                roles.Add(r);
            }
        }

        //Init users
        //TODO: exclude non-actor users like poweruser, admin, ...
        var usersInfo = new List<Tuple<string, string, string>>()
            {
                new Tuple<string, string, string>("Operator", $"{instance.Abbreviation}-Operator", $"اپراتور"),
                new Tuple<string, string, string>("Advertiser", $"{instance.Abbreviation}-Advertiser", $"مدیر تبلیغات"),
                new Tuple<string, string, string>("Mayor", $"{instance.Abbreviation}-Shahrdar", $"شهردار"),
                new Tuple<string, string, string>("Admin", $"{instance.Abbreviation}-Admin", $"مدیر فنی"),
                new Tuple<string, string, string>("PowerUser", "PowerUser", ""),
                new Tuple<string, string, string>("Inspector", $"{instance.Abbreviation}-Inspector", $"بازرس")
        };

        ApplicationUser u;
        var users = new List<ApplicationUser>();

        foreach (var user in usersInfo)
        {
            if (await _userRepository.FindByNameAsync(user.Item2) == null)
            {
                u = new ApplicationUser()
                {
                    UserName = user.Item2,
                    Title = user.Item3,
                    PhoneNumberConfirmed = true,
                    ShahrbinInstanceId = instance.Id,
                };

                await _userRepository.CreateAsync(u, "aA@12345");

                await _userRepository.AddToRoleAsync(u, user.Item1);
                users.Add(u);
            }
        }

        //Add created roles and predefined users as Actors
        foreach (var roleId in roles.Select(p => p.Id).ToList())
        {
            var actor = new Actor()
            {
                Identifier = roleId,
                //Regions = regions,
                //Reports = null,
                Type = ActorType.Role
            };
            _context.Set<Actor>().Add(actor);
        }

        foreach (var userId in users.Select(p => p.Id).ToList())
        {
            var actor = new Actor()
            {
                Identifier = userId,
                Regions = regions,
                //Reports = null,
                Type = ActorType.Person
            };
            _context.Set<Actor>().Add(actor);
        }

        //Add mayors for each region
        if (regions.Count > 1)
        {
            foreach (var region in regions)
            {
                u = new ApplicationUser()
                {
                    UserName = $"{instance.Abbreviation}-shahrdar-m{region.Code}",
                    Title = "شهردار " + region.Name,
                    PhoneNumberConfirmed = true,
                    ShahrbinInstanceId = instance.Id,
                };

                await _userRepository.CreateAsync(u, "aA@12345");

                await _userRepository.AddToRolesAsync(u, new string[] { "Mayor" });
                var actor = new Actor()
                {
                    Identifier = u.Id,
                    Type = ActorType.Person,
                };
                actor.Regions.Add(region);
                _context.Set<Actor>().Add(actor);
            }
        }
        await _context.SaveChangesAsync();


        // Init Violation types
        var violationTypes = new List<ViolationType>()
            {
                new ViolationType(){ Title = "نقض حریم خصوصی", Code = 1, Threshold = 5},
                new ViolationType(){ Title = "حاوی کلمات و عبارات نامناسب", Code = 2, Threshold = 5},
                new ViolationType(){ Title = "حاوی تصاویر نامناسب", Code = 3, Threshold = 5}
            };
        foreach (var violationType in violationTypes)
        {
            if (!await _context.Set<ViolationType>().AnyAsync(p => p.Code == violationType.Code))
            {
                _context.Set<ViolationType>().Add(violationType);
            }
        }
        await _context.SaveChangesAsync();
    }

    private async Task<bool> initCategoriesFromExcel(ShahrbinInstance instance, List<InputForCategory> inputForCategories)
    {
        var rootCategory = new Category()
        {
            ShahrbinInstanceId = instance.Id,
            ParentId = null,
            CategoryType = CategoryType.Root,
            Title = "ریشه",
            Code = "-1",
            RoleId = inputForCategories[0].RoleId
        };
        _context.Set<Category>().Add(rootCategory);
        await _context.SaveChangesAsync();

        Category parentCategory;
        Category category;

        var processes = await _context.Set<Process>().ToListAsync();
        foreach (var parentCat in inputForCategories)
        {
            parentCategory = new Category()
            {
                ShahrbinInstanceId = instance.Id,
                Title = parentCat.Title,
                Code = parentCat.Code,
                Order = parentCat.Order,
                ParentId = rootCategory.Id,
                RoleId = parentCat.RoleId
            };
            foreach (var childCat in parentCat.SubCategories)
            {
                var process = processes.Where(p => p.Code == childCat.ProcessTitle).FirstOrDefault();
                if (process is null)
                    throw new ForbidNullProcessException();

                category = new Category()
                {
                    ShahrbinInstanceId = instance.Id,
                    Title = childCat.Title,
                    Order = childCat.Order,
                    Code = childCat.Code,
                    ProcessId = process.Id,
                    Duration = childCat.Duration,
                    ResponseDuration = childCat.ResponseDuration,
                    ObjectionAllowed = childCat.ObjectionAllowed,
                    Description = childCat.Description,
                    RoleId = childCat.RoleId
                };
                parentCategory.Categories.Add(category);
            }
            _context.Set<Category>().Add(parentCategory);
        }

        await _context.SaveChangesAsync();
        return true;
    }

    private async Task<bool> initCharts(int instanceId)
    {
        var citizenRole = await _userRepository.FindRoleByNameAsync("Citizen");
        var operatorRole = await _userRepository.FindRoleByNameAsync("Operator");
        var executiveRole = await _userRepository.FindRoleByNameAsync("Executive");
        var contractorRole = await _userRepository.FindRoleByNameAsync("Contractor");
        var inspectorRole = await _userRepository.FindRoleByNameAsync("Inspector");
        var adminRole = await _userRepository.FindRoleByNameAsync("Admin");
        var managerRole = await _userRepository.FindRoleByNameAsync("Manager");
        var mayorRole = await _userRepository.FindRoleByNameAsync("Mayor");

        if (citizenRole is null
            || operatorRole is null
            || executiveRole is null
            || contractorRole is null
            || inspectorRole is null
            || adminRole is null
            || managerRole is null
            || mayorRole is null)
            throw new ForbidNullRolesException();

        var chartCodes = await _context.Set<Chart>().Select(p => p.Code).ToListAsync();
        Chart chart;
        int code;

        code = 1;
        if (!chartCodes.Contains(code))
        {
            chart = new Chart()
            {
                Code = instanceId * 100000 + code,
                Order = 1,
                Title = "آمار و اطلاعات کاربران",
                Roles = new List<ApplicationRole>() { adminRole, managerRole, mayorRole },
                ShahrbinInstanceId = instanceId,
            };
            _context.Set<Chart>().Add(chart);
        }

        code = 2;
        if (!chartCodes.Contains(code))
        {
            chart = new Chart()
            {
                Code = instanceId * 100000 + code,
                Order = 1,
                Title = "آمار و اطلاعات درخواست ها",
                Roles = new List<ApplicationRole>() { operatorRole, executiveRole, contractorRole, inspectorRole, managerRole, mayorRole },
                ShahrbinInstanceId = instanceId,
            };
            _context.Set<Chart>().Add(chart);
        }

        code = 3;
        if (!chartCodes.Contains(code))
        {
            chart = new Chart()
            {
                Code = instanceId * 100000 + code,
                Order = 1,
                Title = "آمار و اطلاعات زمانی",
                Roles = new List<ApplicationRole>() { operatorRole, executiveRole, contractorRole, inspectorRole, managerRole, mayorRole },
                ShahrbinInstanceId = instanceId,
            };
            _context.Set<Chart>().Add(chart);
        }

        code = 101;
        if (!chartCodes.Contains(code))
        {
            chart = new Chart()
            {
                Code = instanceId * 100000 + code,
                Order = 1,
                Title = "وضعیت درخواست ها بر اساس زیر گروه موضوعی",
                Roles = new List<ApplicationRole>() { operatorRole, executiveRole, contractorRole, inspectorRole, managerRole, mayorRole },
                ShahrbinInstanceId = instanceId,
            };
            _context.Set<Chart>().Add(chart);
        }

        code = 102;
        if (!chartCodes.Contains(code))
        {
            chart = new Chart()
            {
                Code = instanceId * 100000 + code,
                Order = 2,
                Title = "وضعیت درخواست ها بر اساس گروه موضوعی",
                Roles = new List<ApplicationRole>() { operatorRole, executiveRole, contractorRole, inspectorRole, managerRole, mayorRole },
                ShahrbinInstanceId = instanceId,
            };
            _context.Set<Chart>().Add(chart);
        }

        code = 103;
        if (!chartCodes.Contains(code))
        {
            chart = new Chart()
            {
                Code = instanceId * 100000 + code,
                Order = 3,
                Title = "وضعیت درخواست ها بر اساس واحد اجرایی",
                Roles = new List<ApplicationRole>() { operatorRole, executiveRole, contractorRole, inspectorRole, managerRole, mayorRole },
                ShahrbinInstanceId = instanceId,
            };
            _context.Set<Chart>().Add(chart);
        }

        code = 104;
        if (!chartCodes.Contains(code))
        {
            chart = new Chart()
            {
                Code = instanceId * 100000 + code,
                Order = 7,
                Title = "وضعیت درخواست ها بر اساس منطقه",
                Roles = new List<ApplicationRole>() { operatorRole, executiveRole, contractorRole, inspectorRole, managerRole, mayorRole },
                ShahrbinInstanceId = instanceId,
            };
            _context.Set<Chart>().Add(chart);
        }

        code = 201;
        if (!chartCodes.Contains(code))
        {
            chart = new Chart()
            {
                Code = instanceId * 100000 + code,
                Order = 4,
                Title = "زمان رسیدگی بر اساس زیر گروه موضوعی",
                Roles = new List<ApplicationRole>() { operatorRole, executiveRole, contractorRole, inspectorRole, managerRole, mayorRole },
                ShahrbinInstanceId = instanceId,
            };
            _context.Set<Chart>().Add(chart);
        }

        code = 202;
        if (!chartCodes.Contains(code))
        {
            chart = new Chart()
            {
                Code = instanceId * 100000 + code,
                Order = 5,
                Title = "زمان رسیدگی بر اساس گروه موضوعی",
                Roles = new List<ApplicationRole>() { operatorRole, executiveRole, contractorRole, inspectorRole, managerRole, mayorRole },
                ShahrbinInstanceId = instanceId,
            };
            _context.Set<Chart>().Add(chart);
        }

        code = 203;
        if (!chartCodes.Contains(code))
        {
            chart = new Chart()
            {
                Code = instanceId * 100000 + code,
                Order = 6,
                Title = "زمان رسیدگی بر اساس واحد اجرایی",
                Roles = new List<ApplicationRole>() { operatorRole, executiveRole, contractorRole, inspectorRole, managerRole, mayorRole },
                ShahrbinInstanceId = instanceId,
            };
            _context.Set<Chart>().Add(chart);
        }

        code = 204;
        if (!chartCodes.Contains(code))
        {
            chart = new Chart()
            {
                Code = instanceId * 100000 + code,
                Order = 7,
                Title = "زمان رسیدگی بر اساس منطقه",
                Roles = new List<ApplicationRole>() { operatorRole, executiveRole, contractorRole, inspectorRole, managerRole, mayorRole },
                ShahrbinInstanceId = instanceId,
            };
            _context.Set<Chart>().Add(chart);
        }

        code = 302;
        if (!chartCodes.Contains(code))
        {
            chart = new Chart()
            {
                Code = instanceId * 100000 + code,
                Order = 8,
                Title = "تعداد درخواست ها بر اساس ثبت کننده",
                Roles = new List<ApplicationRole>() { operatorRole, executiveRole, contractorRole, inspectorRole, managerRole, mayorRole },
                ShahrbinInstanceId = instanceId,
            };
            _context.Set<Chart>().Add(chart);
        }

        code = 303;
        if (!chartCodes.Contains(code))
        {
            chart = new Chart()
            {
                Code = instanceId * 100000 + code,
                Order = 9,
                Title = "تعداد درخواست ها بر اساس نوع ثبت کننده",
                Roles = new List<ApplicationRole>() { operatorRole, executiveRole, contractorRole, inspectorRole, managerRole, mayorRole },
                ShahrbinInstanceId = instanceId,
            };
            _context.Set<Chart>().Add(chart);
        }

        code = 141;
        if (!chartCodes.Contains(code))
        {
            chart = new Chart()
            {
                Code = instanceId * 100000 + code,
                Order = 1000,
                Title = "نقشه پراکندگی",
                Roles = new List<ApplicationRole>() { operatorRole, executiveRole, contractorRole, inspectorRole, managerRole, mayorRole },
                ShahrbinInstanceId = instanceId,
            };
            _context.Set<Chart>().Add(chart);
        }

        code = 402;
        if (!chartCodes.Contains(code))
        {
            chart = new Chart()
            {
                Code = instanceId * 100000 + code,
                Order = 2001,
                Title = "عملکرد پیمانکاران",
                Roles = new List<ApplicationRole>() { operatorRole, mayorRole },
                ShahrbinInstanceId = instanceId,
            };
            _context.Set<Chart>().Add(chart);
        }


        await _context.SaveChangesAsync();

        return true;
    }
}
