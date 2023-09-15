using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Models.Gov;

public class GovAddress
{
    public Guid Id { get; set; }

    [JsonPropertyName("traceID")]
    public string? TraceID { get; set; }

    [JsonPropertyName("errorCode")]
    public int ErrorCode { get; set; }

    [JsonPropertyName("province")]
    public string? Province { get; set; }

    [JsonPropertyName("enProvince")]
    public string? EnProvince { get; set; }

    [JsonPropertyName("townShip")]
    public string? TownShip { get; set; }

    [JsonPropertyName("zone")]
    public string? Zone { get; set; }

    [JsonPropertyName("village")]
    public string? Village { get; set; }

    [JsonPropertyName("localityType")]
    public string? LocalityType { get; set; }

    [JsonPropertyName("localityName")]
    public string? LocalityName { get; set; }

    [JsonPropertyName("localityCode")]
    public int LocalityCode { get; set; }

    [JsonPropertyName("subLocality")]
    public string? SubLocality { get; set; }

    [JsonPropertyName("street")]
    public string? Street { get; set; }

    [JsonPropertyName("street2")]
    public string? Street2 { get; set; }

    [JsonPropertyName("houseNumber")]
    public int HouseNumber { get; set; }

    [JsonPropertyName("floor")]
    public string? Floor { get; set; }

    [JsonPropertyName("sideFloor")]
    public string? SideFloor { get; set; }

    [JsonPropertyName("buildingName")]
    public string? BuildingName { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("postCode")]
    public string? PostCode { get; set; }
}

public class GovFamily
{
    public Guid Id { get; set; }

    [JsonPropertyName("personNin")]
    public string? PersonNin { get; set; }

    [JsonPropertyName("relationType")]
    public string? RelationType { get; set; }
}

public class GovUserInfo
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("mobile")]
    public string? Mobile { get; set; }

    [JsonPropertyName("address")]
    public GovAddress Address { get; set; }

    [JsonPropertyName("family")]
    public List<GovFamily> Family { get; set; }

    [JsonPropertyName("nationalId")]
    public string? NationalId { get; set; }

    [JsonPropertyName("firstName")]
    public string? FirstName { get; set; }

    [JsonPropertyName("lastName")]
    public string? LastName { get; set; }

    [JsonPropertyName("fatherName")]
    public string? FatherName { get; set; }

    [JsonPropertyName("gender")]
    public string? Gender { get; set; }

    [JsonPropertyName("birthDate")]
    public string? BirthDate { get; set; }

    [JsonPropertyName("birthDateShamsi")]
    public string? BirthDateShamsi { get; set; }

    [JsonPropertyName("shenasnamehNo")]
    public string? ShenasnamehNo { get; set; }

    [JsonPropertyName("birthCertificatenumber")]
    public string? BirthCertificatenumber { get; set; }

    [JsonPropertyName("postalCode")]
    public string? PostalCode { get; set; }

    [JsonPropertyName("province")]
    public string? Province { get; set; }

    [JsonPropertyName("enProvince")]
    public string? EnProvince { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("roles")]
    [NotMapped]
    public List<string> Roles { get; set; }

    [JsonPropertyName("hasSignPrivateKey")]
    public bool HasSignPrivateKey { get; set; }

    [JsonPropertyName("subsidy")]
    public GovSubsidy Subsidy { get; set; }
}

public class GovSubsidy
{
    public Guid Id { get; set; }

    [JsonPropertyName("registered")]
    public bool Registered { get; set; }

    [JsonPropertyName("included")]
    public bool Included { get; set; }

    [JsonPropertyName("apply")]
    public bool Apply { get; set; }

    [JsonPropertyName("decile")]
    public int Decile { get; set; }

    [JsonPropertyName("verifyFamily")]
    public bool VerifyFamily { get; set; }

    [JsonPropertyName("familyHeadNationalId")]
    public string? FamilyHeadNationalId { get; set; }
}
