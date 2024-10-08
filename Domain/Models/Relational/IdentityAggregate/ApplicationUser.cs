﻿using Domain.Models.Relational.Common;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Relational.IdentityAggregate;
public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Organization { get; set; } = string.Empty;
    public Media? Avatar { get; set; }
    public string PhoneNumber2 { get; set; } = string.Empty;
    public string NationalId { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public Education Education { get; set; }
    public DateTime? BirthDate { get; set; }
    public string CitizenshipCode { get; set; } = string.Empty;
    public Address? Address { get; set; }
    public DateTime? VerificationSent { get; set; }
    public bool IsHidden { get; set; } = false;
    public bool SmsAlert { get; set; } = false;
    [InverseProperty("LikedBy")]
    public ICollection<Report> ReportsLiked { get; set; } = new List<Report>();
    [InverseProperty("Citizen")]
    public ICollection<Report> Reports { get; set; } = new List<Report>();
    [InverseProperty("Registrant")]
    public ICollection<Report> RegisteredReports { get; set; } = new List<Report>();
    [InverseProperty("Executive")]
    public ICollection<Report> ExecutiveReports { get; set; } = new List<Report>();
    [InverseProperty("Contractor")]
    public ICollection<Report> ContractorReports { get; set; } = new List<Report>();
    [InverseProperty("Inspector")]
    public ICollection<Report> InspectorReports { get; set; } = new List<Report>();
    public string FcmToken { get; set; } = string.Empty;

    //These properties are added to specify contractors relating to each executive. the relation is m to n
    public ICollection<ApplicationUser> Executives { get; set; } = new List<ApplicationUser>();
    public ICollection<ApplicationUser> Contractors { get; set; } = new List<ApplicationUser>();
    public UserFlags Flags { get; set; }
    public int? ShahrbinInstanceId { get; set; }
    public ShahrbinInstance? ShahrbinInstance { get; set; }

    public List<Category> Categories { get; set; } = new List<Category>();
}


public class ReportLikes
{
    public string LikedById { get; set; } = null!;
    public ApplicationUser LikedBy { get; set; } = null!;
    public Guid ReportId { get; set; }
    public Report Report { get; set; } = null!;
}