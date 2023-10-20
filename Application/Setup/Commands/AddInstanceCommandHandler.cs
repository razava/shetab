using Application.Common.Interfaces.Persistence;
using ClosedXML.Excel;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.IdentityAggregate;
using Domain.Models.Relational.ProcessAggregate;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Application.Setup.Commands;

internal sealed class AddInstanceCommandHandler : IRequestHandler<AddInstanceCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly DbContext _context;
    public AddInstanceCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _context = _unitOfWork.DbContext;
    }
    public Task<bool> Handle(AddInstanceCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
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
            throw new Exception($"Invalid city: {inputInfo.CityName}");
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
            throw new Exception($"Duplicate instance: {instance.Name}, {city.Name}");
        }
        _context.Set<ShahrbinInstance>().Add(instance);
        await _context.SaveChangesAsync();

        InputForCategory inputForCategory = null;
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
                    SubCategories = new List<InputForCategory>()
                };
                inputForCategories.Add(inputForCategory);
            }
            else
            {
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

        var jsonString = System.IO.File.ReadAllText("AdministrativeDivisions", System.Text.Encoding.Unicode);
        var provinces = JsonSerializer.Deserialize<List<Province>>(jsonString);
        _context.Set<Province>().AddRange(provinces);
        await _context.SaveChangesAsync();
    }

    private async Task<bool> initProcessesFromExcel(ShahrbinInstance instance, List<InputForExecutive> inputForExecutives)
    {
        ApplicationUser user = null;
        Actor actor = null;



        var regions = await _context.Set<Region>()
            .Where(p => p.CityId == instance.CityId)
            .ToListAsync();
        await initDefaultsFromExcel(regions, instance);

        InputForExecutive inputForExecutive;

        if (_context.Set<Process>().Count(p => p.ShahrbinInstanceId == instance.Id) == 0)
        {
            //var typicalProcessManager = new TypicalProcessManager();
            TypicalProcessCreateDto typicalProcess;
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
                    actor = new Actor()
                    {
                        Identifier = user.Id,
                        Type = ActorType.Person,
                    };
                    actor.Regions = regions;
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

                typicalProcess = new TypicalProcessCreateDto()
                {
                    Title = inputForExecutive.Title,
                    Code = $"{inputForExecutive.Code}",
                    ActorIds = actors.Select(p => p.Id).ToList(),
                    ShahrbinInstance = instance
                };

                await AddTypicalProcess(typicalProcess);
            }
        }

        return true;
    }

    private async Task initDefaultsFromExcel(List<Region> regions, ShahrbinInstance instance)
    {
        //Init roles
        var rolesInfo = new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("Citizen", "شهروند"),
                new Tuple<string, string>("Operator", "اپراتور"),
                new Tuple<string, string>("Executive", "واحد اجرایی"),
                new Tuple<string, string>("Contractor", "پیمانکار"),
                new Tuple<string, string>("Manager", "واحد سازمانی"),
                new Tuple<string, string>("Advertiser", "مدیر تبلیغات"),
                new Tuple<string, string>("Mayor", "شهردار"),
                new Tuple<string, string>("Admin", "مدیر فنی"),
                new Tuple<string, string>("PowerUser",""),
                new Tuple<string, string>("Inspector", "بازرس")
            };


        var roles = new List<ApplicationRole>();
        foreach (var role in rolesInfo)
        {
            if (!await _userRepository.RoleExistsAsync(role.Item1))
            {
                var r = new ApplicationRole() { Name = role.Item1, Title = role.Item2 };
                await _userRepository.CreateRoleAsync(r);
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
                Reports = null,
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
                Reports = null,
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
            Title = "ریشه"
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
                ParentId = rootCategory.Id
            };
            foreach (var childCat in parentCat.SubCategories)
            {
                var process = processes.Where(p => p.Code == childCat.ProcessTitle).FirstOrDefault();
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
                    Description = childCat.Description
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

    public async Task<Process> AddTypicalProcess(TypicalProcessCreateDto typicalProcess)
    {
        Process process;
        ProcessStage stageCitizen, stageOperator, stageExecutive, stageContractor, stageInspector;
        List<ProcessReason> ReasonList;
        ProcessTransition transition12, transition21;
        BotActor bot12, bot21;
        Actor actorCitizenRole;
        Actor actorOperatorRole;
        Actor actorExecutiveRole;
        Actor actorContractorRole;
        Actor actorInspectorRole;

        var roleId = (await _userRepository.FindByNameAsync("Citizen")).Id;
        actorCitizenRole = await _context.Set<Actor>()
            .Where(p => p.Type == ActorType.Role && p.Identifier == roleId)
            .FirstOrDefaultAsync();

        roleId = (await _userRepository.FindByNameAsync("Operator")).Id;
        actorOperatorRole = await _context.Set<Actor>()
            .Where(p => p.Type == ActorType.Role && p.Identifier == roleId)
            .FirstOrDefaultAsync();

        roleId = (await _userRepository.FindByNameAsync("Executive")).Id;
        actorExecutiveRole = await _context.Set<Actor>()
            .Where(p => p.Type == ActorType.Role && p.Identifier == roleId)
            .FirstOrDefaultAsync();

        roleId = (await _userRepository.FindByNameAsync("Contractor")).Id;
        actorContractorRole = await _context.Set<Actor>()
            .Where(p => p.Type == ActorType.Role && p.Identifier == roleId)
            .FirstOrDefaultAsync();

        roleId = (await _userRepository.FindByNameAsync("Inspector")).Id;
        actorInspectorRole = await _context.Set<Actor>()
            .Where(p => p.Type == ActorType.Role && p.Identifier == roleId)
            .FirstOrDefaultAsync();

        //Create process related to an executive
        process = new Process()
        {
            ShahrbinInstance = typicalProcess.ShahrbinInstance,
            Title = "فرآیند (" + typicalProcess.Title + ")",
            Code = typicalProcess.Code
        };

        stageCitizen = new ProcessStage()
        {
            Name = "Citizen",
            DisplayName = "شهروند",
            Order = 1,
            Status = "ثبت درخواست در سامانه شهربین",
            DisplayRoleId = actorCitizenRole.Identifier,
            Actors = new List<Actor>() { actorCitizenRole }
        };
        process.Stages.Add(stageCitizen);


        stageOperator = new ProcessStage()
        {
            Name = "Operator",
            DisplayName = "اپراتور",
            Order = 10,
            Status = "در انتظار تأیید اپراتور",
            DisplayRoleId = actorOperatorRole.Identifier,
            Actors = new List<Actor>() { actorOperatorRole }
        };
        process.Stages.Add(stageOperator);

        var actors = await _context.Set<Actor>().Where(p => typicalProcess.ActorIds.Contains(p.Id)).ToListAsync();

        stageExecutive = new ProcessStage()
        {
            Name = "Executive",
            DisplayName = "واحد اجرایی",
            Order = 20,
            Status = "ارجاع به واحد اجرایی",
            DisplayRoleId = actorExecutiveRole.Identifier,
            Actors = actors
        };
        process.Stages.Add(stageExecutive);


        stageContractor = new ProcessStage()
        {
            Name = "Contractor",
            DisplayName = "پیمانکار",
            Order = 30,
            Status = "ارجاع به پیمانکار",
            DisplayRoleId = actorContractorRole.Identifier,
            Actors = new List<Actor>() { actorContractorRole }
        };
        process.Stages.Add(stageContractor);


        stageInspector = new ProcessStage()
        {
            Name = "Inspector",
            DisplayName = "واحد بازرسی",
            Order = 40,
            Status = "ارجاع به واحد بازرسی",
            DisplayRoleId = actorInspectorRole.Identifier,
            Actors = new List<Actor>() { actorInspectorRole }
        };
        process.Stages.Add(stageInspector);


        ////////////////////////////
        ///
        ReasonList = new List<ProcessReason>()
                    {
                        new ProcessReason()
                        {
                            Title = "پیش فرض",
                            Description = "توضیحات",
                            ReasonMeaning = ReasonMeaning.Ok
                        }
                    };
        transition12 = new ProcessTransition()
        {
            From = stageCitizen,
            To = stageOperator,
            Message = "ارجاع به اپراتور",
            Title = "1-2",
            ReasonList = ReasonList,
            ReportState = ReportState.Live,
            CanSendMessageToCitizen = false,
            Order = 1
        };

        process.Transitions.Add(transition12);

        process.Transitions.Add(new ProcessTransition()
        {
            From = stageOperator,
            To = stageExecutive,
            Message = "ارجاع به واحد اجرایی",
            Title = "2-3",
            ReasonList = new List<ProcessReason>()
                        {
                            new ProcessReason(){ Title = "جهت بررسی", ReasonMeaning = ReasonMeaning.Ok }
                        },
            ReportState = ReportState.Live,
            CanSendMessageToCitizen = false,
            Order = 1
        });

        transition21 = new ProcessTransition()
        {
            From = stageOperator,
            To = stageCitizen,
            Message = "ارجاع به شهروند",
            Title = "2-1",
            ReasonList = new List<ProcessReason>()
                        {
                            new ProcessReason(){ Title = "رسیدگی شد", ReasonMeaning = ReasonMeaning.Ok },
                            new ProcessReason(){ Title = "رد شد", ReasonMeaning = ReasonMeaning.Nok }
                        },
            ReportState = ReportState.Finished,
            CanSendMessageToCitizen = false,
            Order = 2
        };
        process.Transitions.Add(transition21);

        process.Transitions.Add(new ProcessTransition()
        {
            From = stageExecutive,
            To = stageOperator,
            Message = "ارجاع به اپراتور",
            Title = "3-2",
            ReasonList = new List<ProcessReason>()
                        {
                            new ProcessReason(){ Title = "رسیدگی شد", ReasonMeaning = ReasonMeaning.Ok },
                            new ProcessReason(){ Title = "خارج از حوزه", ReasonMeaning = ReasonMeaning.Nok },
                            new ProcessReason(){ Title = "رد شد", ReasonMeaning = ReasonMeaning.Nok }
                        },
            ReportState = ReportState.Live,
            CanSendMessageToCitizen = true,
            Order = 2
        });


        process.Transitions.Add(new ProcessTransition()
        {
            From = stageExecutive,
            To = stageContractor,
            Message = "ارجاع به پیمانکار",
            Title = "3-4",
            ReasonList = new List<ProcessReason>()
                        {
                            new ProcessReason(){ Title = "جهت اجرا", ReasonMeaning = ReasonMeaning.Ok }
                        },
            ReportState = ReportState.Live,
            CanSendMessageToCitizen = true,
            Order = 1
        });

        process.Transitions.Add(new ProcessTransition()
        {
            From = stageContractor,
            To = stageExecutive,
            Message = "ارجاع به واحد اجرایی",
            Title = "4-3",
            ReasonList = new List<ProcessReason>()
                        {
                            new ProcessReason(){ Title = "انجام شد", ReasonMeaning = ReasonMeaning.Ok },
                            new ProcessReason(){ Title = "امکانپذیر نیست", ReasonMeaning = ReasonMeaning.Nok },

                        },
            ReportState = ReportState.Live,
            CanSendMessageToCitizen = false,
            Order = 1
        });

        process.Transitions.Add(new ProcessTransition()
        {
            From = stageCitizen,
            To = stageInspector,
            Message = "ارجاع به واحد بازرسی",
            Title = "1-5",
            ReasonList = new List<ProcessReason>()
                        {
                            new ProcessReason(){ Title = "عدم رضایت از نحوه رسیدگی", ReasonMeaning = ReasonMeaning.Nok },
                        },
            ReportState = ReportState.Review,
            CanSendMessageToCitizen = false,
            TransitionType = TransitionType.Objection,
            Order = 4
        });
        process.Transitions.Add(new ProcessTransition()
        {
            From = stageInspector,
            To = stageCitizen,
            Message = "ارجاع به شهروند",
            Title = "5-1",
            ReasonList = new List<ProcessReason>()
                        {
                            new ProcessReason(){ Title = "اعتراض وارد نیست", ReasonMeaning = ReasonMeaning.Nok },
                        },
            ReportState = ReportState.Accepted,
            CanSendMessageToCitizen = false,
            Order = 4
        });
        process.Transitions.Add(new ProcessTransition()
        {
            From = stageInspector,
            To = stageOperator,
            Message = "ارجاع به اپراتور",
            Title = "5-2",
            ReasonList = new List<ProcessReason>()
                        {
                            new ProcessReason(){ Title = "بررسی مجدد", ReasonMeaning = ReasonMeaning.Nok },
                        },
            ReportState = ReportState.Live,
            CanSendMessageToCitizen = false,
            Order = 4
        });
        process.Transitions.Add(new ProcessTransition()
        {
            From = stageInspector,
            To = stageExecutive,
            Message = "ارجاع به واحد اجرایی",
            Title = "5-3",
            ReasonList = new List<ProcessReason>()
                        {
                            new ProcessReason(){ Title = "بررسی مجدد", ReasonMeaning = ReasonMeaning.Nok },
                        },
            ReportState = ReportState.Live,
            CanSendMessageToCitizen = false,
            Order = 4
        });
        process.Transitions.Add(new ProcessTransition()
        {
            From = stageInspector,
            To = stageContractor,
            Message = "ارجاع به پیمانکار",
            Title = "5-4",
            ReasonList = new List<ProcessReason>()
                        {
                            new ProcessReason(){ Title = "بررسی مجدد", ReasonMeaning = ReasonMeaning.Nok },
                        },
            ReportState = ReportState.Live,
            CanSendMessageToCitizen = false,
            Order = 4
        });

        ////////////////
        bot12 = new BotActor()
        {
            Transition = transition12,
            Actors = stageOperator.Actors,
            MessageToCitizen = null,
            Priority = Priority.Normal,
            ReasonId = null,
            Visibility = Visibility.Operators
        };
        _context.Set<BotActor>().Add(bot12);

        stageCitizen.Actors.Add(new Actor()
        {
            Identifier = bot12.Id,
            Type = ActorType.Auto
        });

        bot21 = new BotActor()
        {
            Transition = transition21,
            Actors = stageCitizen.Actors,
            MessageToCitizen = null,
            Priority = null,
            ReasonId = null,
            Visibility = null,
            ReasonMeaning = ReasonMeaning.Ok,
        };
        _context.Set<BotActor>().Add(bot21);

        stageOperator.Actors.Add(new Actor()
        {
            Identifier = bot21.Id,
            Type = ActorType.Auto
        });

        ///////////////////
        ///
        _context.Set<Process>().Add(process);
        await _context.SaveChangesAsync();

        return process;
    }

    public async Task UpdateTypicalProcess(int id, TypicalProcessCreateDto typicalProcess)
    {
        Process process;
        ProcessStage stageExecutive;


        //Create process related to an executive
        process = await _context.Set<Process>()
            .Where(p => p.Id == id)
            .Include(p => p.Stages)
            .ThenInclude(p => p.Actors)
            .SingleOrDefaultAsync();

        process.Title = typicalProcess.Title;
        process.Code = typicalProcess.Code;




        var actors = await _context.Set<Actor>().Where(p => typicalProcess.ActorIds.Contains(p.Id)).ToListAsync();

        stageExecutive = process.Stages.Where(p => p.Name == "Executive").FirstOrDefault();

        stageExecutive.Actors = actors;


        ///////////////////
        ///
        _context.Entry(process).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public class TypicalProcessCreateDto
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public List<int> ActorIds { get; set; }
        public ShahrbinInstance ShahrbinInstance { get; set; }
    }
}
