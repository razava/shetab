using AutoMapper;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.IdentityAggregate;
using Domain.Models.Relational.PollAggregate;
using Domain.Models.Relational.ProcessAggregate;
using Domain.Models.Relational.ReportAggregate;

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

        CreateMap<Address, Dtos.AddressDto>();
        CreateMap<Dtos.AddressDto, Address>();

        CreateMap<ApplicationUser, Dtos.ApplicationUserDto>();
        CreateMap<Dtos.ApplicationUserDto, ApplicationUser>();
        CreateMap<ApplicationUser, Dtos.ApplicationUserRestrictedDto>();
        CreateMap<Dtos.ApplicationUserRestrictedDto, ApplicationUser>();

        CreateMap<ApplicationUser, Dtos.GetUserDto>();
        CreateMap<Dtos.GetUserDto, ApplicationUser>();

        CreateMap<ApplicationUser, Dtos.UpdateUserDto>();
        CreateMap<Dtos.UpdateUserDto, ApplicationUser>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Actor, Dtos.ActorDto>();
        CreateMap<Dtos.ActorDto, Actor>();

        CreateMap<ProcessReason, Dtos.ReasonDto>();
        CreateMap<Dtos.ReasonDto, ProcessReason>();

        //this dto moved to Contracts.ReportDtos.cs
        //CreateMap<Dtos.MakeTransitionModel, Dtos.MakeTransitionDto>();
        //CreateMap<Dtos.MakeTransitionDto, Dtos.MakeTransitionModel>();

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
        CreateMap<Province, Dtos.AdministrativeDivisionsDto>();
        CreateMap<County, Dtos.AdministrativeDivisionsDto>();
        CreateMap<District, Dtos.AdministrativeDivisionsDto>();
        CreateMap<City, Dtos.AdministrativeDivisionsDto>();
        CreateMap<Region, Dtos.AdministrativeDivisionsDto>();

        CreateMap<Domain.Models.Relational.TransitionLog, Dtos.TransitionLogDto>();
        CreateMap<Dtos.TransitionLogDto, Domain.Models.Relational.TransitionLog>();

        CreateMap<Message, Dtos.GetMessageDto>();
        CreateMap<Dtos.GetMessageDto, Message>();

        CreateMap<Process, Dtos.ProcessDto>();
        CreateMap<Dtos.ProcessDto, Process>();

        CreateMap<Process, Dtos.ProcessGetDto>();
        CreateMap<Dtos.ProcessGetDto, Process>();

        CreateMap<ProcessStage, Dtos.StageDto>();
        CreateMap<Dtos.StageDto, ProcessStage>();

        CreateMap<ProcessStage, Dtos.GetStageDto>();
        CreateMap<Dtos.GetStageDto, ProcessStage>();

        CreateMap<ProcessTransition, Dtos.TransitionDto>();
        CreateMap<Dtos.TransitionDto, ProcessTransition>();

        CreateMap<Poll, Dtos.CreatePollDto>();
        CreateMap<Dtos.CreatePollDto, Poll>();

        CreateMap<Poll, Dtos.GetPollDto>();
        CreateMap<Dtos.GetPollDto, Poll>();

        CreateMap<PollChoice, Dtos.ChoiceDto>();
        CreateMap<Dtos.ChoiceDto, PollChoice>();

        CreateMap<PollAnswer, Dtos.AnswerDto>();
        CreateMap<Dtos.AnswerDto, PollAnswer>();

        CreateMap<Comment, Dtos.GetCommentForCitizenDto>();
        CreateMap<Dtos.GetCommentForCitizenDto, Comment>();

        CreateMap<Comment, Dtos.CreateCommentDto>();
        CreateMap<Dtos.CreateCommentDto, Comment>();

        CreateMap<Comment, Dtos.GetCommentForOperatorDto>();
        CreateMap<Dtos.GetCommentForOperatorDto, Comment>();

        CreateMap<Region, Dtos.RegionDto>();
        CreateMap<Dtos.RegionDto, Region>();

        CreateMap<Region, Dtos.RegionGetDto>();
        CreateMap<Dtos.RegionGetDto, Region>();

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

        CreateMap<Complaint, Dtos.ComplaintGetDto>();

        CreateMap<Domain.Models.Relational.ComplaintCategory, Dtos.ComplaintCategoryGetDto>();

        CreateMap<ComplaintLog, Dtos.ComplaintLogGetDto>();

        CreateMap<Complaint, Dtos.ComplaintGetInspectorDto>();

        CreateMap<ShahrbinInstance, Dtos.ShahrbinInstanceGetDto>();


    }
}
