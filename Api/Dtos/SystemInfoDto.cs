using System.Collections.Generic;

namespace Api.Dtos
{
    public class SystemInfoDto
    {
        public string ApiUrl { get; set; }
        public string MapUrl { get; set; }
        public string CityName { get; set; }
        public string CityNameEn { get; set; }
        public string GovUrl { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string AppUrl1 { get; set; }
        public string AppUrl2 { get; set; }
        public string AppUrl3 { get; set; }
        public string AppUrl4 { get; set; }
        public string AppUrl5 { get; set; }
        public string MapPmiToken { get; set; }
        public string MapAccessToken { get; set; }
        public string AppLogoUrl { get; set; }
        public string MunicipalityLogoUrl { get; set; }
        public List<string> InvolvedLogos { get; set; } = new List<string>();

    }
}
