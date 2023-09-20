using AutoMapper;

namespace Api.Mapping;

public class Profiles : Profile
{
    public Profiles()
    {
        CreateMap<Domain.Models.Relational.Category, Dtos.CategoryDto>();
        CreateMap<Dtos.CategoryDto, Domain.Models.Relational.Category>();

        CreateMap<Domain.Models.Relational.Category, Dtos.CategoryUpdateDto>();
        CreateMap<Dtos.CategoryUpdateDto, Domain.Models.Relational.Category>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Domain.Models.Relational.Category, Dtos.CategoryCreateDto>();
        CreateMap<Dtos.CategoryCreateDto, Domain.Models.Relational.Category>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Domain.Models.Relational.Report, Dtos.CreateReportDto>();
        CreateMap<Dtos.CreateReportDto, Domain.Models.Relational.Report>();

        CreateMap<Domain.Models.Relational.Report, Dtos.UpdateReportDto>();
        CreateMap<Dtos.UpdateReportDto, Domain.Models.Relational.Report>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Domain.Models.Relational.Report, Dtos.OperatorCreateReportDto>();
        CreateMap<Dtos.OperatorCreateReportDto, Domain.Models.Relational.Report>();

        CreateMap<Domain.Models.Relational.Report, Dtos.CitizenGetReportDto>()
            .ForMember(dest => dest.IsLiked, opt => opt.MapFrom(src => src.LikedBy != null && src.LikedBy.Count > 0));
        CreateMap<Dtos.CitizenGetReportDto, Domain.Models.Relational.Report>();

        CreateMap<Domain.Models.Relational.Report, Dtos.AdminGetReportDto>();
        CreateMap<Dtos.AdminGetReportDto, Domain.Models.Relational.Report>();

        CreateMap<Domain.Models.Relational.Report, Dtos.ReportDto>();
        CreateMap<Dtos.ReportDto, Domain.Models.Relational.Report>();

        CreateMap<Domain.Models.Relational.Report, Dtos.GetReportDto>();
        CreateMap<Dtos.GetReportDto, Domain.Models.Relational.Report>();

        CreateMap<Domain.Models.Relational.Address, Dtos.AddressDto>();
        CreateMap<Dtos.AddressDto, Domain.Models.Relational.Address>();

        CreateMap<Domain.Models.Relational.ApplicationUser, Dtos.ApplicationUserDto>();
        CreateMap<Dtos.ApplicationUserDto, Domain.Models.Relational.ApplicationUser>();
        CreateMap<Domain.Models.Relational.ApplicationUser, Dtos.ApplicationUserRestrictedDto>();
        CreateMap<Dtos.ApplicationUserRestrictedDto, Domain.Models.Relational.ApplicationUser>();

        CreateMap<Domain.Models.Relational.ApplicationUser, Dtos.GetUserDto>();
        CreateMap<Dtos.GetUserDto, Domain.Models.Relational.ApplicationUser>();

        CreateMap<Domain.Models.Relational.ApplicationUser, Dtos.UpdateUserDto>();
        CreateMap<Dtos.UpdateUserDto, Domain.Models.Relational.ApplicationUser>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Domain.Models.Relational.Actor, Dtos.ActorDto>();
        CreateMap<Dtos.ActorDto, Domain.Models.Relational.Actor>();

        CreateMap<Domain.Models.Relational.ProcessReason, Dtos.ReasonDto>();
        CreateMap<Dtos.ReasonDto, Domain.Models.Relational.ProcessReason>();

        CreateMap<Dtos.MakeTransitionModel, Dtos.MakeTransitionDto>();
        CreateMap<Dtos.MakeTransitionDto, Dtos.MakeTransitionModel>();

        CreateMap<Dtos.ObjectionModel, Dtos.ObjectionDto>();
        CreateMap<Dtos.ObjectionDto, Dtos.ObjectionModel>();

        CreateMap<Dtos.MoveToStageModel, Dtos.MoveToStageDto>();
        CreateMap<Dtos.MoveToStageDto, Dtos.MoveToStageModel>();

        /*
        CreateMap<DNTPersianUtils.Core.IranCities.Province, Domain.Models.Relational.Province>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ProvinceName));
        CreateMap<DNTPersianUtils.Core.IranCities.County, Domain.Models.Relational.County>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CountyName));
        CreateMap<DNTPersianUtils.Core.IranCities.District, Domain.Models.Relational.District>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.DistrictName));
        CreateMap<DNTPersianUtils.Core.IranCities.City, Domain.Models.Relational.City>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CityName));
        */
        CreateMap<Domain.Models.Relational.Province, Dtos.AdministrativeDivisionsDto>();
        CreateMap<Domain.Models.Relational.County, Dtos.AdministrativeDivisionsDto>();
        CreateMap<Domain.Models.Relational.District, Dtos.AdministrativeDivisionsDto>();
        CreateMap<Domain.Models.Relational.City, Dtos.AdministrativeDivisionsDto>();
        CreateMap<Domain.Models.Relational.Region, Dtos.AdministrativeDivisionsDto>();

        CreateMap<Domain.Models.Relational.TransitionLog, Dtos.TransitionLogDto>();
        CreateMap<Dtos.TransitionLogDto, Domain.Models.Relational.TransitionLog>();

        CreateMap<Domain.Models.Relational.Message, Dtos.GetMessageDto>();
        CreateMap<Dtos.GetMessageDto, Domain.Models.Relational.Message>();

        CreateMap<Domain.Models.Relational.Process, Dtos.ProcessDto>();
        CreateMap<Dtos.ProcessDto, Domain.Models.Relational.Process>();

        CreateMap<Domain.Models.Relational.Process, Dtos.ProcessGetDto>();
        CreateMap<Dtos.ProcessGetDto, Domain.Models.Relational.Process>();

        CreateMap<Domain.Models.Relational.ProcessStage, Dtos.StageDto>();
        CreateMap<Dtos.StageDto, Domain.Models.Relational.ProcessStage>();

        CreateMap<Domain.Models.Relational.ProcessStage, Dtos.GetStageDto>();
        CreateMap<Dtos.GetStageDto, Domain.Models.Relational.ProcessStage>();

        CreateMap<Domain.Models.Relational.ProcessTransition, Dtos.TransitionDto>();
        CreateMap<Dtos.TransitionDto, Domain.Models.Relational.ProcessTransition>();

        CreateMap<Domain.Models.Relational.Poll, Dtos.CreatePollDto>();
        CreateMap<Dtos.CreatePollDto, Domain.Models.Relational.Poll>();

        CreateMap<Domain.Models.Relational.Poll, Dtos.GetPollDto>();
        CreateMap<Dtos.GetPollDto, Domain.Models.Relational.Poll>();

        CreateMap<Domain.Models.Relational.PollChoice, Dtos.ChoiceDto>();
        CreateMap<Dtos.ChoiceDto, Domain.Models.Relational.PollChoice>();

        CreateMap<Domain.Models.Relational.PollAnswer, Dtos.AnswerDto>();
        CreateMap<Dtos.AnswerDto, Domain.Models.Relational.PollAnswer>();

        CreateMap<Domain.Models.Relational.Comment, Dtos.GetCommentForCitizenDto>();
        CreateMap<Dtos.GetCommentForCitizenDto, Domain.Models.Relational.Comment>();

        CreateMap<Domain.Models.Relational.Comment, Dtos.CreateCommentDto>();
        CreateMap<Dtos.CreateCommentDto, Domain.Models.Relational.Comment>();

        CreateMap<Domain.Models.Relational.Comment, Dtos.GetCommentForOperatorDto>();
        CreateMap<Dtos.GetCommentForOperatorDto, Domain.Models.Relational.Comment>();

        CreateMap<Domain.Models.Relational.Region, Dtos.RegionDto>();
        CreateMap<Dtos.RegionDto, Domain.Models.Relational.Region>();

        CreateMap<Domain.Models.Relational.Region, Dtos.RegionGetDto>();
        CreateMap<Dtos.RegionGetDto, Domain.Models.Relational.Region>();

        CreateMap<Domain.Models.Relational.OrganizationalUnit, Dtos.OrganizationalUnitDto>();
        CreateMap<Dtos.OrganizationalUnitDto, Domain.Models.Relational.OrganizationalUnit>();

        //CreateMap<FirebaseAdmin.Messaging.MulticastMessage, Shahrbin.Api.Services.MulticastMessageDto>();
        //CreateMap<Shahrbin.Api.Services.MulticastMessageDto, FirebaseAdmin.Messaging.MulticastMessage>();

        CreateMap<Domain.Models.Relational.Violation, Dtos.ViolationCreateDto>();
        CreateMap<Dtos.ViolationCreateDto, Domain.Models.Relational.Violation>();

        CreateMap<Domain.Models.Relational.Violation, Dtos.ViolationGetDto>();
        CreateMap<Dtos.ViolationGetDto, Domain.Models.Relational.Violation>();

        CreateMap<Domain.Models.Relational.Violation, Dtos.ViolationPutDto>();
        CreateMap<Dtos.ViolationPutDto, Domain.Models.Relational.Violation>();

        CreateMap<Domain.Models.Relational.News, Dtos.NewsDto>();
        CreateMap<Dtos.NewsDto, Domain.Models.Relational.News>();

        CreateMap<Domain.Models.Relational.ApplicationLink, Dtos.ApplicationLinkDto>();
        CreateMap<Dtos.ApplicationLinkDto, Domain.Models.Relational.ApplicationLink>();

        CreateMap<Domain.Models.Relational.QuickAccess, Dtos.QuickAccessDto>();
        CreateMap<Dtos.QuickAccessDto, Domain.Models.Relational.QuickAccess>();

        //Inspection
        CreateMap<Domain.Models.Relational.ComplaintCategory, Dtos.ComplaintCategoryUpsertDto>();
        CreateMap<Dtos.ComplaintCategoryUpsertDto, Domain.Models.Relational.ComplaintCategory>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Domain.Models.Relational.ComplaintOrganizationalUnit, Dtos.ComplaintOrganizationalUnitGetDto>();
        CreateMap<Dtos.ComplaintOrganizationalUnitInsertDto, Domain.Models.Relational.ComplaintOrganizationalUnit>();
        CreateMap<Dtos.ComplaintOrganizationalUnitUpdateDto, Domain.Models.Relational.ComplaintOrganizationalUnit>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Domain.Models.Relational.ComplaintOrganizationalUnit, Dtos.ComplaintOrganizationalUnitReferToDto>();

        CreateMap<Domain.Models.Relational.Complaint, Dtos.ComplaintGetDto>();

        CreateMap<Domain.Models.Relational.ComplaintCategory, Dtos.ComplaintCategoryGetDto>();

        CreateMap<Domain.Models.Relational.ComplaintLog, Dtos.ComplaintLogGetDto>();

        CreateMap<Domain.Models.Relational.Complaint, Dtos.ComplaintGetInspectorDto>();

        CreateMap<Domain.Models.Relational.ShahrbinInstance, Dtos.ShahrbinInstanceGetDto>();


    }
}
