namespace Api.Dtos
{
    public class ShahrbinInstanceGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Abbreviation { get; set; }
        public int CityId { get; set; }
        public AdministrativeDivisionsDto City { get; set; }
        public string EnglishName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
